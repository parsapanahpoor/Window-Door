namespace Window.Domain.ViewModels.Site.Shop.Landing;

public record ShopCategoriesDTO
{
    #region properties

    public ulong ShopCategoryId { get; set; }

    public string ShopCategoryTitle { get; set; }

    public ulong? ParentId { get; set; }

    public bool ShowOnSiteLanding { get; set; }

    public decimal Priority { get; set; }

    #endregion
}
