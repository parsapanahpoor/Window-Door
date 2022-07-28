using Window.Domain.Entities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window.Application.StaticTools
{
    public static class PermissionsList
    {
        public static List<Permission> Permissions
        {
            get
            {
                List<Permission> list = new List<Permission>();

                var date = new DateTime(2021, 08, 12);

                #region Add Permissions

                #region Dashboard

                list.Add(new Permission
                {
                    Id = 1,
                    CreateDate = date,
                    IsDelete = false,
                    ParentId = null,
                    PermissionUniqueName = "Dashboard",
                    Title = "داشبورد"
                });

                #endregion

                #endregion

                // Last Id Use is : 

                return list;
            }
        }
    }
}
