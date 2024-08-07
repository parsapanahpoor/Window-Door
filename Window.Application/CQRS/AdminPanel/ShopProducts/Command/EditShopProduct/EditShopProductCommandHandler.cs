﻿using Window.Application.Common.IUnitOfWork;
using Window.Application.Extensions;
using Window.Application.Security;
using Window.Application.StticTools;
using Window.Domain.Entities.ShopProduct;
using Window.Domain.Interfaces.ShopColors;
using Window.Domain.Interfaces.ShopProduct;
using Window.Domain.ViewModels.Admin.ShopProduct;
using Window.Domain.ViewModels.Seller.ShopProduct;
using Window.Domain.ViewModels.Site.Shop.ShopProduct;

namespace Window.Application.CQRS.AdminPanel.ShopProducts.Command.EditShopProduct;

public record EditShopProductCommandHandler : IRequestHandler<EditShopProductCommand, EditShopProductFromAdminPanelResult>
{
    #region Ctor

    private readonly IShopProductCommandRepository _shopProductCommandRepository;
    private readonly IShopColorsQueryRepository _shopColorsQueryRepository;
    private readonly IShopProductQueryRepository _shopProductQueryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EditShopProductCommandHandler(IShopProductCommandRepository shopProductCommandRepository ,
                                         IShopProductQueryRepository shopProductQueryRepository ,
                                         IShopColorsQueryRepository shopColorsQueryRepository,
                                         IUnitOfWork unitOfWork)
    {
        _shopProductCommandRepository = shopProductCommandRepository;
        _shopProductQueryRepository = shopProductQueryRepository;
        _shopColorsQueryRepository = shopColorsQueryRepository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    public async Task<EditShopProductFromAdminPanelResult> Handle(EditShopProductCommand request, CancellationToken cancellationToken)
    {
        #region Get Product By Id 

        var oldProduct = await _shopProductQueryRepository.GetByIdAsync(cancellationToken, request.model.ShopProductId);
        if (oldProduct == null) return EditShopProductFromAdminPanelResult.Faild;

        #endregion

        #region Brand and Color Validator

        if (!await _shopColorsQueryRepository.IsExistColorById(request.model.ShopColorId, cancellationToken))
            return EditShopProductFromAdminPanelResult.Faild;

        #endregion

        #region Update Product Fields

        oldProduct.ProductName = request.model.Title.SanitizeText();
        oldProduct.ShortDescription = request.model.ShortDescription.SanitizeText();
        oldProduct.LongDescription = request.model.Description.SanitizeText();
        oldProduct.Price = decimal.Parse(request.model.Price);
        oldProduct.SaleScaleId = request.model.SaleScaleId;
        oldProduct.ProductColorId = request.model.ShopColorId;
        oldProduct.SalesRatio = request.model.SaleRatio;

        #endregion

        _shopProductCommandRepository.Update(oldProduct);

        #region Shop Tags

        var productTags = await _shopProductQueryRepository.GetListOfProductTagsByProductId(oldProduct.Id, cancellationToken);
        if (productTags != null && productTags.Any()) _shopProductCommandRepository.DeleteRange(productTags);

        if (!string.IsNullOrEmpty(request.model.ProductTag))
        {
            List<string> tagsList = request.model.ProductTag.Split(',').ToList<string>();
            foreach (var itemTag in tagsList)
            {
                var newTag = new ProductTag
                {
                    ProductId = oldProduct.Id,
                    TagTitle = itemTag,
                    IsDelete = false,
                    CreateDate = DateTime.Now
                };
                await _shopProductCommandRepository.AddShopTagAsync(newTag, cancellationToken);
            }
        }

        #endregion

        #region Incredible Products

        var incredibleProducts = await _shopProductQueryRepository.Get_IncredibleProducts_ByProductId(oldProduct.Id , cancellationToken);

        //Add to the incredible products
        if (incredibleProducts == null && request.model.IsInIncridble)
        {
            await _shopProductCommandRepository.Add_IncredileProducts(new IncredibleProducts()
            {
                ShopProductId = oldProduct.Id
            });
        }

        //Delete incredible products
        if (incredibleProducts != null && !request.model.IsInIncridble)
        {
            incredibleProducts.IsDelete = true;

            _shopProductCommandRepository.Update_IncredibleProducts(incredibleProducts);
        }

        #endregion

        #region Customers Suggestions

        var customersSuggestions = await _shopProductQueryRepository.Get_CustomerSuggestions_ByProductId(oldProduct.Id , cancellationToken);

        //Add to the customer suggestions products
        if (customersSuggestions == null && request.model.IsInCustomersSuggestions)
        {
            await _shopProductCommandRepository.Add_CustomerSuggestions(new CustomersSuggestions()
            {
                ShopProductId = oldProduct.Id
            });
        }

        //Delete customer suggestions
        if (customersSuggestions != null && !request.model.IsInCustomersSuggestions)
        {
            customersSuggestions.IsDelete = true;

            _shopProductCommandRepository.Update_CustomerSuggestion(customersSuggestions);
        }

        #endregion

        await _unitOfWork.SaveChangesAsync();

        return EditShopProductFromAdminPanelResult.Success;
    }
}
