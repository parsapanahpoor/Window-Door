using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Market;
using Window.Domain.ViewModels.Seller.PersonalInfo;
using Window.Web.Areas.Seller.Controllers;

namespace Window.Web.Areas.Seller;

public enum SellerNoticeLevel { Error, Warning, Info, Success }

public sealed record SellerLink(string Controller, string Action, string Label, string Icon)
{
    public bool IsStoreManagement => Controller == "SellerPersonalInfo" && Action == "PageOfManageSellerInformation";
}

public sealed record SellerNotice(SellerNoticeLevel Level, string Title, string Message,
    bool TranslateMessage = true, SellerLink? Action = null, StoreProfileProgress? Progress = null)
{
    public string Icon => Level switch
    {
        SellerNoticeLevel.Error => "fa-times-circle",
        SellerNoticeLevel.Warning => "fa-exclamation-triangle",
        SellerNoticeLevel.Info => "fa-info-circle",
        _ => "fa-check-circle"
    };
}

public sealed record SellerPanel(User User, IReadOnlyList<SellerNotice> Notices, IReadOnlyList<SellerLink> ExtraLinks, StoreProfileProgress? StoreProgress)
{
    public static IReadOnlyList<SellerLink> PrimaryLinks { get; } =
    [
        new("Home", "Index", "Dashboard", "fa-th-large"),
        new("ShopOrder", "ManageOrderPage", "Orders", "fa-shopping-cart"),
        new("Ticket", "FilterTickets", "Support", "fa-ticket")
    ];

    public static (string Label, string Style, string Icon) ApprovalBadge(StoreProfileProgress progress) => progress.State switch
    {
        MarketPersonalsInfoState.WaitingForConfirmPersonalInformations => ("Store under review", "review", "fa-clock-o"),
        MarketPersonalsInfoState.Rejected => ("Store needs correction", "correction", "fa-exclamation-circle"),
        MarketPersonalsInfoState.ActiveMarketAccount or MarketPersonalsInfoState.AcceptedPersonalInformation
            => ("Store verified", "verified", "fa-check-circle"),
        MarketPersonalsInfoState.WaitingForPyedFromSeller => ("Store fee pending badge", "review", "fa-clock-o"),
        MarketPersonalsInfoState.DisAcctiveMarketAccount => ("Store account inactive", "review", "fa-pause-circle"),
        _ when progress.CompletedSteps == StoreProfileProgress.TotalSteps => ("Store not yet verified", "review", "fa-clock-o"),
        _ => ("Store profile incomplete", "correction", "fa-circle-o")
    };

    public static SellerPanel Create(User user, string? state, bool isMaster, ITempDataDictionary tempData,
        StoreProfileProgress? storeProgress = null)
    {
        var profile = new SellerLink("Account", "EditProfile", "Edit profile", "fa-user");
        var store = new SellerLink("SellerPersonalInfo", "PageOfManageSellerInformation", "Store information", "fa-cog");
        var notices = new List<SellerNotice>();
        var accountNotice = state switch
        {
            "NewUser" => new SellerNotice(SellerNoticeLevel.Warning, "Complete store information",
                "Complete your store information to activate seller tools.", Action: isMaster ? store : null),
            "WatingForConfirmInformations" => new SellerNotice(SellerNoticeLevel.Info, "Store information under review",
                "Support is reviewing your submitted information."),
            "WaitingForPyedFromSeller" or "AcceptedPersonalInformation" => new SellerNotice(SellerNoticeLevel.Warning,
                "Seller fee pending", "Pay the seller fee to activate your account."),
            "Rejected" => new SellerNotice(SellerNoticeLevel.Error, "Store information needs correction",
                "Support could not approve your information. Review your store details.", Action: isMaster ? store : null),
            "DisAcctiveSellerAccount" => new SellerNotice(SellerNoticeLevel.Error, "Seller balance expired",
                "Recharge your account to resume using seller tools."),
            _ => null
        };
        if (isMaster && storeProgress is { Percent: < 100 })
        {
            var nextAction = !storeProgress.HasProfile ? "AddPersonalInfo"
                : storeProgress.State != MarketPersonalsInfoState.WaitingForCompleteInfoFromSeller ? "ListOfPersonalInfo"
                : !storeProgress.HasLinks ? "AddSellerPersonalLinks" : "AddSellerWorkSampleTitle";
            notices.Add(new(SellerNoticeLevel.Warning, "Finish your store profile",
                "Complete the remaining steps to finish your store profile.",
                Action: new("SellerPersonalInfo", nextAction, "Continue store setup", "fa-arrow-circle-left"),
                Progress: storeProgress));
        }
        if (accountNotice != null && (state != "NewUser" || !notices.Any(notice => notice.Progress != null)))
            notices.Add(accountNotice);
        if (string.IsNullOrWhiteSpace(user.ShopName))
            notices.Add(new(SellerNoticeLevel.Warning, "Add your store name",
                "Enter your store name in your profile so customers can see it.", Action: profile));

        (string Key, SellerNoticeLevel Level)[] flashKeys =
        [
            (SellerBaseController.ErrorMessage, SellerNoticeLevel.Error), (SellerBaseController.SwalError, SellerNoticeLevel.Error),
            (SellerBaseController.WarningMessage, SellerNoticeLevel.Warning), ("warning", SellerNoticeLevel.Warning),
            (SellerBaseController.InfoMessage, SellerNoticeLevel.Info), (SellerBaseController.SwalInfo, SellerNoticeLevel.Info),
            (SellerBaseController.SuccessMessage, SellerNoticeLevel.Success), (SellerBaseController.SwalSuccess, SellerNoticeLevel.Success)
        ];
        var flashMessages = new HashSet<(SellerNoticeLevel, string)>();
        foreach (var (key, level) in flashKeys)
        {
            if (tempData.TryGetValue(key, out var value) && value is string message && !string.IsNullOrWhiteSpace(message)
                && flashMessages.Add((level, message)))
                notices.Add(new(level, level.ToString(), message, TranslateMessage: false));

            tempData.Remove(key);
        }

        var links = new List<SellerLink> { new("OrderCheque", "ManageOrderChequesPages", "Manage cheques", "fa-credit-card"), profile };
        if (isMaster)
        {
            links.AddRange([
                new("User", "Index", "Employees", "fa-users"),
                new("User", "AddNewUser", "Add employee", "fa-user-plus"),
                store
            ]);
            if (state == "ActiveSellerAccount")
            {
                links.AddRange([
                    new("Contarct", "ListOfContracts", "Contract requests", "fa-file-text-o"),
                    new("Product", "FilterProducts", "Inquiry products", "fa-list-ul"),
                    new("Product", "CreateProduct", "Add inquiry product", "fa-plus"),
                    new("Product", "CreateGlassPricing", "Glass pricing", "fa-th-large"),
                    new("ShopProduct", "FilterShopProducts", "Store products", "fa-cubes"),
                    new("ShopProduct", "CreateProduct", "Add store product", "fa-plus-square")
                ]);
            }
        }

        return new(user, notices.OrderBy(notice => notice.Level).ToArray(), links, isMaster ? storeProgress : null);
    }
}
