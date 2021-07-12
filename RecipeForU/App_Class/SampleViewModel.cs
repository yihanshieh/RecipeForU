using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RecipeForU.App_Class
{
    public class SampleViewModel
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }
    public class SampleViewModel2
    {
        public int rowid { get; set; }
        public string recipe_id { get; set; }
        public string step_id { get; set; }
        public string recipe_name { get; set; }
        public string recipe_author { get; set; }
        public System.DateTime time { get; set; }
        public int view_times { get; set; }
        public string recipe_intro { get; set; }
        public string recipe_cover { get; set; }
    }
}