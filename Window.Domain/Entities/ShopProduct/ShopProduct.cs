﻿using Window.Domain.Entities.Common;
namespace Window.Domain.Entities.ShopProduct;

public sealed class ShopProduct : BaseEntity
{
    #region properties

    public ulong SellerUserId { get; set; }

    public ulong ProductColorId { get; set; }

    public ulong ProductBrandId { get; set; }

    public string ProductImage { get; set; }

    public string ProductName { get; set; }

    #endregion
}