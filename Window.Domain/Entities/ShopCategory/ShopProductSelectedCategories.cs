﻿using Microsoft.EntityFrameworkCore;
using Window.Domain.Entities.Common;

namespace Window.Domain.Entities.ShopCategories;

public sealed class ShopProductSelectedCategories : BaseEntity
{
    #region properties

    public ulong ShopProductId { get; set; }

    public ulong ShopCategoryId { get; set; }

    #endregion

    #region Navigation properties

    public ShopProduct.ShopProduct ShopProduct { get; set; }

    public ShopCategory ShopCategory { get; set; }

    #endregion
}
