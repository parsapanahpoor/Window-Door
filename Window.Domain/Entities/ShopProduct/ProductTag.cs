using System.ComponentModel.DataAnnotations;
using Window.Domain.Entities.Common;
namespace Window.Domain.Entities.ShopProduct;

public sealed class ProductTag : BaseEntity
{
    #region properties

    public ulong ProductId { get; set; }

    public string TagTitle { get; set; }

    #endregion
}
