﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecipeForU.Models
{
    [MetadataType(typeof(sRECIPEMetaData))]
    public partial class sRECIPE
    {
        public string ActionNo { get; set; }
        private class sRECIPEMetaData
        {
            [Key]
            public int rowid { get; set; }
            [Display(Name = "步驟")]
            public string step_id { get; set; }
            [Display(Name = "食譜代號")]
            public string recipe_id { get; set; }
            [Display(Name = "步驟說明")]
            public string step_body { get; set; }
            [Display(Name = "步驟畫面")]
            public string step_cover { get; set; }
        }
    }
}