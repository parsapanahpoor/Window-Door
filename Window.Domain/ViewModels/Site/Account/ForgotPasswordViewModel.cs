﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Window.Domain.ViewModels.Account
{
    public class ForgetPasswordViewModel
    {
        [Display(Name = "تلفن همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Mobile { get; set; }
    }

    public enum ForgotPasswordResult
    {
        Success,
        SuccessSendEmail,
        NotFound,
        Error,
        VerificationSmsFaildFromParsGreen,
        UserIsBlocked,
        FailSendEmail
    }
}
