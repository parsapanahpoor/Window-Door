namespace Window.Domain.ViewModels.Site.Shop.SellerDetail;

public record SellerDetailDTO
{
    #region properties

    public List<ShopCard> ShopCards { get; set; }

    public UserDetail UserDetail { get; set; }

    #endregion
}

public record UserDetail
{
    #region properties

    public ulong SellerUserId{ get; set; }

    public string SellerUsername { get; set; }

    public string? ShopCenterName { get; set; }

    public string SellerMobile { get; set; }

    public DateTime RegisterDate { get; set; }

    public string? SellerImageName { get; set; }

    #endregion
}

public record ShopCard
{
    #region properties

    public ulong ProductId { get; set; }

    public string ShopProductName { get; set; }

    public string ColorName { get; set; }

    public string BrandName { get; set; }

    public decimal Price { get; set; }

    public string ProductImage { get; set; }

    #endregion
}
