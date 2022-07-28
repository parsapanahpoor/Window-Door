using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window.Domain.Entities.Location;
using Window.Domain.ViewModels.Admin.State;
using Window.Domain.ViewModels.Common;

namespace Window.Application.Services.Interfaces
{
    public interface IStateService
    {
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
