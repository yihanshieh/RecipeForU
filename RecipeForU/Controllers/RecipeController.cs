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
    public class RecipeController : Controller
    {

        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult List()
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var datas = db.RECIPE.Where(m => m.recipe_id != " ").OrderByDescending(m => m.time).ToList();
                return View(datas);
            }
        }
        [LoginAuthorize(RoleList = "Admin, User")]
        public ActionResult MyList()
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var datas = db.RECIPE.Where(m => m.recipe_author == UserAccount.UserNo).OrderBy(m => m.time).ToList();
                return View(datas);
            }
        }

        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult AdminList()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult GetDataList()
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var datas = db.RECIPE.Where(m => m.recipe_id != "").ToList();
                return Json(new { data = datas }, JsonRequestBehavior.AllowGet);
            }
        }

        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult GetFullDataList()
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var datas = db.RECIPE.ToList();
                return Json(new { data = datas }, JsonRequestBehavior.AllowGet);
            }
        }


        //OLD ONE
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Detail(int id = 0)
        {
            if (id == 0) return View(new RECIPE());
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var data = db.RECIPE.Where(m => m.rowid == id).FirstOrDefault();
                data.view_times += 1;
                db.SaveChanges();
                if (data != null) return View(data);
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Edit(string id)
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var data = db.RECIPE.Where(m => m.recipe_id == id).FirstOrDefault();
                if (data == null) return RedirectToAction("List");
                return View(data);
            }
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Edit(RECIPE model)
        {
            if (!ModelState.IsValid) return View(model);
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var data = db.RECIPE.Where(m => m.recipe_id == model.recipe_id).FirstOrDefault();
                if (data == null) return View(model);
                data.recipe_author = model.recipe_author;
                data.recipe_cover = model.recipe_cover;
                data.recipe_id = model.recipe_id;
                data.recipe_intro = model.recipe_intro;
                data.recipe_name = model.recipe_name;
                data.rowid = model.rowid;
                data.time = model.time;
                data.view_times = model.view_times;
                db.SaveChanges();
                return RedirectToAction("List");
            }
        }


        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult AddRecipe()
        {
            RECIPE model = new RECIPE()
            {
                time = DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult AddRecipe(FormCollection collection)
        {
            //接步驟資料
            var inputCount = 0;
            var inputValues = new List<string>();
            string inputname, inputintro;
            if (int.TryParse(collection["TextBoxCount"], out inputCount))
            {
                for (int i = 1; i <= inputCount; i++)
                {
                    if (!string.IsNullOrWhiteSpace(collection["textbox" + i]))
                    {
                        inputValues.Add(collection["textbox" + i]);
                    }
                }
            }
            TempData["InputResult"] = inputValues;

            //接食材資料
            var inputCountx = 0;
            var inputValuesx = new List<string>();
            var inputValuesxq = new List<string>();
            if (int.TryParse(collection["TextBoxCountx"], out inputCountx))
            {
                for (int i = 1; i <= inputCountx; i++)
                {
                    if (!string.IsNullOrWhiteSpace(collection["textboxx" + i]))
                    {
                        inputValuesx.Add(collection["textboxx" + i]);
                        inputValuesxq.Add(collection["textboxxq" + i]);
                    }
                }
            }
            TempData["InputResultx"] = inputValuesx;

            //接食譜資料
            inputname = collection["textbox_name"];
            inputintro = collection["textbox_intro"];
            TempData["InputResultname"] = inputname;
            TempData["InputResultintro"] = inputintro;

            //傳入資料庫
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                //食譜資料
                RECIPE data = new RECIPE();
                data.recipe_author = UserAccount.UserNo;
                data.recipe_cover = "0";
                data.recipe_id = "尚未編號";
                data.recipe_intro = TempData["InputResultintro"].ToString();
                data.recipe_name = TempData["InputResultname"].ToString();
                data.recommended = false;
                data.time = DateTime.Now;
                data.view_times = 0;
                db.RECIPE.Add(data);
                db.SaveChanges();

                //步驟資料
                foreach (var item in (List<string>)TempData["InputResult"])
                {
                    sRECIPE sdata = new sRECIPE();
                    sdata.recipe_id = "尚未編號";
                    sdata.step_body = item;
                    sdata.step_cover = "0";
                    sdata.step_id = "0";
                    db.sRECIPE.Add(sdata);
                }
                //食材資料
                int l = 0;
                foreach (var item in (List<string>)TempData["InputResultx"])
                {
                    eRECIPE edata = new eRECIPE();
                    edata.recipe_id = "尚未編號";
                    edata.element_id = item;
                    edata.qty = inputValuesxq[l];
                    db.eRECIPE.Add(edata);
                    l++;
                }
                db.SaveChanges();
                return RedirectToAction("List");
            }
        }


        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Delete(string id)
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var data = db.RECIPE.Where(m => m.recipe_id == id).FirstOrDefault();
                if (data != null)
                {
                    db.RECIPE.Remove(data);
                    db.SaveChanges();
                }
                var sdata = db.sRECIPE.Where(m => m.recipe_id == id).FirstOrDefault();
                if (sdata != null)
                {
                    db.sRECIPE.Remove(sdata);
                    db.SaveChanges();
                }
                var edata = db.eRECIPE.Where(m => m.recipe_id == id).FirstOrDefault();
                if (edata != null)
                {
                    db.eRECIPE.Remove(edata);
                    db.SaveChanges();
                }
                return RedirectToAction("List", "Recipe");
            }
        }

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
        [HttpPost]
        public JsonResult FileUpload()
        {
            string paths = "";
            //多張圖片上傳
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i];
                var fileName = Guid.NewGuid().ToString("N");
                var filePath = Server.MapPath("~/Image/Temp/");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                paths += "~/Image/Temp/" + fileName + ",";
                file.SaveAs(Path.Combine(filePath, fileName));
            }
            return Json(new { paths }, JsonRequestBehavior.AllowGet);
        }

        //NEW ONE
        [AllowAnonymous]
        [HttpGet]
        public ActionResult RecipeDetail(string id)
        {
            RecipeDetailViewModel RecipeDatas = new RecipeDetailViewModel();
            RecipeDatas.RECIPE = RecipeDetailService.GetRecipeById(id);
            RecipeDatas.sRECIPE = RecipeDetailService.GetRecipeSetepsById(id);
            RecipeDatas.eRECIPE = RecipeDetailService.GetRecipeElementsById(id);
            RecipeDatas.Favored = RecipeDetailService.CheckFavored(UserAccount.UserNo, id);
            RecipeDatas.Author = RecipeDetailService.GetRecipeAuthorById(id);
            RecipeDetailService.AddViewTimes(id);
            return View(RecipeDatas);
        }

        [LoginAuthorize(RoleList = "Admin,User")]
        public ActionResult Pop(string id)
        {
            bool Favored = RecipeDetailService.CheckFavored(UserAccount.UserNo, id);
            if (Favored == true)
            {
                RecipeDetailService.PopFavor(UserAccount.UserNo, id);
            }
            return View();

        }
        [LoginAuthorize(RoleList = "Admin,User")]
        public ActionResult Put(string id)
        {
            bool flag = RecipeDetailService.CheckFavored(UserAccount.UserNo, id);
            if (flag == false)
            {
                RecipeDetailService.AddFavor(UserAccount.UserNo, id);
            }
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult EditRecipeDetail(string id)
        {
            RecipeDetailViewModel RecipeDatas = new RecipeDetailViewModel();
            RecipeDatas.RECIPE = RecipeDetailService.GetRecipeById(id);
            RecipeDatas.sRECIPE = RecipeDetailService.GetRecipeSetepsById(id);
            RecipeDatas.eRECIPE = RecipeDetailService.GetRecipeElementsById(id);
            return View(RecipeDatas);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult EditRecipeDetail(string id, RecipeDetailViewModel model)
        {
            RecipeDetailService.EditRecipe(id, model.RECIPE);
            RecipeDetailService.EditRecipeElements(id, model.eRECIPE);
            RecipeDetailService.EditRecipeSteps(id, model.sRECIPE);
            return View("List", "Recipe");
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult SetRecommended(string id)
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var data = db.RECIPE.Where(m => m.recipe_id == id).FirstOrDefault();
                data.recommended = true;
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult CancelRecommended(string id)
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var data = db.RECIPE.Where(m => m.recipe_id == id).FirstOrDefault();
                data.recommended = false;
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }

        [LoginAuthorize(RoleList = "Admin, User")]
        public ActionResult MyFavoredList()
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var ResultList = (from s in db.RECIPE
                                  join a in db.rMEMBER on s.recipe_id equals a.recipe_id
                                  where a.member_id == UserAccount.UserNo
                                  select s).OrderBy(s => s.time).ToList();
                return View(ResultList);
            }
        }

    }
}

