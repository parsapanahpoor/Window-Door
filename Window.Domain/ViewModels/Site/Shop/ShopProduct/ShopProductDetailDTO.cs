using Window.Domain.Entities.ShopProductFeature;

namespace Window.Domain.ViewModels.Site.Shop.ShopProduct;

public record ShopProductDetailDTO
{
    #region properties

    public ulong ProductId { get; set; }

    public string ProductImage { get; set; }

    public string ProductName { get; set; }

    public string ShortDescription { get; set; }

    public string LongDescription { get; set; }

    public decimal Price { get; set; }

    public string? SaleScale { get; set; }

    public Seller Seller { get; set; }

    public ShopProductDetailColor Color { get; set; }

    public ShopProductDetailBrand Brand { get; set; }

    public List<string>? ProductPirctures { get; set; }

    public List<ShopProductFeature>? ShopProductFeatures { get; set; }

    #endregion
}

public record ShopProductDetailColor
{
    #region properties

    public ulong ColorId { get; set; }

    public string ColorTitle { get; set; }

    #endregion
}


public record ShopProductDetailBrand
{
    #region properties

    public ulong BrandId { get; set; }

    public string BrandTitle { get; set; }

    #endregion
}

public record Seller
{
    public ulong SellerUserId { get; set; }

    public string Username { get; set; }
}