
using Window.Application.Common.IUnitOfWork;
using Window.Application.Extensions;
using Window.Application.Services.Interfaces;
using Window.Application.StticTools;
using Window.Domain.Interfaces.Maket;
using Window.Domain.Interfaces.ShopProduct;
using Window.Domain.Interfaces.ShopProductFeature;

namespace Window.Application.CQRS.SellerPanel.ShopProducts.Commands.DeleteShopProductFeature;

public record DeleteShopProductFeatureCommandHandler : IRequestHandler<DeleteShopProductFeatureCommand, bool>
{
    #region Ctor

    private readonly IMarketQueryRepository _marketQueryRepository;
    private readonly IShopProductFeatureCommandRepository _shopShopProductFeatureCommandRepository;
    private readonly IShopProductFeatureQueryRepository _shopShopProductFeatureQueryRepository;
    private readonly IShopProductQueryRepository _shopProductQueryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteShopProductFeatureCommandHandler(IMarketQueryRepository marketQueryRepository,
                                              IShopProductFeatureCommandRepository shopShopProductFeatureCommandRepository,
                                              IShopProductFeatureQueryRepository shopShopProductFeatureQueryRepository,
                                              IShopProductQueryRepository shopProductQueryRepository , 
                                              IUnitOfWork unitOfWork)
    {
        _marketQueryRepository = marketQueryRepository;
        _shopShopProductFeatureCommandRepository = shopShopProductFeatureCommandRepository;
        _shopShopProductFeatureQueryRepository = shopShopProductFeatureQueryRepository;
        _shopProductQueryRepository = shopProductQueryRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<bool> Handle(DeleteShopProductFeatureCommand request, CancellationToken cancellationToken)
    {
        #region Get Market By UserId 

        var market = await _marketQueryRepository.GetMarketByUserId(request.SellerUserId, cancellationToken);
        if (market == null) return false;

        #endregion

        #region Get Product Feature

        var ShopProductFeature = await _shopShopProductFeatureQueryRepository.GetByIdAsync(cancellationToken, request.FeatureId);
        if (ShopProductFeature == null) return false;

        #endregion

        #region Get Product By Id 

        var originProduct = await _shopProductQueryRepository.GetByIdAsync(cancellationToken, ShopProductFeature.ProductId);
        if (originProduct == null) return false;
        if (originProduct.SellerUserId != request.SellerUserId) return false;

        #endregion

        #region Delete 

        ShopProductFeature.IsDelete = true;

        _shopShopProductFeatureCommandRepository.Update(ShopProductFeature);
        await _unitOfWork.SaveChangesAsync();

        #endregion

        return true;
    }
}
