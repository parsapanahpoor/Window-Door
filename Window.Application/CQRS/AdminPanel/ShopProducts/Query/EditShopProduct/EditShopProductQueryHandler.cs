using Microsoft.EntityFrameworkCore.ChangeTracking;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using Window.Domain.Interfaces.ShopProduct;
using Window.Domain.ViewModels.Admin.ShopProduct;
using Window.Domain.ViewModels.Seller.ShopProduct;
using Window.Domain.ViewModels.Site.Shop.ShopProduct;

namespace Window.Application.CQRS.AdminPanel.ShopProducts.Query.EditShopProduct;

public record EditShopProductQueryHandler : IRequestHandler<EditShopProductQuery, EditShopProductAdminSideDTO>
{
    #region Ctor

    private readonly IShopProductQueryRepository _shopProductQueryRepository;

    public EditShopProductQueryHandler(IShopProductQueryRepository shopProductQueryRepository)
    {
        _shopProductQueryRepository = shopProductQueryRepository;
    }

    #endregion

    public async Task<EditShopProductAdminSideDTO> Handle(EditShopProductQuery request, CancellationToken cancellationToken)
    {
        #region Get Product By Id 

        var product = await _shopProductQueryRepository.GetByIdAsync(cancellationToken, request.ProductId);
        if (product == null) return null;

        #endregion

        #region Fill DTO 

        EditShopProductAdminSideDTO model = new EditShopProductAdminSideDTO()
        {
            ShopProductId = product.Id,
            ShopColorId = product.ProductColorId,
            Title = product.ProductName,
            Description = product.LongDescription,
            Price = product.Price.ToString(),
            ShortDescription = product.ShortDescription,
            SaleScaleId = product.SaleScaleId,
            SaleRatio = product.SalesRatio,
            IsInIncridble = await _shopProductQueryRepository.IsExistCurrentProduct_InIncrediblesProducts_ByProductId(product.Id , cancellationToken),
            IsInCustomersSuggestions = await _shopProductQueryRepository.IsExistCurrentProduct_InCustomersSuggestions_ByProductId(product.Id , cancellationToken),
        };

        #endregion

        #region Product Tags

        var tags = await _shopProductQueryRepository.GetListOfProductTagsByProductId(product.Id, cancellationToken);
        model.ProductTag = string.Join(",", tags.Select(p => p.TagTitle).ToList());

        #endregion

        return model;
    }
}
