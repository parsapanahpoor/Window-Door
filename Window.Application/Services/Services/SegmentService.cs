using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Application.Services.Interfaces;
using Window.Data.Context;
using Window.Domain.Entities.Glass;
using Window.Domain.Entities.Segment;
using Window.Domain.ViewModels.Admin.Segment;

namespace Window.Application.Services.Services
{
    public class SegmentService : ISegmentService
    {
        #region Ctor

        private readonly WindowDbContext _context;

        public SegmentService(WindowDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Admin Side 

        public async Task<FilterGlassesViewModel> FilterGlasseViewModel(FilterGlassesViewModel filter)
        {
            var query = _context.Glasses
                .Where(a => !a.IsDelete)
                .OrderByDescending(s => s.CreateDate)
                .AsQueryable();

            #region Filter By Properties

            if (!string.IsNullOrEmpty(filter.GlassName))
            {
                query = query.Where(p => p.GlassName.Contains(filter.GlassName));
            }

            #endregion

            await filter.Paging(query);

            return filter;
        }


        public async Task<FilterSegmentViewModel> FilterSegmentViewModel(FilterSegmentViewModel filter)
        {
            var query = _context.Segments
                .Where(a => !a.IsDelete)
                .OrderByDescending(s => s.CreateDate)
                .AsQueryable();

            #region Filter By Properties

            if (!string.IsNullOrEmpty(filter.SegmentName))
            {
                query = query.Where(p => p.SegmentName.Contains(filter.SegmentName));
            }

            #endregion

            await filter.Paging(query);

            return filter;
        }

        public async Task<bool> CreateSegment(Segment segment)
        {
            #region Add Method 

            await _context.Segments.AddAsync(segment);
            await _context.SaveChangesAsync();

            #endregion

            return true;
        }

        public async Task<bool> CreateGlass(Glass glass)
        {
            #region Add Method 

            await _context.Glasses.AddAsync(glass);
            await _context.SaveChangesAsync();

            #endregion

            return true;
        }

        public async Task<Segment?> GetSegmentById(ulong segmentId)
        {
            return await _context.Segments.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == segmentId);
        }

        public async Task<Glass?> GetGlassById(ulong glassId)
        {
            return await _context.Glasses.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == glassId);
        }

        public async Task<bool> UpdateSegment(Segment segment)
        {
            #region Get Segment By Id  

            var lastSegment = await _context.Segments.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == segment.Id);

            if (lastSegment == null) return false;

            #endregion

            #region Update Models 

            lastSegment.SegmentName = segment.SegmentName;
            lastSegment.Keshoie = segment.Keshoie;
            lastSegment.Lolaie = segment.Lolaie;
            lastSegment.Window = segment.Window;
            lastSegment.Door = segment.Door;
            lastSegment.UPVC = segment.UPVC;
            lastSegment.Aluminum = segment.Aluminum;

            #endregion

            _context.Segments.Update(lastSegment);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateGlass(Glass glass)
        {
            #region Get Glass By Id  

            var LastGlass = await _context.Glasses.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == glass.Id);

            if (LastGlass == null) return false;

            #endregion

            #region Update Models 

            LastGlass.GlassName = glass.GlassName;

            #endregion

            _context.Glasses.Update(LastGlass);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteSegment(ulong segmentId)
        {
            #region Get Segment By Brand Id

            var segment = await _context.Segments.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == segmentId);
            if (segment == null) return false;

            #endregion

            segment.IsDelete = true;

            _context.Segments.Update(segment);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteGlass(ulong glassId)
        {
            #region Get Segment By Brand Id

            var glass = await _context.Glasses.FirstOrDefaultAsync(p => !p.IsDelete && p.Id == glassId);
            if (glass == null) return false;

            #endregion

            glass.IsDelete = true;

            _context.Glasses.Update(glass);
            await _context.SaveChangesAsync();

            return true;
        }

        #endregion
    }
}
