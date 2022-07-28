﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window.Domain.ViewModels.Access
{
    public class EditRoleViewModel
    {
        [DisplayName("Title")]
        [Required(ErrorMessage = "Please Enter {0}")]
        public string Title { get; set; }

        [DisplayName("Unique Name")]
        [Required(ErrorMessage = "Please Enter {0}")]
        public string RoleUniqueName { get; set; }

        public List<ulong>? Permissions { get; set; }

        public ulong Id { get; set; }
    }

    public enum EditRoleResult
    {
        Success,
        RoleNotFound,
        UniqueNameExists
    }
}
