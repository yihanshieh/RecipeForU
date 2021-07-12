using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using RecipeForU.Models;

public class RecipeViewModel
{
    [Display(Name = "動作名稱")]
    public string ActionNo { get; set; }
    [Display(Name = "明細選取的 ID")]
    public string RowID { get; set; }
    [Display(Name = "明細選取的步驟編號")]
    public string StepNo { get; set; }
    [Display(Name = "明細選取的步驟編號")]
    public string recipe_id { get; set; }
    [Display(Name = "食譜名稱")]
    public string recipe_name { get; set; }
    [Display(Name = "食譜介紹")]
    public string recipe_intro { get; set; }
    public eRECIPE RecipeElement { get; set; }
    public sRECIPE RecipeStep { get; set; }
}
