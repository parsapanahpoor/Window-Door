using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Sample;
using Window.Domain.Enums.SellerType;
using Window.Domain.Enums.Types;
using Window.Domain.ViewModels.Admin.Sample;
using Window.Domain.ViewModels.Common;

namespace Window.Application.Services.Interfaces
{
    public interface ISampleService
    {
        #region Admin Side 

        Task<bool> CreateSampleFromAdmin(Sample sample, List<ulong> segmentsId, IFormFile sampleImage);

        Task<FilterSampleAdminSideViewModel> FilterSampleAdminSideViewModel(FilterSampleAdminSideViewModel filter);

        Task<List<ulong>> GetSampleSelectedSegmnetIdBySampleId(ulong sampleId);

        Task<Sample?> GetSampleBySampleId(ulong sampleId);

        Task<bool> EditSample(Sample sample, List<ulong> segmentsId, IFormFile? ArticleImage);

        Task<bool> DeleteSample(ulong sampleId);

        #endregion

        #region Site Side

        Task<List<Sample>?> GetAllSample();

        Task<List<Sample>?> GetAllDoorAndWindowSample(ProductKind sellerType);

        Task<List<Sample>?> GetAlllolaieKeshoieSample(ProductType productType);

        Task<List<Sample>?> GetAllSampleUsingProductTypeAndProductKind(ProductKind productKind, ProductType productType);

        #endregion
    }
}
