using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Brand;
using Window.Domain.ViewModels.Admin.Brand;

namespace Window.Application.Services.Interfaces
{
    public interface IBrandService
    {
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
    }
}
