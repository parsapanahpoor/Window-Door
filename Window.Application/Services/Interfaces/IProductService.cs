using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Account;
using Window.Domain.Entities.Brand;
using Window.Domain.Entities.Glass;
using Window.Domain.Entities.Product;
using Window.Domain.Entities.Sample;
using Window.Domain.Enums.SellerType;
using Window.Domain.ViewModels.Common;
using Window.Domain.ViewModels.Seller.Pricing;
using Window.Domain.ViewModels.Seller.Product;
using Window.Domain.ViewModels.Site.Inquiry;

namespace Window.Application.Services.Interfaces
{
    public interface IProductService
    {
        #region Seller Side

        //Get List OF Brnads
        Task<List<MainBrand>> GetListOfBrands();

        //Load Brands For Create Product From Seller Side 
        Task<List<SelectListViewModel>>? LoadBrandsForCreateProductFromSellerSide(ulong userId);

        //Step 2
        Task<bool> GetSellerTypeForValidAddProductStep2(ulong userId , ulong brandId);

        Task<Glass?> GetGlassWithName(string glassName);

        Task<List<Glass>> GetListOfGlasses();

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

        Task<List<SelectListViewModel>> LoadBrands();

        Task<bool> GetSellerTypeForValidAddProduct(ulong userId);

        Task<SegmentPricingViewModel?> FillSegmentPricingViewModel(ulong productId, ulong userId);

        Task<bool> AddPricingForSegment(ulong ProductId, ulong SegmentId, int Price, ulong userId);

        Task<List<SegmentPricing>?> FillSegmentPricing(ulong productId, ulong userId);

        Task<GlassPricingViewModel?> FillGlassPricingEntityViewModel(ulong userId);

        Task<List<GlassPricing>?> FillGlassPricing(ulong userId);

        Task<bool> AddPricingForGlass(ulong GlassId, int Price, ulong userId);

        //Delete Product 
        Task<bool> DeleteProductById(ulong productId, ulong sellerId);

        #endregion

        #region Site Side

        //Get All Glasses
        Task<List<SelectListViewModel>> GetAllGlasses();

        Task<FilterInquiryViewModel> FilterInquiryViewModel(FilterInquiryViewModel filter);

        Task<List<InquiryViewModel>?> ListOfInquiry(List<SampleSizeViewModel> sampleSize, SellersFieldFitreViewModel sellerInfo);

        Task<List<Sample?>> GetSamplesById(List<ulong?> samplesId);

        Task<int?> InitializeSamplesPrice(List<Sample?> samples, User user, int height, int width);

        Task<int?> InitialTotalSamplePrice(ulong brandId, ulong sampleId, int height, int width, ulong userId);

        #endregion
    }
}
