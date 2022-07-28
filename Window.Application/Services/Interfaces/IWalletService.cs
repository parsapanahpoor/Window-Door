using Window.Domain.Entities.Wallet;
using Window.Domain.ViewModels.Admin.Wallet;

namespace Window.Application.Interfaces;

public interface IWalletService
{
    #region Wallet

    Task<FilterWalletViewModel> FilterWalletsAsync(FilterWalletViewModel filter);

    Task<int?> GetSumUserWalletAsync(ulong userId);

    Task<AdminEditWalletViewModel?> GetWalletForEditAsync(ulong walletId);

    Task<AdminCreateWalletResponse> CreateWalletAsync(AdminCreateWalletViewModel model);

    Task<AdminEditWalletResponse> EditWalletAsync(AdminEditWalletViewModel model);

    Task ConfirmPayment(ulong payId, string authority , string refId);

    Task<bool> RemoveWalletAsync(ulong walletId);

    Task<ulong> ChargeUserWallet(AdminCreateWalletViewModel wallet);

    Task<WalletViewModel> GetWalletById(ulong id);

    Task UpdateWalletToken(ulong walletId, string token);

    Task UpdateWalletDataWithBankInfo(WalletViewModel wallet, VerifyPaymentResultViewModel verify);


    Task AddTransaction(Wallet wallet);

    Task<int> GetUserTotalDepositTransactions(ulong userId);

    Task<int> GetUserTotalWithdrawTransactions(ulong userId);

    Task<int> GetUserWalletBalance(ulong userId);

    #endregion
}