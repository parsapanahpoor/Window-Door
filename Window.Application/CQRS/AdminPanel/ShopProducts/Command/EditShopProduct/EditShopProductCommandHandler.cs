using Window.Application.Common.IUnitOfWork;
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

        #endregion

        #region  Image

        if (request.ProductImage != null)
        {
            var imageName = Guid.NewGuid() + Path.GetExtension(request.ProductImage.FileName);
            request.ProductImage.AddImageToServer(imageName, FilePaths.ProductsPathServer, 400, 300, FilePaths.ProductsPathThumbServer);

            if (!string.IsNullOrEmpty(oldProduct.ProductImage))
            {
                oldProduct.ProductImage.DeleteImage(FilePaths.ProductsPathServer, FilePaths.ProductsPathThumbServer);
            }

            oldProduct.ProductImage = imageName;
        }

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

        await _unitOfWork.SaveChangesAsync();

        return EditShopProductFromAdminPanelResult.Success;
    }
}
