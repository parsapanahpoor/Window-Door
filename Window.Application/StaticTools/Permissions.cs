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

                #region Manage Users

                list.Add(new Permission
                {
                    Id = 2,
                    CreateDate = date,
                    IsDelete = false,
                    ParentId = null,
                    PermissionUniqueName = "ManageUsers",
                    Title = "مدیریت اطلاعات کاربران"
                });

                #endregion

                #region Manage Users

                list.Add(new Permission
                {
                    Id = 3,
                    CreateDate = date,
                    IsDelete = false,
                    ParentId = null,
                    PermissionUniqueName = "ManageSellersInfo",
                    Title = "مدیریت اطلاعات فروشندگان"
                });

                #endregion

                #region Manage Articles

                list.Add(new Permission
                {
                    Id = 4,
                    CreateDate = date,
                    IsDelete = false,
                    ParentId = null,
                    PermissionUniqueName = "ManageArticles",
                    Title = "مدیریت مقالات"
                });

                #endregion

                #region Manage Questions And Answers

                list.Add(new Permission
                {
                    Id = 5,
                    CreateDate = date,
                    IsDelete = false,
                    ParentId = null,
                    PermissionUniqueName = "ManageQuestionsAndAnswers",
                    Title = "مدیریت سوال و جواب ها"
                });

                #endregion

                #region Manage Samples

                list.Add(new Permission
                {
                    Id = 6,
                    CreateDate = date,
                    IsDelete = false,
                    ParentId = null,
                    PermissionUniqueName = "ManageSamples",
                    Title = "مدیریت نمونه ها"
                });

                #endregion

                #region Manage Segments 

                list.Add(new Permission
                {
                    Id = 7,
                    CreateDate = date,
                    IsDelete = false,
                    ParentId = null,
                    PermissionUniqueName = "ManageSegments",
                    Title = "مدیریت قطعات"
                });

                #endregion

                #region Manage Glasses 

                list.Add(new Permission
                {
                    Id = 8,
                    CreateDate = date,
                    IsDelete = false,
                    ParentId = null,
                    PermissionUniqueName = "ManageGlasses",
                    Title = "مدیریت شیشه ها"
                });

                #endregion

                #region Manage Brands 

                list.Add(new Permission
                {
                    Id = 9,
                    CreateDate = date,
                    IsDelete = false,
                    ParentId = null,
                    PermissionUniqueName = "ManageBrands",
                    Title = "مدیریت برند ها"
                });

                #endregion

                #region Manage Locations 

                list.Add(new Permission
                {
                    Id = 10,
                    CreateDate = date,
                    IsDelete = false,
                    ParentId = null,
                    PermissionUniqueName = "ManageLocations",
                    Title = "مدیریت مکان ها"
                });

                #endregion

                #region List Of Transactions 

                list.Add(new Permission
                {
                    Id = 11,
                    CreateDate = date,
                    IsDelete = false,
                    ParentId = null,
                    PermissionUniqueName = "ListOfTransactions",
                    Title = "لیست تراکنش ها "
                });

                #endregion

                #region Manage Tickets 

                list.Add(new Permission
                {
                    Id = 12,
                    CreateDate = date,
                    IsDelete = false,
                    ParentId = null,
                    PermissionUniqueName = "ManageTickets",
                    Title = "مدیریت تیکت ها "
                });

                #endregion

                #region Manage Access 

                list.Add(new Permission
                {
                    Id = 13,
                    CreateDate = date,
                    IsDelete = false,
                    ParentId = null,
                    PermissionUniqueName = "ManageAccess",
                    Title = "مدیریت دسترسی ها "
                });

                #endregion

                #endregion

                // Last Id Use is : 13

                return list;
            }
        }
    }
}
