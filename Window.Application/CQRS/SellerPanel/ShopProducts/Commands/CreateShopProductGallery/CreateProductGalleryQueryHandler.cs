
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using Window.Application.Common.IUnitOfWork;
using Window.Domain.Interfaces.ShopProductGallery;

namespace Window.Application.CQRS.SellerPanel.ShopProducts.Commands.CreateShopProductGallery;

public record CreateProductGalleryQueryHandler : IRequestHandler<CreateProductGalleryQuery, bool>
{
    #region Ctor

    private readonly IShopProductGalleryCommandRepository _commandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductGalleryQueryHandler(IShopProductGalleryCommandRepository commandRepository , 
                                            IUnitOfWork unitOfWork)
    {
        _commandRepository = commandRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<bool> Handle(CreateProductGalleryQuery request, CancellationToken cancellationToken)
    {
        #region Get Market By UserId 

        var market = await _marketService.GetMarketByUserId(sellerId);
        if (market == null) return null;

        #endregion

        #region Get Product By Id 

        var product = await _shopProductQueryRepository.GetByIdAsync(token, productId);
        if (product == null) return null;
        if (product.SellerUserId != sellerId) return null;

        #endregion

        #region Add Gallery



        #endregion

        return true;
    }
}
