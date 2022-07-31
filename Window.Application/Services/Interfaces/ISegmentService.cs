using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Glass;
using Window.Domain.Entities.Segment;
using Window.Domain.ViewModels.Admin.Segment;
using Window.Domain.ViewModels.Common;

namespace Window.Application.Services.Interfaces
{
    public interface ISegmentService
    {
        #region Admin Side 

        Task<FilterSegmentViewModel> FilterSegmentViewModel(FilterSegmentViewModel filter);

        Task<bool> CreateSegment(Segment segment);

        Task<Segment?> GetSegmentById(ulong segmentId);

        Task<bool> UpdateSegment(Segment segment);

        Task<bool> DeleteSegment(ulong segmentId);

        Task<FilterGlassesViewModel> FilterGlasseViewModel(FilterGlassesViewModel filter);

        Task<bool> CreateGlass(Glass glass);

        Task<Glass?> GetGlassById(ulong glassId);

        Task<bool> UpdateGlass(Glass glass);

        Task<bool> DeleteGlass(ulong glassId);

        Task<List<SelectListViewModel>> GetAllSegments();

        #endregion
    }
}
