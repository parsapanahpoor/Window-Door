using Window.Application.Interfaces;
using Window.Domain.Entities.Wallet;
using Window.Domain.Interfaces;
using Window.Domain.ViewModels.Admin.Wallet;

namespace Window.Application.Services;

public class WalletService : IWalletService
{
    #region Ctor

    private readonly IWalletRepository _walletRepository;
    private readonly IUserRepository _userRepository;

    public WalletService(IWalletRepository walletRepository, IUserRepository userRepository)
    {
        _walletRepository = walletRepository;
        _userRepository = userRepository;
    }

    #endregion

    #region Wallet

    //Update Wallet And Calculate User Balance After Banking Payment
    public async Task UpdateWalletAndCalculateUserBalanceAfterBankingPayment(Wallet wallet)
    {
        #region Update Wallet Fields

        wallet.IsFinally = true;

        #endregion

        #region Update Wallet 

        await _walletRepository.UpdateWalletWithCalculateBalance(wallet);

        #endregion
    }

    //Find Wallet Transaction For Redirect To The Bank Portal 
    public async Task<Wallet?> FindWalletTransactionForRedirectToTheBankPortal(ulong userId, GatewayType gateway, string authority, int amount)
    {
        return await _walletRepository.FindWalletTransactionForRedirectToTheBankPortal(userId, gateway, authority, amount);
    }

    //Create New Wallet Transaction For Redirext To The Bank Portal
    public async Task CreateNewWalletTransactionForRedirextToTheBankPortal(ulong userId, int price, GatewayType gateway, string authority, string description, ulong? requestId)
    {
        #region Fill Wallet 

        var wallet = new Wallet
        {
            UserId = userId,
            TransactionType = TransactionType.Deposit,
            GatewayType = gateway,
            PaymentType = PaymentType.ChargeWallet,
            Price = price,
            Description = description,
            IsFinally = false,
        };

        #endregion

        #region Add Wallet Method 

        await _userRepository.CreateWalletWithoutCalculate(wallet);

        #endregion

        #region Fill Wallet Data 

        var walletData = new WalletData
        {
            GatewayType = gateway,
            TrackingCode = authority,
            WalletId = wallet.Id
        };

        #endregion

        #region Add Wallet Data Method

        await _walletRepository.CreateWalletData(walletData);

        #endregion
    }

    public Task<FilterWalletViewModel> FilterWalletsAsync(FilterWalletViewModel filter)
    {
        return _walletRepository.FilterWalletsAsync(filter);
    }

    public async Task<int?> GetSumUserWalletAsync(ulong userId)
    {
        if (!await _userRepository.IsUserExist(userId))
        {
            return null;
        }

        return await _walletRepository.GetSumUserWalletAsync(userId);
    }

    public async Task<AdminCreateWalletResponse> CreateWalletAsync(AdminCreateWalletViewModel model)
    {
        if (!await _userRepository.IsUserExist(model.UserId))
        {
            return AdminCreateWalletResponse.UserNotFound;
        }

        var wallet = new Wallet
        {
            UserId = model.UserId,
            TransactionType = model.TransactionType,
            GatewayType = model.GatewayType,
            PaymentType = model.PaymentType,
            Price = model.Price,
            Description = model.Description,
            IsFinally = true
        };

        await _walletRepository.CreateWalletAsync(wallet);
        return AdminCreateWalletResponse.Success;
    }

    public async Task<AdminEditWalletResponse> EditWalletAsync(AdminEditWalletViewModel model)
    {
        var wallet = await _walletRepository.GetWalletByWalletIdAsync(model.WalletId);

        if (wallet == null)
        {
            return AdminEditWalletResponse.WalletNotFound;
        }

        wallet.TransactionType = model.TransactionType;
        wallet.GatewayType = model.GatewayType;
        wallet.PaymentType = model.PaymentType;
        wallet.Price = model.Price;
        wallet.Description = model.Description;

        await _walletRepository.EditWalletAsync(wallet);
        return AdminEditWalletResponse.Success;
    }

    public async Task<bool> RemoveWalletAsync(ulong walletId)
    {
        var wallet = await _walletRepository.GetWalletByWalletIdAsync(walletId);

        if (wallet == null)
        {
            return false;
        }

        wallet.IsDelete = true;
        await _walletRepository.EditWalletAsync(wallet);

        return true;
    }

    public Task<AdminEditWalletViewModel?> GetWalletForEditAsync(ulong walletId)
    {
        return _walletRepository.GetWalletForEditAsync(walletId);
    }

    public async Task<ulong> ChargeUserWallet(AdminCreateWalletViewModel wallet)
    {
        if (await _userRepository.IsUserExist(wallet.UserId))
        {
            var charge = new Wallet
            {
                UserId = wallet.UserId,
                TransactionType = wallet.TransactionType,
                GatewayType = wallet.GatewayType,
                PaymentType = wallet.PaymentType,
                Price = wallet.Price,
                Description = wallet.Description,
            };
            return await _walletRepository.CreateWallet(charge);
        }

        return 0;
    }

    public async Task<WalletViewModel> GetWalletById(ulong id)
    {
        var wallet= await _walletRepository.GetWalletById(id);

        var newWallet = new WalletViewModel()
        {
            Id = wallet.Id,
            Description = wallet.Description,
            GatewayType = wallet.GatewayType,
            Price = wallet.Price,
            TransactionType = wallet.TransactionType,
            TransactionWay = wallet.TransactionWay,
            PaymentType = wallet.PaymentType,
        };
        return newWallet;
    }

    public async Task ConfirmPayment(ulong payId, string authority, string refId)
    {
       await _walletRepository.ConfirmPayment(payId, authority,refId);
    }

    public async Task AddTransaction(Wallet wallet)
    {
        await _walletRepository.AddTransaction(wallet);
    }

    public async Task UpdateWalletToken(ulong walletId, string token)
    {
      await _walletRepository.UpdateWalletToken(walletId, token);
    }

    public async Task UpdateWalletDataWithBankInfo(WalletViewModel wallet, VerifyPaymentResultViewModel verify)
    {
        var editedWallet = new Wallet()
        {
            IsFinally = true
        };
        await _walletRepository.UpdateWalletDataWithBankInfo(editedWallet, verify);
    }

    public async Task<int> GetUserTotalDepositTransactions(ulong userId)
    {
        return await  _walletRepository.GetUserTotalDepositTransactions(userId);
    }

    public async Task<int> GetUserTotalWithdrawTransactions(ulong userId)
    {
        return await _walletRepository.GetUserTotalWithdrawTransactions(userId);
    }

    public async Task<int> GetUserWalletBalance(ulong userId)
    {
        return await _walletRepository.GetUserTotalDepositTransactions(userId);
    }

    #endregion
}