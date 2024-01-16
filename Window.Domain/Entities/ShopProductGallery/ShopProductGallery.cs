using Window.Domain.Entities.Common;
namespace Window.Domain.Entities.ShopProductGallery;

public sealed class ShopProductGallery : BaseEntity
{
    #region properties

    public ulong ProductId { get; set; }

    public string? ImageName { get; set; }

    #endregion
}

