
using Window.Domain.Entities.Counseling;
using Window.Domain.ViewModels.Admin.Cosultant;

namespace Window.Application.Services.Interfaces
{
    public interface IConsulantService
    {
        #region Admin Side 

        //Filter Consultant Admin Side View Model
        Task<FilterConsultantAdminSideViewModel> FilterConsultantAdminSideViewModel(FilterConsultantAdminSideViewModel filter);

        //Create Consultant
        Task CreateConsultant(Counseling counseling);

        //Get Consultant By Id
        Task<Counseling?> GetConsultantById(ulong id);

        //Update Consultant 
        Task UpdateConsultant(Counseling counseling);

        //Delete Consultant 
        Task<bool> DeleteConsultant(ulong id);

        #endregion

        #region Site Side 

        //List Of Consoltant For Show Site Side 
        Task<List<Window.Domain.Entities.Counseling.Counseling>> ListOfConsoltantForShowSiteSide();

        #endregion
    }
}
