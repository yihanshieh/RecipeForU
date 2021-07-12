using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using RecipeForU.Models;
using RecipeForU.Models.ViewModel;

namespace RecipeForU.Controllers
{
    public class RecipeSearchingController : Controller
    {

        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult RecipeSearchListForAdmin(string keyword="")
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            { 
                var ResultList=db.RECIPE
                    .Where(m => 
                    m.recipe_name.Contains(keyword)||
                    m.recipe_id.Contains(keyword) ||
                    m.recipe_intro.Contains(keyword) ||
                    m.recipe_author.Contains(keyword))
                    .OrderBy(m=>m.time).ToList();
                return View(ResultList);
            }
        }
        public ActionResult Search(FormCollection collection)
        {
            string str_text = collection["keyword"];
            return RedirectToAction("RecipeSearchListForAdmin", "RecipeSearching", new { keyword=str_text});
            
        }

        public ActionResult SearchByElementsList(string keyword = "")
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var ResultList = (from s in db.RECIPE
                              join a in db.eRECIPE on s.recipe_id equals a.recipe_id
                              where a.element_id == keyword select s).OrderBy(s=>s.time).ToList();
                return View(ResultList);
            }
        }
        public ActionResult SearchElement(FormCollection collection)
        {
            string str_text = collection["keyword"];
            return RedirectToAction("SearchByElementsList", "RecipeSearching", new { keyword = str_text });

        }

        public ActionResult SearchByNameList(string keyword = "")
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var ResultList = db.RECIPE
                    .Where(m =>
                    m.recipe_name.Contains(keyword))
                    .OrderBy(m => m.time).ToList();
                return View(ResultList);
            }
        }
        public ActionResult SearchName(FormCollection collection)
        {
            string str_text = collection["keyword"];
            return RedirectToAction("SearchByNameList", "RecipeSearching", new { keyword = str_text });

        }
    }
}