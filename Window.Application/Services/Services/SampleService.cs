#region Usings

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Window.Application.Extensions;
using Window.Application.Services.Interfaces;
using Window.Application.StticTools;
using Window.Data.Context;
using Window.Domain.Entities.Sample;
using Window.Domain.Enums.SellerType;
using Window.Domain.Enums.Types;
using Window.Domain.ViewModels.Admin.Sample;

namespace Window.Application.Services.Services;

#endregion

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
        if (!await IsValidSample(sample, segmentsId)) return false;

        return await SaveSampleWithImage(sampleImage, async imageName =>
        {
            sample.Image = imageName!;
            sample.SampleSelectedSegment = segmentsId.Distinct()
                .Select(segmentId => new SampleSelectedSegment { SegmentId = segmentId }).ToList();
            _context.Samples.Add(sample);
            await _context.SaveChangesAsync();
        });
    }

    private async Task<bool> IsValidSample(Sample sample, List<ulong> segmentsId)
    {
        if (string.IsNullOrWhiteSpace(sample.SegmentName) || sample.MinHeight < 0 || sample.MinWidth < 0
            || sample.MaxHeight <= 0 || sample.MaxWidth <= 0 || sample.MinHeight > sample.MaxHeight
            || sample.MinWidth > sample.MaxWidth || !(sample.UPVC || sample.Aluminum)
            || !(sample.Door || sample.Window) || !(sample.Keshoie || sample.Lolaie))
            return false;

        var ids = segmentsId.Distinct().ToArray();
        return await _context.Segments.CountAsync(segment => !segment.IsDelete && ids.Contains(segment.Id)) == ids.Length;
    }

    private static async Task<bool> SaveSampleWithImage(IFormFile? image, Func<string?, Task> persist)
    {
        string? imageName = null;
        var saved = false;
        try
        {
            if (image != null)
            {
                var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
                if (extension is not (".jpg" or ".jpeg" or ".png")) return false;

                imageName = Guid.NewGuid().ToString("N") + extension;
                image.AddImageToServer(imageName, FilePaths.SampleImageOriginPathServer, 400, 300, FilePaths.SampleImageThumbPathServer);
                if (!File.Exists(Path.Combine(FilePaths.SampleImageOriginPathServer, imageName))
                    || !File.Exists(Path.Combine(FilePaths.SampleImageThumbPathServer, imageName)))
                    return false;
            }

            await persist(imageName);
            saved = true;
            return true;
        }
        finally
        {
            if (!saved && imageName != null)
                imageName.DeleteImage(FilePaths.SampleImageOriginPathServer, FilePaths.SampleImageThumbPathServer);
        }
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

    //Get Sample By Id With As No Tracking
    public async Task<Sample?> GetSampleBySampleIdWithAsNoTracking(ulong sampleId)
    {
        #region Get Sample 

        var sample = await _context.Samples
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(p => p.Id == sampleId && !p.IsDelete);

        if (sample == null) return null;

        #endregion

        return sample;
    }

    public async Task<bool> EditSample(Sample sample, List<ulong> segmentsId, IFormFile? ArticleImage)
    {
        if (!await IsValidSample(sample, segmentsId)) return false;

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

        return await SaveSampleWithImage(ArticleImage, async imageName =>
        {
            if (imageName != null) model.Image = imageName;

            var previous = await _context.SampleSelectedSegments
                .Where(part => !part.IsDelete && part.SampleId == model.Id).ToListAsync();
            var desired = segmentsId.Distinct().ToHashSet();

            _context.SampleSelectedSegments.RemoveRange(previous.Where(part => !desired.Contains(part.SegmentId)));
            _context.SampleSelectedSegments.AddRange(desired.Except(previous.Select(part => part.SegmentId))
                .Select(id => new SampleSelectedSegment { SampleId = model.Id, SegmentId = id }));
            await _context.SaveChangesAsync();
        });
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

        var log = await _context.LogInquiryForUsers
                                .AsNoTracking()
                                .FirstOrDefaultAsync(p => !p.IsDelete && p.UserMAcAddress == userMacAddress);

        if (log == null) return null;

        #endregion

        #region Get Samples

        var samples =  _context.Samples
                               .AsNoTracking()
                               .Where(p => !p.IsDelete)
                               .OrderByDescending(p=> p.Priority)
                               .AsQueryable();

        if (log.SellerType == SellerType.Aluminium)
        {
            samples = samples.Where(s => !s.IsDelete && s.Aluminum);
        }

        if (log.SellerType == SellerType.UPC)
        {
            samples = samples.Where(s => !s.IsDelete && s.UPVC);
        }

        if (log.SellerType == SellerType.UPVCAlminium)
        {
            samples = samples.Where(s => !s.IsDelete);
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
