using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using RecipeForU.Models;


public class DashboardService
{
    public static RECIPE ShowPopular(int place)
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            var datas = db.RECIPE.OrderBy(m => m.time).ToList();
            var oneatatime = datas[place];
            return oneatatime;
        }
    }

    public static int CountRecipe()
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            var count = db.RECIPE.Count();
            return count;
        }
    }

    public static int CountMember()
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            var count = db.MEMBER.Count();
            return count;
        }
    }

}
