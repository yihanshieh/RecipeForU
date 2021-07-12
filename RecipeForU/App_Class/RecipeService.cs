using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using RecipeForU.Models;

public static class RecipeService
{
    public static string RecipeID { get; set; } = NewRecipeID();
    public static string ChangeRecipeID()
    {
        RecipeID = NewRecipeID();
        return RecipeID;
    }

    public static string NewRecipeID()
    {
        string str_now = DateTime.Now.ToString("yyyyMMddHHmmssff");
        string str_guid = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
        return string.Format("{0}_{1}" , str_now , str_guid);
    }

    public static string RecipeImageUrl(string imageName)
    {
        return string.Format("~/Images/Uploads/Recipe/RecipeCover/{0}.jpg", imageName);
    }

    public static string StepImageUrl(string imageName)
    {
        return string.Format("~/Images/Uploads/Recipe/StepCover/{0}.jpg", imageName);
    }

    public static string TempImageUrl(string imageName)
    {
        return string.Format("~/Images/Uploads/Temp/{0}.jpg", imageName);
    }

    public static RecipeViewModel Recipe
    {
        get
        {
            if (HttpContext.Current.Session["Recipe"] == null) return new RecipeViewModel();
            return (RecipeViewModel)HttpContext.Current.Session["Recipe"];
        }
        set { HttpContext.Current.Session["Recipe"] = value; }
    }

    public static List<eRECIPE> RecipeElement
    {
        get
        {
            if (HttpContext.Current.Session["RecipeElement"] == null) return new List<eRECIPE>();
            return (List<eRECIPE>)HttpContext.Current.Session["RecipeElement"];
        }
        set { HttpContext.Current.Session["RecipeElement"] = value; }
    }

    public static List<sRECIPE> RecipeStep
    {
        get
        {
            if (HttpContext.Current.Session["RecipeStep"] == null) return new List<sRECIPE>();
            return (List<sRECIPE>)HttpContext.Current.Session["RecipeStep"];
        }
        set { HttpContext.Current.Session["RecipeStep"] = value; }
    }

    public static sRECIPE GetRecipeStep(string rowID)
    {
        int int_rowid = int.Parse(rowID);
        return RecipeStep.Where(m => m.rowid == int_rowid).FirstOrDefault();
    }

    private static void MoveFileFromTemp(string typeName, string fileName)
    {
        string str_file = TempImageUrl(fileName);
        string str_file_name = HttpContext.Current.Server.MapPath(str_file);
        string str_new = "";
        string str_new_name = "";
        if (File.Exists(str_file_name))
        {
            if (typeName == "Recipe") str_new = RecipeImageUrl(fileName);
            if (typeName == "Step") str_new = StepImageUrl(fileName);
            if (!string.IsNullOrEmpty(str_new))
            {
                str_new_name = HttpContext.Current.Server.MapPath(str_new);
                if (File.Exists(str_new_name)) File.Delete(str_new_name);
                File.Copy(str_file_name, str_new_name);
                File.Delete(str_file_name);
            }
        }
    }

    public static void AddRecipeData()
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            RECIPE newRecipe = new RECIPE();
            newRecipe.recipe_id = RecipeID;
            newRecipe.recipe_name = Recipe.recipe_name;
            newRecipe.recipe_author = UserAccount.UserNo;
            newRecipe.time = DateTime.Now;
            newRecipe.view_times = 0;
            newRecipe.recipe_intro = Recipe.recipe_intro;
            newRecipe.recipe_cover = RecipeID;
            
            db.RECIPE.Add(newRecipe);
            db.SaveChanges();
            MoveFileFromTemp("Recipe", RecipeID);

            if (RecipeElement.Count() > 0)
            {
                foreach (var item in RecipeElement)
                {
                    db.eRECIPE.Add(item);
                }
                db.SaveChanges();
            }
            if (RecipeStep.Count() > 0)
            {
                foreach (var item in RecipeStep)
                {
                    db.sRECIPE.Add(item);
                    MoveFileFromTemp("Step", item.step_cover);
                }
                db.SaveChanges();
            }
        }
    }
}
