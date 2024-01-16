
using Window.Application.Common.IUnitOfWork;
using Window.Application.Extensions;
using Window.Application.Services.Interfaces;
using Window.Application.StticTools;
using Window.Domain.Interfaces.Maket;
using Window.Domain.Interfaces.ShopProduct;
using Window.Domain.Interfaces.ShopProductGallery;

namespace Window.Application.CQRS.SellerPanel.ShopProducts.Commands.DeleteProductGallery;

public record DeleteProductGalleryCommandHandler : IRequestHandler<DeleteProductGalleryCommand, bool>
{
    #region Ctor

    private readonly IMarketQueryRepository _marketQueryRepository;
    private readonly IShopProductGalleryCommandRepository _shopProductGalleryCommandRepository;
    private readonly IShopProductGalleryQueryRepository _shopProductGalleryQueryRepository;
    private readonly IShopProductQueryRepository _shopProductQueryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductGalleryCommandHandler(IMarketQueryRepository marketQueryRepository,
                                              IShopProductGalleryCommandRepository shopProductGalleryCommandRepository,
                                              IShopProductGalleryQueryRepository shopProductGalleryQueryRepository,
                                              IShopProductQueryRepository shopProductQueryRepository , 
                                              IUnitOfWork unitOfWork)
    {
        _marketQueryRepository = marketQueryRepository;
        _shopProductGalleryCommandRepository = shopProductGalleryCommandRepository;
        _shopProductGalleryQueryRepository = shopProductGalleryQueryRepository;
        _shopProductQueryRepository = shopProductQueryRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<bool> Handle(DeleteProductGalleryCommand request, CancellationToken cancellationToken)
    {
        #region Get Market By UserId 

        var market = await _marketQueryRepository.GetMarketByUserId(request.userSellerId, cancellationToken);
        if (market == null) return false;

        #endregion

        #region Get Product Gallery

        var productGallery = await _shopProductGalleryQueryRepository.GetByIdAsync(cancellationToken, request.galleryId);
        if (productGallery == null) return false;

        #endregion

        #region Get Product By Id 

        var originProduct = await _shopProductQueryRepository.GetByIdAsync(cancellationToken, productGallery.ProductId);
        if (originProduct == null) return false;
        if (originProduct.SellerUserId != request.userSellerId) return false;

        #endregion

        #region Delete 

        productGallery.IsDelete = true;

        if (!string.IsNullOrEmpty(productGallery.ImageName))
        {
            productGallery.ImageName.DeleteImage(FilePaths.ProductsGalleryPathServer, FilePaths.ProductsGalleryPathThumbServer);
        }

        _shopProductGalleryCommandRepository.Update(productGallery);
        await _unitOfWork.SaveChangesAsync();

        #endregion

        return true;
    }
}
