#region Usings

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Window.Domain.Entities.Brand;
using Window.Domain.Enums.SellerType;
using Window.Domain.ViewModels.Admin.Brand;
using Window.Domain.ViewModels.Common;
using Window.Domain.ViewModels.Seller.ShopProduct;
using Window.Domain.ViewModels.Site.Inquiry;
using Window.Domain.ViewModels.Site.Shop.Landing;
using Window.Domain.ViewModels.Site.Shop.ShopProduct;

namespace Window.Application.Services.Interfaces;

#endregion

public interface IBrandService
{
    #region Seller Side

    Task<ulong?> GetShopProductSelectedBrandByProductId(ulong productId, CancellationToken cancellation);

    Task<List<ShopProductsBrandDTO>?> FillShopProductsBrandDTO(CancellationToken cancellation);

    #endregion

    #region Genral Methods

    Task<List<BrandCategory>?> GetListOfBrandCategories();

    #endregion

    #region Site Side 

    //Fill ShopBrandsDTO For SiteSide Bar
    Task<List<ShopBrandsDTO>?> FillShopBrandsDTOForSiteSideBar(CancellationToken cancellationToken);

    //Fill List Of Brands For Filter Products 
    Task<List<ListOfBrandsForFilterProductsDTO>> FillListOfBrandsForFilterProductsDTO(CancellationToken cancellationToken);

    #endregion

    //Get Brand By Name
    Task<MainBrand> GetMainBrandByBrandName(string name);

    Task<List<SelectListViewModel>> GetUPVCBrands();

    Task<List<SelectListViewModel>> GetBrandsFromBrandType(int brandTypeId);

    //Get List Of Main Brands Of API
    Task<List<MainBrand>> GetListOfMainBrand();

    //Show List OF Brands By Brand Type 
    Task<List<ListOfBrandsWithCountOfSellersViewModel>?> ShowListOFBrandsByBrandType(SellerType sellerType);

    Task<FilterMainBrandViewModel> FilterMainBrandViewModel(FilterMainBrandViewModel filter);

    Task<bool> CreateMainBrand(MainBrand brand, IFormFile? brandLogo);

    Task<MainBrand?> GetMainBrandById(ulong brandId);

    Task<bool> UpdateMainBrand(MainBrand mainBrand, IFormFile? brandLogo);

    Task<bool> DeleteMainBrand(ulong brandId);

    Task<FilterYaraghBrandViewModel> FilterYaraghBrandViewModel(FilterYaraghBrandViewModel filter);

    Task<bool> CreateYaraghBrand(YaraghBrand brand, IFormFile? brandLogo);

    Task<YaraghBrand?> GetYaraghBrandById(ulong brandId);

    Task<bool> UpdateYaraghBrand(YaraghBrand mainBrand, IFormFile? brandLogo);

    Task<bool> DeleteYaraghBrand(ulong brandId);

    Task<List<SelectListViewModel>> GetAllBrands();

    //Get All Brands With As No Tracking
    Task<List<SelectListViewModel>> GetAllBrandsWithAsNoTracking();
}
