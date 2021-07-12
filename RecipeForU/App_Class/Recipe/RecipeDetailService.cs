using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using RecipeForU.Models;




public class RecipeDetailService
{
    public static RECIPE ShowRecomended(int place)
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            var datas = db.RECIPE.Where(m => m.recommended == true).OrderBy(m => m.time).ToList();
            var oneatatime = datas[place];
            return oneatatime;
        }
    }

    public static RECIPE ShowPopular(int place)
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            var datas = db.RECIPE.OrderBy(m => m.time).ToList();
            var oneatatime = datas[place];
            return oneatatime;
        }
    }

    public static RECIPE GetRecipeById(string id)
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            //List<RECIPE> data = new List<RECIPE>();
            var data = db.RECIPE.Where(m => m.recipe_id == id).FirstOrDefault();
            return data;
        }
    }

    public static List<sRECIPE> GetRecipeSetepsById(string id)
    {
        List<sRECIPE> steps = new List<sRECIPE>();
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            var data = db.sRECIPE.Where(m => m.recipe_id == id).OrderBy(m => m.step_id);
            foreach (var item in data)
            {
                steps.Add(item);
            }
            return steps;
        }
    }

    public static List<eRECIPE> GetRecipeElementsById(string id)
    {
        List<eRECIPE> elements = new List<eRECIPE>();
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            var data = db.eRECIPE.Where(m => m.recipe_id == id);
            foreach (var item in data)
            {
                elements.Add(item);
            }
            return elements;
        }
    }

    public static string GetRecipeAuthorById(string id)
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            string mbid = (from s in db.RECIPE where s.recipe_id == id select s.recipe_author).First();
            string aka = (from s in db.MEMBER where s.member_id == mbid select s.aka).First();
            return aka;
        }
    }

    public static bool CheckFavored(string memberid, string recipeid)
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            bool favoried = true;
            if (db.rMEMBER.Where(m => m.recipe_id == recipeid && m.member_id == memberid).FirstOrDefault() == null)
            {
                favoried = false;
            }

            return favoried;
        }
    }

    public static void AddFavor(string memberid, string recipeid)
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            rMEMBER data = new rMEMBER();
            data.recipe_id = recipeid;
            data.member_id = memberid;
            db.rMEMBER.Add(data);
            db.SaveChanges();
        }
    }
    public static void PopFavor(string memberid, string recipeid)
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            if (db.rMEMBER.Where(m => m.recipe_id == recipeid && m.member_id == memberid) != null)
            {
                var data = db.rMEMBER.Where(m => m.recipe_id == recipeid && m.member_id == memberid).FirstOrDefault();
                db.rMEMBER.Remove(data);
                db.SaveChanges();
            }
        }
    }
    public static void AddViewTimes(string id)
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            var data = db.RECIPE.Where(m => m.recipe_id == id).FirstOrDefault();
            data.view_times += 1;
            db.SaveChanges();
        }
    }

    public static int CountFavored(string id)
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            var data = db.rMEMBER.Where(m => m.recipe_id == id).Count();
            return data;
        }
    }






    public static void EditRecipe(string id, RECIPE newdata)
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            var data = db.RECIPE.Where(m => m.recipe_id == id).First();
            data.recipe_name = newdata.recipe_name;
            data.recipe_intro = newdata.recipe_intro;
            db.SaveChanges();
        }
    }
    public static void EditRecipeElements(string prevdata, List<eRECIPE> newdata)
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            var getprevdata = db.eRECIPE.Where(m => m.recipe_id == prevdata);
            foreach (var item in getprevdata)
            {
                item.element_id = "To be deleted";
            }
            foreach (var item in newdata)
            {
                eRECIPE catchnewdata = new eRECIPE();
                catchnewdata.element_id = item.element_id;
                catchnewdata.qty = item.qty;
                catchnewdata.recipe_id = prevdata;
                db.eRECIPE.Add(catchnewdata);
            }
            db.SaveChanges();
        }
    }
    public static void EditRecipeSteps(string prevdata, List<sRECIPE> newdata)
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            var getprevdata = db.sRECIPE.Where(m => m.recipe_id == prevdata);
            foreach (var item in getprevdata)
            {
                item.step_id = "To be deleted";
            }
            int i = 1;
            foreach (var item in newdata)
            {
                sRECIPE catchnewdata = new sRECIPE();
                catchnewdata.step_body = item.step_body;
                catchnewdata.step_id = i.ToString();
                catchnewdata.recipe_id = prevdata;
                db.sRECIPE.Add(catchnewdata);
                i++;
            }
            db.SaveChanges();
        }
    }
}


