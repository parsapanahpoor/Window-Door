namespace Window.Domain.ViewModels.Site.Shop.Order;

public record InvoiceDTO
{
    #region properties

    public ulong OrderId { get; set; }

    public List<OrderDetailForInvoice> OrderDetails { get; set; }

    public Entities.Location.Location? Location { get; set; }

    #endregion
}

public record OrderDetailForInvoice
{
    #region properties

    public ProductForInvoice? Product { get; set; }

    public int Count { get; set; }

    #endregion
}

public record ProductForInvoice
{
    public ulong ProductId { get; set; }

    public string ProductTitle { get; set; }

    public string ProductImage { get; set; }

    public decimal Price { get; set; }

    public string? SellerName { get; set; }

    public string? ColorName { get; set; }
}
