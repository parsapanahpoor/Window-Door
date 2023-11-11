namespace Window.Domain.ViewModels.Seller.PersonalInfo;

public record AddOrEditSellerPersonalVideoDTO
{
    #region properties

    public ulong? Id{ get; set; }

    public string AttachmentFileName { get; set; }

    public string? imagename { get; set; }

    public string Title { get; set; }

    #endregion
}
