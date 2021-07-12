using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecipeForU.Models
{
    [MetadataType(typeof(MEMBERMetaData))]
    public partial class MEMBER
    {
        private class MEMBERMetaData
        {
            [Display(Name = "rowid")]
            public int rowid { get; set; }
            [Display(Name = "會員型")]
            public string member_type { get; set; }

            [Display(Name = "會員識別碼")] 
            public string member_id { get; set; }
            [Display(Name = "Email")]
            //[Required(ErrorMessage = "Email不可空白")]
            public string email { get; set; }
            [Display(Name = "密碼")]
            [Required(ErrorMessage = "密碼不可空白")]
            public string password { get; set; }
            [Display(Name = "姓名")]
            [Required(ErrorMessage = "姓名不可空白")]
            public string member_name { get; set; }
            [Display(Name = "電話")]
            [Required(ErrorMessage = "電話不可空白")]
            public string phone { get; set; }
            [Display(Name = "性別")]
            [Required(ErrorMessage = "性別不可空白")]
            public string gender { get; set; }
            [Display(Name = "出生日期")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
            public System.DateTime birthday { get; set; }
            [Display(Name = "職業")]
            public string occupation { get; set; }
            [Display(Name = " ")]

            public bool is_valid { get; set; }
            [Display(Name = "暱稱")]

            public string aka { get; set; }


        }

    }
}