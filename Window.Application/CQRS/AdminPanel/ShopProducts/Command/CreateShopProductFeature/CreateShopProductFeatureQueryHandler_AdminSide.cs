
using Window.Application.Common.IUnitOfWork;
using Window.Application.CQRS.SellerPanel.ShopProducts.Commands.CreateShopShopProductFeature;
using Window.Application.Security;
using Window.Domain.Interfaces.Maket;
using Window.Domain.Interfaces.ShopProduct;
using Window.Domain.Interfaces.ShopProductFeature;

namespace Window.Application.CQRS.AdminPanel.ShopProducts.Command.CreateShopProductFeature;

public record CreateShopProductFeatureQueryHandler_AdminSide : IRequestHandler<CreateShopProductFeatureQuery_AdminSide, bool>
{
    #region Ctor

    private readonly IShopProductFeatureCommandRepository _commandRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMarketQueryRepository _maketsQueryRepository;
    private readonly IShopProductQueryRepository _shopProductQueryRepository;

    public CreateShopProductFeatureQueryHandler_AdminSide(IShopProductFeatureCommandRepository commandRepository,
                                            IUnitOfWork unitOfWork,
                                            IMarketQueryRepository maketsQueryRepository,
                                            IShopProductQueryRepository shopProductQueryRepository)
    {
        _commandRepository = commandRepository;
        _unitOfWork = unitOfWork;
        _maketsQueryRepository = maketsQueryRepository;
        _shopProductQueryRepository = shopProductQueryRepository;
    }

    #endregion

    public async Task<bool> Handle(CreateShopProductFeatureQuery_AdminSide request, CancellationToken cancellationToken)
    {
        #region Get Product By Id 

        var originProduct = await _shopProductQueryRepository.GetByIdAsync(cancellationToken, request.productId);
        if (originProduct == null) return false;

        #endregion

        #region Add ShopProductFeature

        var ShopProductFeature = new Domain.Entities.ShopProductFeature.ShopProductFeature()
        {
            ProductId = request.productId,
            FeatureTitle = request.FeatureTitle.SanitizeText(),
            FeatureValue = request.FeatureValue.SanitizeText(),
        };

        await _commandRepository.AddAsync(ShopProductFeature, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        #endregion

        return true;
    }
}
