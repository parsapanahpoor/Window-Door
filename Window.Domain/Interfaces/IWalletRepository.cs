using Window.Domain.Entities.Wallet;
using Window.Domain.ViewModels.Admin.Wallet;

namespace Window.Domain.Interfaces;

public interface IWalletRepository
{
    #region Wallet

    //Update Wallet With Calculate Balance
    Task UpdateWalletWithCalculateBalance(Wallet wallet);

    //Find Wallet Transaction For Redirect To The Bank Portal 
    Task<Wallet?> FindWalletTransactionForRedirectToTheBankPortal(ulong userId, GatewayType gateway, string authority, int amount);

    //Create Wallet Data
    Task CreateWalletData(WalletData walletData);

    Task<FilterWalletViewModel> FilterWalletsAsync(FilterWalletViewModel filter);

    Task<Wallet?> GetWalletByWalletIdAsync(ulong walletId);

    Task<int> GetSumUserWalletAsync(ulong userId);

    Task<AdminEditWalletViewModel?> GetWalletForEditAsync(ulong walletId);

    Task CreateWalletAsync(Wallet wallet);

    Task EditWalletAsync(Wallet wallet);

    Task ConfirmPayment(ulong payId , string authority ,string refId);

    Task<Wallet> GetWalletById(ulong id);

    Task<ulong> CreateWallet(Wallet charge);

    Task UpdateWalletToken(ulong walletId, string token);

    Task UpdateWalletDataWithBankInfo(Wallet wallet, VerifyPaymentResultViewModel verify);

    Task AddTransaction(Wallet charge);

    Task<int> GetUserTotalDepositTransactions(ulong userId);

    Task<int> GetUserTotalWithdrawTransactions(ulong userId);

    Task<int> GetUserWalletBalance(ulong userId);

    #endregion

    #region Save Changes

    Task SaveChangesAsync();

    #endregion
}