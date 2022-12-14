using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Application.Extensions;
using Window.Application.Security;
using Window.Application.Services.Interfaces;
using Window.Application.StticTools;
using Window.Data.Context;
using Window.Domain.Entities.Brand;
using Window.Domain.Enums.BrandType;
using Window.Domain.ViewModels.Admin.Brand;
using Window.Domain.ViewModels.Admin.Ticket;
using Window.Domain.ViewModels.Common;
using Window.Domain.ViewModels.Site.API;

namespace Window.Application.Services.Services
{
    public class BrandService : IBrandService
    {
        #region Ctor

        private readonly WindowDbContext _context;

        public BrandService(WindowDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Admin Side 

        public async Task<FilterMainBrandViewModel> FilterMainBrandViewModel(FilterMainBrandViewModel filter)
        {
            var query = _context.MainBrands
                .Where(a => !a.IsDelete)
                .OrderBy(s => s.Priority)
                .AsQueryable();

            #region Filter By Properties

            switch (filter.BrandType)
            {
                case Domain.Enums.BrandType.BrandType.All:
                    break;
                case BrandType.Alminum:
                    query = query.Where(s => s.Alominum);
                    break;
                case BrandType.UPVC:
                    query = query.Where(s => s.UPVC);
                    break;
                case BrandType.Yaragh:
                    query = query.Where(s => s.Yaragh);
                    break;
            }

            if (!string.IsNullOrEmpty(filter.BrandName))
            {
                query = query.Where(p => p.BrandName.Contains(filter.BrandName));
            }

            #endregion

            await filter.Paging(query);

            return filter;
        }

        public async Task<FilterYaraghBrandViewModel> FilterYaraghBrandViewModel(FilterYaraghBrandViewModel filter)
        {
            var query = _context.YaraghBrands
                .Where(a => !a.IsDelete)
                .OrderByDescending(s => s.CreateDate)
                .AsQueryable();

            #region Filter By Properties

            if (!string.IsNullOrEmpty(filter.BrandName))
            {
                query = query.Where(p => p.YaraghBrandName.Contains(filter.BrandName));
            }

            #endregion

            await filter.Paging(query);

            return filter;
        }

        public async Task<bool> CreateMainBrand(MainBrand brand , IFormFile? brandLogo)
        {
            #region Add Image 

            if (brandLogo != null && brandLogo.IsImage())
            {
                var imageName = Guid.NewGuid() + Path.GetExtension(brandLogo.FileName);
                brandLogo.AddImageToServer(imageName, FilePaths.BrandImageOriginPathServer, 400, 300, FilePaths.BrandImageThumbPathServer);
                brand.BrandLogo = imageName;
            }

            #endregion

            #region Add Method 

            await _context.MainBrands.AddAsync(brand);
            await _context.SaveChangesAsync();  

            #endregion

            return true;
        }

        public async Task<bool> CreateYaraghBrand(YaraghBrand brand, IFormFile? brandLogo)
        {
            #region Add Image 

            if (brandLogo != null && brandLogo.IsImage())
            {
                var imageName = Guid.NewGuid() + Path.GetExtension(brandLogo.FileName);
                brandLogo.AddImageToServer(imageName, FilePaths.BrandImageOriginPathServer, 400, 300, FilePaths.BrandImageThumbPathServer);
                brand.YaraghBrandLogo = imageName;
            }

            #endregion

            #region Add Method 

            await _context.YaraghBrands.AddAsync(brand);
            await _context.SaveChangesAsync();

            #endregion

            return true;
        }


        public async Task<MainBrand?> GetMainBrandById(ulong brandId)
        {
            return await _context.MainBrands.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == brandId);   
        }

        public async Task<YaraghBrand?> GetYaraghBrandById(ulong brandId)
        {
            return await _context.YaraghBrands.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == brandId);
        }


        public async Task<bool> UpdateMainBrand(MainBrand mainBrand , IFormFile? brandLogo)
        {
            #region Get Brand By Id  

            var brand = await _context.MainBrands.FirstOrDefaultAsync(p=> !p.IsDelete && p.Id == mainBrand.Id);

            if (brand == null) return false;

            #endregion

            #region Update Models 

            brand.BrandName = mainBrand.BrandName;
            brand.UPVC = mainBrand.UPVC;
            brand.Alominum = mainBrand.Alominum;
            brand.Priority = mainBrand.Priority;
            brand.Yaragh = mainBrand.Yaragh;

            if (brandLogo != null && brandLogo.IsImage())
            {
                var imageName = Guid.NewGuid() + Path.GetExtension(brandLogo.FileName);
                brandLogo.AddImageToServer(imageName, FilePaths.BrandImageOriginPathServer, 400, 300, FilePaths.BrandImageThumbPathServer);

                if (!string.IsNullOrEmpty(brand.BrandLogo))
                {
                    brand.BrandLogo.DeleteImage(FilePaths.BrandImageOriginPathServer, FilePaths.BrandImageThumbPathServer);
                }

                brand.BrandLogo = imageName;
            }

            #endregion

            _context.MainBrands.Update(brand);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateYaraghBrand(YaraghBrand mainBrand, IFormFile? brandLogo)
        {
            #region Get Brand By Id  

            var brand = await _context.YaraghBrands.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == mainBrand.Id);

            if (brand == null) return false;

            #endregion

            #region Update Models 

            brand.YaraghBrandName = mainBrand.YaraghBrandName;
            brand.SellerType = mainBrand.SellerType;

            if (brandLogo != null && brandLogo.IsImage())
            {
                var imageName = Guid.NewGuid() + Path.GetExtension(brandLogo.FileName);
                brandLogo.AddImageToServer(imageName, FilePaths.BrandImageOriginPathServer, 400, 300, FilePaths.BrandImageThumbPathServer);

                if (!string.IsNullOrEmpty(brand.YaraghBrandLogo))
                {
                    brand.YaraghBrandLogo.DeleteImage(FilePaths.BrandImageOriginPathServer, FilePaths.BrandImageThumbPathServer);
                }

                brand.YaraghBrandLogo = imageName;
            }

            #endregion

            _context.YaraghBrands.Update(brand);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> DeleteMainBrand(ulong brandId)
        {
            #region Get Brand By Brand Id

            var mainBrand = await _context.MainBrands.FirstOrDefaultAsync(p=> !p.IsDelete && p.Id == brandId);
            if (mainBrand == null) return false;

            #endregion

            mainBrand.IsDelete = true;

            _context.MainBrands.Update(mainBrand);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteYaraghBrand(ulong brandId)
        {
            #region Get Brand By Brand Id

            var mainBrand = await _context.YaraghBrands.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == brandId);
            if (mainBrand == null) return false;

            #endregion

            mainBrand.IsDelete = true;

            _context.YaraghBrands.Update(mainBrand);
            await _context.SaveChangesAsync();

            return true;
        }


        #endregion

        #region Site Side

        //Get Brand By Name
        public async Task<MainBrand> GetMainBrandByBrandName(string name)
        {
            return await _context.MainBrands.FirstOrDefaultAsync(p => !p.IsDelete && p.BrandName == name);
        }

        public async Task<List<SelectListViewModel>> GetAllBrands()
        {
            return await _context.MainBrands.Where(s => !s.IsDelete)
                .Select(s => new SelectListViewModel
                {
                    Id = s.Id,
                    Title = s.BrandName
                }).ToListAsync();
        }

        //Get List Of Main Brands Of API
        public async Task<List<MainBrand>> GetListOfMainBrand()
        {
            return await _context.MainBrands.Where(p=> !p.IsDelete).ToListAsync();
        }

        #endregion
    }
}
