
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System.Runtime.CompilerServices;
using Window.Application.Common.IUnitOfWork;
using Window.Application.CQRS.SellerPanel.ShopProducts.Commands.CreateShopShopProductFeature;
using Window.Application.Extensions;
using Window.Application.Security;
using Window.Application.Services.Interfaces;
using Window.Application.StticTools;
using Window.Domain.Entities.ShopProductFeature;
using Window.Domain.Interfaces.Maket;
using Window.Domain.Interfaces.ShopProduct;
using Window.Domain.Interfaces.ShopProductFeature;

namespace Window.Application.CQRS.SellerPanel.ShopProducts.Commands.CreateShopProductFeature;

public record CreateShopProductFeatureQueryHandler : IRequestHandler<CreateShopProductFeatureQuery, bool>
{
    #region Ctor

    private readonly IShopProductFeatureCommandRepository _commandRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMarketQueryRepository _maketsQueryRepository;
    private readonly IShopProductQueryRepository _shopProductQueryRepository;

    public CreateShopProductFeatureQueryHandler(IShopProductFeatureCommandRepository commandRepository , 
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

    public async Task<bool> Handle(CreateShopProductFeatureQuery request, CancellationToken cancellationToken)
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

        #region Add ShopProductFeature

        ShopProductFeature ShopProductFeature = new ShopProductFeature()
        {
            ProductId = request.productId,
            FeatureTitle = request.FeatureTitle.SanitizeText(),
            FeatureValue = request.FeatureValue.SanitizeText(),
        };

        await _commandRepository.AddAsync(ShopProductFeature , cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        #endregion

        return true;
    }
}
