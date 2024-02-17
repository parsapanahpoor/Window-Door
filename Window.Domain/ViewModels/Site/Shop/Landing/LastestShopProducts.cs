namespace Window.Domain.ViewModels.Site.Shop.Landing;

public record LastestShopProducts 
{
    #region properties

    public ulong ProductId { get; set; }

    public string ProductTitle { get; set; }

    public string ProductImage { get; set; }

    public decimal ProductPrice { get; set; }

    public SellerInfo? SellerInfo { get; set; }

    #endregion
}

public record SellerInfo
{
    #region properties

    public ulong SellerId { get; set; }

    public string SellerUsername { get; set; }

    #endregion
}