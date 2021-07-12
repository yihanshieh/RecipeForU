﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecipeForU.Models
{
    public class ResetPasswordViewModel
    {
        [Display(Name = "新的密碼")]
        [Required(ErrorMessage = "新的密碼不可空白!!")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Display(Name = "確認密碼")]
        [Required(ErrorMessage = "確認密碼不可空白!!")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "確認密碼與新的密碼不相同!!")]
        public string ConfirmPassword { get; set; }
    }
}