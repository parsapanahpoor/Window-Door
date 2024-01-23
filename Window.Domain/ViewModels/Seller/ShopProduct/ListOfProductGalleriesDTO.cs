﻿namespace Window.Domain.ViewModels.Seller.ShopProduct;

public record ProductGalleriesDTO 
{
    #region properties

    public ulong Id { get; set; }

    public ulong ProductGalleryId { get; set; }

    public string ImageName { get; set; }

    #endregion
}