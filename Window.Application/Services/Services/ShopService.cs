using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Window.Application.Common.IUnitOfWork;
using Window.Application.Extensions;
using Window.Application.Security;
using Window.Application.Services.Interfaces;
using Window.Application.StticTools;
using Window.Domain.Entities.Article;
using Window.Domain.Entities.ShopProduct;
using Window.Domain.Interfaces.ShopProduct;
using Window.Domain.ViewModels.Seller.ShopProduct;

namespace Window.Application.Services.Services;

public class ShopProductService : IShopProductService
{
	#region Ctor

	private readonly IShopProductCommandRepository _shopProductCommandRepository;
	private readonly IShopProductQueryRepository _shopProductQueryRepository;
	private readonly IUnitOfWork _unitOfWork;
    private readonly ISellerService _marketService;

    public ShopProductService(IShopProductCommandRepository shopProductCommandRepository ,
                              IShopProductQueryRepository shopProductQueryRepository ,
                              IUnitOfWork unitOfWork ,
                              ISellerService marketService) 
    {
        _shopProductCommandRepository = shopProductCommandRepository;
        _shopProductQueryRepository = shopProductQueryRepository;
        _unitOfWork = unitOfWork;
        _marketService = marketService;
    }

    #endregion

    #region Seller Side 

    public async Task<FilterShopProductSellerSideDTO> FilterShopProductSellerSide(FilterShopProductSellerSideDTO filter, CancellationToken cancellation)
    {
        return await _shopProductQueryRepository.FilterShopProductSellerSide(filter , cancellation);
    }

    public async Task<CreateShopProductFromSellerPanelResult> AddShopProductToTheDataBase(ulong sellerId ,
                                                                                  CreateShopProductSellerSideDTO model, 
                                                                                  IFormFile newsImage, 
                                                                                  CancellationToken cancellation)
    {
        #region Get Market By UserId 

        var market = await _marketService.GetMarketByUserId(sellerId);
        if (market == null) return CreateShopProductFromSellerPanelResult.SellerIsNotFound;

        #endregion

        #region Add Shop Product Entity

        ShopProduct product = new ShopProduct()
        {
            CreateDate = DateTime.Now,
            IsDelete = false,
            ProductBrandId = model.ShopBrandId,
            ProductColorId = model.ShopColorId,
            ProductName = model.Title,
            SellerUserId = sellerId,
        };

        if (newsImage != null && newsImage.IsImage())
        {
            var imageName = Guid.NewGuid() + Path.GetExtension(newsImage.FileName);
            newsImage.AddImageToServer(imageName, FilePaths.ProductsPathServer, 400, 300, FilePaths.ProductsPathThumbServer);
            product.ProductImage = imageName;
        }

        #endregion

        #region Add Shop Product 

        await _shopProductCommandRepository.AddAsync(product, cancellation);
        await _unitOfWork.SaveChangesAsync();

        #endregion

        #region Shop Tags

        if (!string.IsNullOrEmpty(model.ProductTag))
        {
            List<string> tagsList = model.ProductTag.Split(',').ToList<string>();
            foreach (var itemTag in tagsList)
            {
                var newTag = new ProductTag
                {
                    ProductId = product.Id,
                    TagTitle = itemTag,
                    IsDelete = false,
                    CreateDate = DateTime.Now
                };
                await _shopProductCommandRepository.AddShopTagAsync(newTag, cancellation);
            }
        }

        #endregion

        #region Products Selected Brands



        #endregion

        #region Products Selected Colors



        #endregion

        await _unitOfWork.SaveChangesAsync();

    }

#endregion
}
