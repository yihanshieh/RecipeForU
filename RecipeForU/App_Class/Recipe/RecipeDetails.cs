using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using DevStudio;

namespace DevStudio
{
    public class RecipeDetails
    {

        public static string GetRecipeImage(string rcid)
        {
            string str_image = string.Format("~/Images/Uploads/Recipe/RecipeCover/{0}.jpg", rcid);
            if (File.Exists(HttpContext.Current.Server.MapPath(str_image)))
                str_image = string.Format("../../Images/Uploads/Recipe/RecipeCover/{0}.jpg", rcid);
            else
                str_image = "../../Images/norecipecover.jpg";
            return str_image;
        }

        public static string GetStepImage(string stid)
        {
            string str_image = string.Format("~/Images/Uploads/Recipe/StepCover/{0}.jpg", stid);
            if (File.Exists(HttpContext.Current.Server.MapPath(str_image)))
                str_image = string.Format("../../Images/Uploads/Recipe/StepCover/{0}.jpg", stid);
            else
                str_image = "../../Images/norecipecover.jpg";
            return str_image;
        }
    }
}