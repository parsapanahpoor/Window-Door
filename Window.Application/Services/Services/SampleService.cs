using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Application.Extensions;
using Window.Application.Services.Interfaces;
using Window.Application.StticTools;
using Window.Data.Context;
using Window.Domain.Entities.Sample;
using Window.Domain.Enums.SellerType;
using Window.Domain.Enums.Types;
using Window.Domain.ViewModels.Admin.Sample;
using Window.Domain.ViewModels.Common;

namespace Window.Application.Services.Services
{
    public class SampleService : ISampleService
    {
        #region ctor

        private readonly WindowDbContext _context;


        public SampleService(WindowDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Admin Side

        public async Task<bool> CreateSampleFromAdmin(Sample sample, List<ulong> segmentsId, IFormFile sampleImage)
        {
            #region Article Image

            var imageName = Guid.NewGuid() + Path.GetExtension(sampleImage.FileName);
            sampleImage.AddImageToServer(imageName, FilePaths.SampleImageOriginPathServer, 400, 300, FilePaths.SampleImageThumbPathServer);
            sample.Image = imageName;

            #endregion

            #region Add Sample

            await _context.Samples.AddAsync(sample);
            await _context.SaveChangesAsync();

            #endregion

            #region Add Sample Selected Segment

            foreach (var item in segmentsId)
            {
                SampleSelectedSegment segment = new SampleSelectedSegment()
                {
                    SampleId = sample.Id,
                    SegmentId = item,
                };

                await _context.SampleSelectedSegments.AddAsync(segment);
                await _context.SaveChangesAsync();
            }

            #endregion

            return true;
        }

        public async Task<FilterSampleAdminSideViewModel> FilterSampleAdminSideViewModel(FilterSampleAdminSideViewModel filter)
        {
            var query = _context.Samples
                .Where(a => !a.IsDelete)
                .OrderByDescending(s => s.Priority)
                .AsQueryable();

            #region Filter By Properties

            if (!string.IsNullOrEmpty(filter.SampleName))
                query = query.Where(s => EF.Functions.Like(s.SegmentName, $"%{filter.SampleName}%"));

            #endregion

            await filter.Paging(query);

            return filter;
        }

        public async Task<List<ulong>> GetSampleSelectedSegmnetIdBySampleId(ulong sampleId)
        {
            #region Get Sample Id

            var sample = await _context.Samples.FirstOrDefaultAsync(p => p.Id == sampleId && !p.IsDelete);

            #endregion

            #region Get Segments

            return await _context.SampleSelectedSegments.Where(p => !p.IsDelete && p.SampleId == sampleId).Select(p => p.SegmentId).ToListAsync();

            #endregion
        }

        public async Task<Sample?> GetSampleBySampleId(ulong sampleId)
        {
            #region Get Sample 

            var sample = await _context.Samples.FirstOrDefaultAsync(p => p.Id == sampleId && !p.IsDelete);
            if (sample == null) return null;

            #endregion

            return sample;
        }

        public async Task<bool> EditSample(Sample sample, List<ulong> segmentsId, IFormFile? ArticleImage)
        {
            #region Get Sample

            var model = await _context.Samples.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == sample.Id);
            if (model == null) return false;

            #endregion

            #region Update Field

            model.SegmentName = sample.SegmentName;
            model.UPVC = sample.UPVC;
            model.Aluminum = sample.Aluminum;
            model.Door = sample.Door;
            model.Window = sample.Window;
            model.Keshoie = sample.Keshoie;
            model.Lolaie = sample.Lolaie;
            model.MaxHeight = sample.MaxHeight;
            model.MaxWidth = sample.MaxWidth;
            model.MinWidth = sample.MinWidth;
            model.MinHeight = sample.MinHeight;
            model.Priority = sample.Priority;

            #endregion

            #region Sample Image

            if (ArticleImage != null)
            {
                var imageName = Guid.NewGuid() + Path.GetExtension(ArticleImage.FileName);
                ArticleImage.AddImageToServer(imageName, FilePaths.SampleImageOriginPathServer, 400, 300, FilePaths.SampleImageThumbPathServer);

                if (!string.IsNullOrEmpty(model.Image))
                {
                    model.Image.DeleteImage(FilePaths.SampleImageOriginPathServer, FilePaths.SampleImageThumbPathServer);
                }

                model.Image = imageName;
            }

            #endregion

            #region Update Sample

            _context.Samples.Update(model);
            await _context.SaveChangesAsync();

            #endregion

            #region Update Sample Selected Segment 

            var segments = await _context.SampleSelectedSegments.Where(p => !p.IsDelete && p.SampleId == sample.Id).ToListAsync();

            if (segments != null && segments.Any())
            {
                _context.SampleSelectedSegments.RemoveRange(segments);
                await _context.SaveChangesAsync();
            }

            if (segmentsId != null && segmentsId.Any())
            {
                foreach (var item in segmentsId)
                {
                    SampleSelectedSegment sampleSelected = new SampleSelectedSegment()
                    {
                        SampleId = sample.Id,
                        SegmentId = item
                    };

                    await _context.SampleSelectedSegments.AddAsync(sampleSelected);
                    await _context.SaveChangesAsync();
                }
            }

            #endregion

            return true;
        }

