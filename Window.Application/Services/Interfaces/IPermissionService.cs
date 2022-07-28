using Window.Domain.Entities.Account;
using Window.Domain.ViewModels.Access;
using Window.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window.Application.Services.Interfaces
{
    public interface IPermissionService
    {

        #region Check Permission

        Task<bool> HasUserPermission(ulong userId, string permissionName);

        #endregion

        #region Role

        Task<FilterRolesViewModel> FilterRoles(FilterRolesViewModel filter);

        Task<bool> CreateRole(CreateRoleViewModel create);

        Task<bool> IsRoleNameValid(string name, ulong roleId);

        Task<EditRoleViewModel> FillEditRoleViewModel(ulong roleId);

        Task<Role?> GetRoleById(ulong roleId);

        Task<List<SelectListViewModel>> GetSelectRolesList();

        Task<EditRoleResult> EditRole(EditRoleViewModel edit);

        Task<bool> DeleteRole(ulong roleId);

        Task<GetUserRoles> GetUserRole(ulong userId);

        Task<bool> IsUserAdmin(ulong userId);

        Task<bool> IsUserDoctor(ulong userId);

        Task<bool> IsUserSupporter(ulong userId);

        Task<bool> IsUserSeller(ulong userId);

        Task<bool> IsUserMaster(ulong userId);

        #endregion
    }
}
