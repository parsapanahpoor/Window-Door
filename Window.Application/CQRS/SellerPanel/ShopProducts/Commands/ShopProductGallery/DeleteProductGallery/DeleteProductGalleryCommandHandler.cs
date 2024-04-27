using Window.Application.Common.IUnitOfWork;
using Window.Application.Extensions;
using Window.Application.Services.Interfaces;
using Window.Application.StticTools;
using Window.Domain.Interfaces.Maket;
using Window.Domain.Interfaces.ShopProduct;
using Window.Domain.Interfaces.ShopProductGallery;

namespace Window.Application.CQRS.SellerPanel.ShopProducts.Commands.DeleteProductGallery;

public record DeleteProductGalleryCommandHandler : IRequestHandler<DeleteProductGalleryCommand, DeleteProductGalleryCommand_Result>
{
    #region Ctor

    private readonly IMarketQueryRepository _marketQueryRepository;
    private readonly IShopProductGalleryCommandRepository _shopProductGalleryCommandRepository;
    private readonly IShopProductGalleryQueryRepository _shopProductGalleryQueryRepository;
    private readonly IShopProductQueryRepository _shopProductQueryRepository;
    private readonly IShopProductCommandRepository _shopProductCommandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductGalleryCommandHandler(IMarketQueryRepository marketQueryRepository,
                                              IShopProductGalleryCommandRepository shopProductGalleryCommandRepository,
                                              IShopProductGalleryQueryRepository shopProductGalleryQueryRepository,
                                              IShopProductQueryRepository shopProductQueryRepository,
                                              IShopProductCommandRepository shopProductCommandRepository,
                                              IUnitOfWork unitOfWork)
    {
        _marketQueryRepository = marketQueryRepository;
        _shopProductGalleryCommandRepository = shopProductGalleryCommandRepository;
        _shopProductGalleryQueryRepository = shopProductGalleryQueryRepository;
        _shopProductCommandRepository = shopProductCommandRepository;
        _shopProductQueryRepository = shopProductQueryRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<DeleteProductGalleryCommand_Result> Handle(DeleteProductGalleryCommand request, CancellationToken cancellationToken)
    {
        #region Get Market By UserId 

        var market = await _marketQueryRepository.GetMarketByUserId(request.userSellerId, cancellationToken);
        if (market == null) return new DeleteProductGalleryCommand_Result()
        {
            ProductId = 0,
            Result = false
        };

        #endregion

        #region Get Product Gallery

        var productGallery = await _shopProductGalleryQueryRepository.GetByIdAsync(cancellationToken, request.galleryId);
        if (productGallery == null) return new DeleteProductGalleryCommand_Result()
        {
            ProductId = 0,
            Result = false
        };

        #endregion

        #region Get Product By Id 

        var originProduct = await _shopProductQueryRepository.GetByIdAsync(cancellationToken, productGallery.ProductId);
        if (originProduct == null) return new DeleteProductGalleryCommand_Result()
        {
            ProductId = 0,
            Result = false
        };
        if (originProduct.SellerUserId != request.userSellerId) return new DeleteProductGalleryCommand_Result()
        {
            ProductId = 0,
            Result = false
        };

        #endregion

        #region Delete 

        productGallery.IsDelete = true;

        if (!string.IsNullOrEmpty(productGallery.ImageName))
        {
            productGallery.ImageName.DeleteImage(FilePaths.ProductsPathServer, FilePaths.ProductsPathThumbServer);

            #region Delete Main Image

            originProduct.ProductImage = "default.png";

            _shopProductCommandRepository.Update(originProduct);

            #endregion
        }

        _shopProductGalleryCommandRepository.Update(productGallery);
        await _unitOfWork.SaveChangesAsync();

        #endregion

        return new DeleteProductGalleryCommand_Result()
        {
            ProductId = originProduct.Id,
            Result = true
        };
    }
}
