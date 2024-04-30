namespace Window.Domain.ViewModels.Site.Shop.Order;

public class ShopCartOrderDTO
{
    #region properties

    public List<ShopCartOrderDetailItems> ProductItems { get; set; }

    public int TotalPrice { get; set; }

    #endregion
}

public class ShopCartOrderDetailItems
{
    #region properties

    public Product? Products { get; set; }

    #endregion
}

public class ShopCartOrderProductSellerInfo
{
    #region properties

    public ulong SellerUserId { get; set; }

    public string SellerUsername { get; set; }

    #endregion
}

public class Product
{
    #region properties

    public ulong ProductId { get; set; }

    public int ProductEntity { get; set; }

    public string ProductTitle { get; set; }

    public string ProductImage { get; set; }

    public decimal ProductPrice { get; set; }

    public Color? Color { get; set; }

    public string? ProductSaleScale { get; set; }

    public ShopCartOrderProductSellerInfo? SellerInfo { get; set; }

    #endregion
}

public class Color
{
    #region properties

    public ulong ColorId { get; set; }

    public string ColorTitle { get; set; }

    public string ColorCode { get; set; }

    #endregion
}