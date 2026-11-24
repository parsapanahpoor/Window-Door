namespace Window.Domain.Entities.Inquiry;

public sealed class InquirySellerExposure
{
    public ulong InquiryId { get; set; }
    public ulong SellerId { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public int Tariff { get; set; }
}
