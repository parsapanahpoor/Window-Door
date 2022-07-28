using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Brand;
using Window.Domain.Entities.Product;
using Window.Domain.ViewModels.Common;
using Window.Domain.ViewModels.Seller.Pricing;
using Window.Domain.ViewModels.Seller.Product;

namespace Window.Application.Services.Interfaces
{
    public interface IProductService
    {
        #region Seller Side

        Task<List<SelectListViewModel>> GetProductTypeCategories(ulong productTypeId);

        Task<ulong> CreateProduct(CreateProductSellerSideViewModel model, ulong userId);

        Task<Product?> GetProductById(ulong productId);

        Task<List<MainBrand>> GetListOfMainBrandBaseOnSelectedProduct(Product product);

        List<SelectListItem> GetSegmentsForAddProduct(ulong productId);

        Task<List<ProductMainBrandPrice>?> ListOfProductMainBrandPriceByProductId(ulong productId, ulong brandId);

        Task<List<ProductYaraghBrandPrice>?> ListOfProductYaraghBrandPriceByProductId(ulong productId, ulong brandId);

        Task<bool> AddPriceForSelectedMainBrand(AddPriceForSelectedMainBrandViewModel model, ulong userId);

        Task<List<YaraghBrand>> GetListOfYaraghBrandBaseOnSelectedProduct(Product product);

        Task<bool> AddPriceForSelectedYaraghBrand(AddPriceForSelectedYaraghBrandViewModel model, ulong userId);

        Task<FilterProductSellerSideViewModel> FilterProductSellerSideViewModel(FilterProductSellerSideViewModel filter);

        Task<List<SelectListViewModel>> LoadBrands(ulong sellerTypeId);

        Task<bool> GetSellerTypeForValidAddProduct(ulong userId);

        Task<SegmentPricingViewModel?> FillSegmentPricingViewModel(ulong productId, ulong userId);

        Task<bool> AddPricingForSegment(ulong ProductId, ulong SegmentId, int Price, ulong userId);

        Task<List<SegmentPricing>?> FillSegmentPricing(ulong productId, ulong userId);

        #endregion
    }
}