        public async Task<bool> DeleteSample(ulong sampleId)
        {
            #region Get Sample

            var model = await _context.Samples.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == sampleId);
            if (model == null) return false;

            #endregion

            #region Sample Image

            if (!string.IsNullOrEmpty(model.Image))
            {
                model.Image.DeleteImage(FilePaths.SampleImageOriginPathServer, FilePaths.SampleImageThumbPathServer);
            }

            #endregion

            #region Update Sample

            model.IsDelete = true;

            _context.Samples.Update(model);
            await _context.SaveChangesAsync();

            #endregion

            #region Update Sample Selected Segment 

            var segments = await _context.SampleSelectedSegments.Where(p => !p.IsDelete && p.SampleId == sampleId).ToListAsync();

            if (segments != null && segments.Any())
            {
                _context.SampleSelectedSegments.RemoveRange(segments);
                await _context.SaveChangesAsync();
            }

            #endregion

            return true;
        }

        #endregion

        #region Site Side 

        //Get Samples For Show In Inquiry Page (Or API)
        public async Task<List<Sample>?> GetListOfSamplesForShowInAPI(string userMacAddress)
        {
            #region Get User Log 

            var log = await _context.LogInquiryForUsers.FirstOrDefaultAsync(p => !p.IsDelete && p.UserMAcAddress == userMacAddress);
            if (log == null) return null;

            #endregion

            #region Get Samples

            var samples =  _context.Samples.Where(p => !p.IsDelete).AsQueryable();

            if (log.SellerType == SellerType.Aluminium)
            {
                samples = samples.Where(s => !s.IsDelete && s.Aluminum);
            }

            if (log.SellerType == SellerType.UPC)
            {
                samples = samples.Where(s => !s.IsDelete && s.UPVC);
            }

            #endregion

            return await samples.ToListAsync();
        }

        public async Task<List<Sample>?> GetAllSample()
        {
            return await _context.Samples.Where(s => !s.IsDelete).ToListAsync();
        }

        public async Task<List<Sample>?> GetAllDoorAndWindowSample(ProductKind productKind)
        {
            if (productKind == 0)
            {
                return await _context.Samples.Where(s => !s.IsDelete && s.Door).ToListAsync();
            }

            return await _context.Samples.Where(s => !s.IsDelete && s.Window).ToListAsync();
        }

        public async Task<List<Sample>?> GetAllSampleUsingProductTypeAndProductKind(ProductKind productKind, ProductType productType)
        {
            if (productKind == 0 && productType == 0)
            {
                return await _context.Samples.Where(s => !s.IsDelete && s.Door && s.Keshoie).ToListAsync();
            }

            if (((int)productKind) == 1 && productType == 0)
            {
                return await _context.Samples.Where(s => !s.IsDelete && s.Window && s.Keshoie).ToListAsync();
            }

            if (((int)productKind) == 0 && ((int)productType) == 1)
            {
                return await _context.Samples.Where(s => !s.IsDelete && s.Door && s.Lolaie).ToListAsync();
            }

            return await _context.Samples.Where(s => !s.IsDelete && s.Window && s.Lolaie).ToListAsync();
        }

        public async Task<List<Sample>?> GetAlllolaieKeshoieSample(ProductType productType)
        {
            if (productType == 0)
            {
                return await _context.Samples.Where(s => !s.IsDelete && s.Keshoie).ToListAsync();
            }

            return await _context.Samples.Where(s => !s.IsDelete && s.Lolaie).ToListAsync();
        }

        #endregion
    }
}
