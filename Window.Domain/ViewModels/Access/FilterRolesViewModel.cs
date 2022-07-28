using Window.Domain.Entities.Account;
using Window.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window.Domain.ViewModels.Access
{
    public class FilterRolesViewModel : BasePaging<Role>
    {
        public string? RoleTitle { get; set; }

        public string? RoleUniqueName { get; set; }
    }
}
