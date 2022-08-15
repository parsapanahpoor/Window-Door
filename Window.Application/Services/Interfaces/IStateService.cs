using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Location;
using Window.Domain.ViewModels.Admin.State;
using Window.Domain.ViewModels.Common;
using Window.Domain.ViewModels.Site.API;

namespace Window.Application.Services.Interfaces
{
    public interface IStateService
    {
        //Get Location By Unique Name
        Task<State> GetLocationByUniqueName(string uniqueName);

        //Get List Of States 
        Task<List<State>> GetListOfState();

        //Get List Of City By City Name
        Task<List<StateAPIViewModel>> GetListOfCityByCityName(string cityName);

        Task<State> GetStateById(ulong stateId);

        Task<List<SelectListViewModel>> GetStateChildren(ulong stateId);

        Task<List<SelectListViewModel>> GetAllCountries();

        Task<bool> IsExistsStateById(ulong stateId);

        Task<CreateStateResult> CreateState(CreateStateViewModel stateViewModel);

        Task<FilterStateViewModel> FilterState(FilterStateViewModel filter);

        Task<EditStateViewModel> FillEditStateViewModel(ulong stateId);

        Task<EditStateResult> EditState(EditStateViewModel stateViewModel);

        Task<bool> DeleteState(ulong stateId);
    }
}
