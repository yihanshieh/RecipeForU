using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecipeForU.Models;
using PagedList;

namespace RecipeForU.Controllers
{
    public class RecipesController : Controller
    {
        //[AllowAnonymous]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var datas = db.RECIPE
                    .Where(m => m.recipe_id != null && m.recipe_id.Trim() != "")
                    .OrderByDescending(m => m.time)
                    .ToPagedList(page, pageSize);
                return View(datas);
            }
        }

        // [AllowAnonymous]
        [LoginAuthorize(RoleList = "Admin")]
        [HttpGet]
        public ActionResult AddRecipe(string id = "")
        {
            if (string.IsNullOrEmpty(id))
            {
                RecipeService.ChangeRecipeID();
                RecipeViewModel model = new RecipeViewModel()
                {
                    ActionNo = "Add",
                    RowID = "0",
                    StepNo = "1",
                    recipe_id = RecipeService.RecipeID,
                    recipe_name = "",
                    recipe_intro = ""
                };

                RecipeService.Recipe = model;
                RecipeService.RecipeElement = new List<eRECIPE>();
                RecipeService.RecipeStep = new List<sRECIPE>();
                return View(model);
            }
            else
            {
                if (RecipeService.RecipeStep.Count > 0)
                {
                    RecipeService.RecipeStep = RecipeService.RecipeStep.OrderBy(m => m.step_id).ToList();
                }
                return View(RecipeService.Recipe);
            }
        }

        [LoginAuthorize(RoleList = "Admin")]
        [HttpPost]
        public ActionResult AddRecipe(RecipeViewModel model)
        {
            RecipeService.Recipe = model;
            string str_action_no = "";
            if (model.ActionNo == "ElementAdd") str_action_no = model.ActionNo;
            if (model.ActionNo == "ElementEdit") str_action_no = model.ActionNo;
            if (model.ActionNo == "ElementDelete") str_action_no = model.ActionNo;
            if (model.ActionNo == "StepAdd") str_action_no = model.ActionNo;
            if (model.ActionNo == "StepEdit") str_action_no = model.ActionNo;
            if (model.ActionNo == "StepDelete") str_action_no = model.ActionNo;
            if (model.ActionNo == "SaveRecipe") str_action_no = model.ActionNo;
            if (model.ActionNo == "RecipeUpload") str_action_no = "UploadImage";
            if (model.ActionNo == "StepUpload") str_action_no = "UploadImage";

            if (!string.IsNullOrEmpty(str_action_no)) return RedirectToAction(str_action_no);
            return RedirectToAction("AddRecipe", new { id = RecipeService.RecipeID });
        }

        // [AllowAnonymous]
        [LoginAuthorize(RoleList = "Admin")]
        [HttpGet]
        public ActionResult ElementAdd()
        {
            int int_rowid = 1;
            if (RecipeService.RecipeElement.Count > 0)
            {
                var data = RecipeService.RecipeElement.OrderByDescending(m => m.rowid).First();
                int_rowid = data.rowid + 1;
            }

            eRECIPE model = new eRECIPE();
            model.rowid = int_rowid;
            model.recipe_id = RecipeService.RecipeID;
            model.element_id = "";
            model.qty = "";
            model.ActionNo = "";
            return View(model);
        }

        // [AllowAnonymous]
        [LoginAuthorize(RoleList = "Admin")]
        [HttpPost]
        public ActionResult ElementAdd(eRECIPE model)
        {
            if (!ModelState.IsValid) return View(model);
            RecipeService.RecipeElement.Add(model);

            if (model.ActionNo == "SaveContinue") return RedirectToAction("ElementAdd");
            return RedirectToAction("AddRecipe", new { id = RecipeService.RecipeID });
        }

        // [AllowAnonymous]
        [LoginAuthorize(RoleList = "Admin")]
        [HttpGet]
        public ActionResult ElementEdit()
        {
            int int_rowid = int.Parse(RecipeService.Recipe.RowID);
            var model = RecipeService.RecipeElement.Where(m => m.rowid == int_rowid).FirstOrDefault();
            return View(model);
        }

        // [AllowAnonymous]
        [LoginAuthorize(RoleList = "Admin")]
        [HttpPost]
        public ActionResult ElementEdit(eRECIPE model)
        {
            if (!ModelState.IsValid) return View(model);
            var data = RecipeService.RecipeElement.Where(m => m.rowid == model.rowid).First();
            var index = RecipeService.RecipeElement.IndexOf(data);
            if (index >= 0) RecipeService.RecipeElement[index] = model;
            return RedirectToAction("AddRecipe", new { id = RecipeService.RecipeID });
        }

        //[AllowAnonymous]
        [LoginAuthorize(RoleList = "Admin")]
        [HttpGet]
        public ActionResult ElementDelete()
        {
            int int_rowid = int.Parse(RecipeService.Recipe.RowID);
            var model = RecipeService.RecipeElement.Where(m => m.rowid == int_rowid).FirstOrDefault();
            if (model != null) { RecipeService.RecipeElement.Remove(model); }
            return RedirectToAction("AddRecipe", new { id = RecipeService.RecipeID });
        }

        //[AllowAnonymous]
        [LoginAuthorize(RoleList = "Admin")]
        [HttpGet]
        public ActionResult StepAdd()
        {
            int int_rowid = 1;
            string str_step_id = "01";
            if (RecipeService.RecipeStep.Count > 0)
            {
                var data = RecipeService.RecipeStep.OrderByDescending(m => m.rowid).First();
                //int_rowid = data.rowid + 1;
                str_step_id = (int.Parse(data.step_id) + 1).ToString().PadLeft(2, '0');
            }

            sRECIPE model = new sRECIPE();
            //model.rowid = int_rowid;
            model.recipe_id = RecipeService.RecipeID;
            model.step_id = str_step_id;
            model.step_body = "";
            model.step_cover = RecipeService.NewRecipeID();
            return View(model);
        }

        //[AllowAnonymous]
        [LoginAuthorize(RoleList = "Admin")]
        [HttpPost]
        public ActionResult StepAdd(sRECIPE model)
        {
            if (!ModelState.IsValid) return View(model);
            RecipeService.RecipeStep.Add(model);
            if (model.ActionNo == "SaveContinue") return RedirectToAction("StepAdd");
            return RedirectToAction("AddRecipe", new { id = RecipeService.RecipeID });
        }

        // [AllowAnonymous]
        [LoginAuthorize(RoleList = "Admin")]
        [HttpGet]
        public ActionResult StepEdit()
        {
            int int_rowid = int.Parse(RecipeService.Recipe.RowID);
            var model = RecipeService.RecipeStep.Where(m => m.rowid == int_rowid).FirstOrDefault();
            return View(model);
        }

        // [AllowAnonymous]
        [LoginAuthorize(RoleList = "Admin")]
        [HttpPost]
        public ActionResult StepEdit(sRECIPE model)
        {
            if (!ModelState.IsValid) return View(model);
            var data = RecipeService.RecipeStep.Where(m => m.rowid == model.rowid).First();
            var index = RecipeService.RecipeStep.IndexOf(data);
            if (index >= 0) RecipeService.RecipeStep[index] = model;
            return RedirectToAction("AddRecipe", new { id = RecipeService.RecipeID });
        }

        // [AllowAnonymous]
        [LoginAuthorize(RoleList = "Admin")]
        [HttpGet]
        public ActionResult StepDelete()
        {
            int int_rowid = int.Parse(RecipeService.Recipe.RowID);
            var model = RecipeService.RecipeStep.Where(m => m.rowid == int_rowid).FirstOrDefault();
            if (model != null)
            {
                RecipeService.RecipeStep.Remove(model);
                if (RecipeService.RecipeStep.Count > 0)
                {
                    for (int i = 0; i < RecipeService.RecipeStep.Count; i++)
                    {
                        RecipeService.RecipeStep[i].step_id = (i + 1).ToString().PadLeft(2, '0');
                    }
                }
            }
            return RedirectToAction("AddRecipe", new { id = RecipeService.RecipeID });
        }

        public ActionResult SaveRecipe()
        {
            RecipeService.AddRecipeData();
            return RedirectToAction("Index", "Home");
        }

        //[AllowAnonymous]
        [LoginAuthorize(RoleList = "Admin")]
        [HttpGet]
        public ActionResult UploadImage()
        {
            if (RecipeService.Recipe.ActionNo == "RecipeUpload")
            {
                ImageService.ImageTitle = string.Format("食譜： {0} 圖片上傳", RecipeService.Recipe.recipe_name);
                ImageService.ImageName = RecipeService.Recipe.recipe_id;
            }
            if (RecipeService.Recipe.ActionNo == "StepUpload")
            {
                var data = RecipeService.GetRecipeStep(RecipeService.Recipe.RowID);
                ImageService.ImageTitle = string.Format("食譜： {0} 步驟： {1} 圖片上傳", RecipeService.Recipe.recipe_name, data.step_id);
                ImageService.ImageName = data.step_cover;
            }
            ImageService.ImageFolder = "~/Images/Uploads";
            ImageService.ImageSubFolder = "Temp";
            ImageService.ImageExtention = "jpg";
            ImageService.UploadImageMode = true;
            ImageService.ReturnAction("", "Recipes", "AddRecipe", "id", "image");
            return RedirectToAction("UploadImage", "Image");
        }
    }
}