
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System.Runtime.CompilerServices;
using Window.Application.Common.IUnitOfWork;
using Window.Application.Extensions;
using Window.Application.Security;
using Window.Application.Services.Interfaces;
using Window.Application.StticTools;
using Window.Domain.Entities.ShopProductGallery;
using Window.Domain.Interfaces.Maket;
using Window.Domain.Interfaces.ShopProduct;
using Window.Domain.Interfaces.ShopProductGallery;

namespace Window.Application.CQRS.SellerPanel.ShopProducts.Commands.CreateShopProductGallery;

public record CreateProductGalleryQueryHandler : IRequestHandler<CreateProductGalleryQuery, bool>
{
    #region Ctor

    private readonly IShopProductGalleryCommandRepository _commandRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMarketQueryRepository _maketsQueryRepository;
    private readonly IShopProductQueryRepository _shopProductQueryRepository;

    public CreateProductGalleryQueryHandler(IShopProductGalleryCommandRepository commandRepository , 
                                            IUnitOfWork unitOfWork ,
                                            IMarketQueryRepository maketsQueryRepository ,
                                            IShopProductQueryRepository shopProductQueryRepository)
    {
        _commandRepository = commandRepository;
        _unitOfWork = unitOfWork;
        _maketsQueryRepository = maketsQueryRepository;
        _shopProductQueryRepository = shopProductQueryRepository;
    }

    #endregion

    public async Task<bool> Handle(CreateProductGalleryQuery request, CancellationToken cancellationToken)
    {
        #region Get Market By UserId 

        var market = await _maketsQueryRepository.GetMarketByUserId(request.sellerUserId , cancellationToken);
        if (market == null) return false;

        #endregion

        #region Get Product By Id 

        var originProduct = await _shopProductQueryRepository.GetByIdAsync(cancellationToken, request.productId);
        if (originProduct == null) return false;
        if (originProduct.SellerUserId != request.sellerUserId) return false;

        #endregion

        #region Add Gallery

        ShopProductGallery productGallery = new ShopProductGallery()
        {
            ProductId = request.productId,
        };

        if (request.image != null && request.image.IsImage())
        {
            var imageName = Guid.NewGuid() + Path.GetExtension(request.image.FileName);
            request.image.AddImageToServer(imageName, FilePaths.ProductsGalleryPathServer, 400, 300, FilePaths.ProductsGalleryPathThumbServer);
            productGallery.ImageName = imageName;
        }

        await _commandRepository.AddAsync(productGallery , cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        #endregion

        return true;
    }
}
