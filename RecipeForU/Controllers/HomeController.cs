using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecipeForU.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]

        [LoginAuthorize(RoleList ="User,Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult Dashboard()
        {
            ViewBag.Message = "Your dashboard page.";

            return View();
        }
    }
}