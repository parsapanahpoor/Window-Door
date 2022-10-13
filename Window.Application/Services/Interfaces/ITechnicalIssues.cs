using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.TechnicalIssues;
using Window.Domain.ViewModels.Admin.TechnicalIssues;

namespace Window.Application.Services.Interfaces
{
    public interface ITechnicalIssues
    {
        #region Admin Side 

        //Filter Technical Issues Admin Side View Model
        Task<FilterTechnicalIssuesAdminSideViewModel> FilterTechnicalIssuesAdminSideViewModel(FilterTechnicalIssuesAdminSideViewModel filter);

        //Create Technical Issues
        Task CreateTechnicalIssues(TechnicalIssues technicalIssues);

        //Get Technical Issues By Id
        Task<TechnicalIssues?> GetTechnicalIssuesById(ulong id);

        //Update Technical Issues 
        Task UpdateTechnicalIssues(TechnicalIssues technical);

        //Delete Technical Issues 
        Task<bool> DeleteTechnicalIssues(ulong id);

        #endregion
    }
}
