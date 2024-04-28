
using Window.Application.Common.IUnitOfWork;
using Window.Application.Services.Interfaces;
using Window.Domain.Interfaces.ShopProduct;

namespace Window.Application.CQRS.AdminPanel.ShopProducts.Command.ProductBrandsCommand;

public record ProductBrandsCommandHandler_AdminSide : IRequestHandler<ProductBrandsCommand_AdminSide, bool>
{
    #region Ctor

    private readonly IShopProductCommandRepository _shopProductCommandRepository;
    private readonly IShopProductQueryRepository _shopProductQueryRepository;
    private readonly IBrandService _brandService;
    private readonly IUnitOfWork _unitOfWork;

    public ProductBrandsCommandHandler_AdminSide(IShopProductCommandRepository shopProductCommandRepository,
                                       IShopProductQueryRepository shopProductQueryRepository,
                                       IBrandService brandService,
                                       IUnitOfWork unitOfWork)
    {
        _brandService = brandService;
        _shopProductCommandRepository = shopProductCommandRepository;
        _shopProductQueryRepository = shopProductQueryRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<bool> Handle(ProductBrandsCommand_AdminSide request, CancellationToken cancellationToken)
    {
        #region Get Product By Id

        var product = await _shopProductQueryRepository.GetByIdAsync(cancellationToken, request.ProductId);
        if (product == null ) return false;

        #endregion

        #region Edit Produc Brand

        product.ProductBrandId = request.BrandId;

        #endregion

        #region Update 

        _shopProductCommandRepository.Update(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return true;
    }
}
