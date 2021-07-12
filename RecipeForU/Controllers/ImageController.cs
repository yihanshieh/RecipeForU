using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecipeForU.Controllers
{
    public class ImageController : Controller
    {
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult UploadImage()
        {
            //ImageService.ImageTitle = string.Format("{0} {1} 圖片上傳", UserAccount.UserNo, UserAccount.UserName);
            //ImageService.ImageFolder = "~/Images";
            //ImageService.ImageSubFolder = "User";
            //ImageService.ImageName = UserAccount.UserNo;
            //ImageService.ImageExtention = "jpg";
            //ImageService.UploadImageMode = true;
            return View();
        }

        [LoginAuthorize(RoleList = "Admin")]
        [HttpPost]
        public ActionResult Uploadimage(HttpPostedFileBase file)
        {
            ImageService.UserUploadimage(file);
            if (!string.IsNullOrEmpty(ImageService.ReturnAreaName))
            {
                if(!string.IsNullOrEmpty(ImageService.ReturnParmName))
                {
                    return RedirectToAction(ImageService.ReturnActionName, ImageService.ReturnControllerName, new { area = ImageService.ReturnActionName , id=ImageService.ReturnParmValue });
                }
                else
                {
                    return RedirectToAction(ImageService.ReturnActionName, ImageService.ReturnControllerName, new { area = ImageService.ReturnActionName });
                }
            }
            if (!string.IsNullOrEmpty(ImageService.ReturnParmName))
            {
                return RedirectToAction(ImageService.ReturnActionName, ImageService.ReturnControllerName , new { id = ImageService.ReturnParmValue });
            }
            else
            {
                return RedirectToAction(ImageService.ReturnActionName, ImageService.ReturnControllerName);
            }
        }
    }
}