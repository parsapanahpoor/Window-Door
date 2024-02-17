namespace Window.Domain.ViewModels.Site.Shop.Landing;

public class ShopLandingDTO
{
    #region properties

    public List<LastestShopProducts> LastestShopProducts { get; set; }

    public List<LastestSellers> LastestSellers { get; set; }

    public List<LastestBrands> LastestBrands { get; set; }

    #endregion
}

public class LastestSellers
{
    #region properties

    public ulong UserSellerId { get; set; }

    public string? Username { get; set; }

    public string? UserAvatar { get; set; }

    public DateTime CreateDate { get; set; }

    public int ProductCount { get; set; }

    #endregion
}

public class LastestBrands
{
    #region properties

    public ulong BrandId { get; set; }

    public int ProductCounts { get; set; }

    public string BrandTitle { get; set; }

    public string Image { get; set; }

    #endregion
}