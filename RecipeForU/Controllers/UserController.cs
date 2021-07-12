using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecipeForU.Models;
using RecipeForU.Models.ViewModel;

namespace RecipeForU.Controllers
{
    public class UserController : Controller
    {
        [HttpGet] // GET: User
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult List()
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var datas = db.MEMBER.Where(m => m.member_type == "U").OrderBy(m => m.rowid).ToList();
                return View(datas);
            }

        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            UserAccount.Logout();
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            using (Cryptographys cryp = new Cryptographys())
            {
                string str_password = cryp.SHA256Encode(model.password);
                using (RecipeForUEntities db = new RecipeForUEntities())
                {
                    var data = db.MEMBER
                        .Where(m => m.email == model.email)
                        .Where(m => m.password == str_password)
                        .Where(m => m.is_valid == true)
                        .FirstOrDefault();
                    if (data == null)
                    {
                        ModelState.AddModelError("email", "登入帳號或密碼輸入錯誤!!");
                        return View(model);
                    }
                    UserAccount.UserEmail = model.email;
                    UserAccount.Login();

                    if (UserAccount.Role == EnumList.LoginRole.Admin)
                    {
                        return RedirectToAction("Dashboard", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
        }
        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Create()
        {
            MEMBER model = new MEMBER()
            {
                birthday = DateTime.Today,
                is_valid = false
            };
            return View(model);
        }
        [HttpPost]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Create(MEMBER model)
        {

            if (!ModelState.IsValid) return View(model);
            using (Cryptographys cryp = new Cryptographys())
            {
                using (RecipeForUEntities db = new RecipeForUEntities())
                {
                    MEMBER data = new MEMBER();
                    data.member_id = "";
                    data.member_name = model.member_name;
                    data.member_type = "U";
                    data.password = cryp.SHA256Encode(model.password);
                    data.birthday = model.birthday;
                    data.email = model.email;
                    data.phone = model.phone;
                    data.gender = model.gender;
                    data.occupation = model.occupation;
                    data.is_valid = model.is_valid;
                    data.aka = model.aka;
                    db.MEMBER.Add(data);
                    db.SaveChanges();
                    return RedirectToAction("List");
                }
            }
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Edit(int id)
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var data = db.MEMBER.Where(m => m.rowid == id).FirstOrDefault();
                if (data == null) return RedirectToAction("List");
                return View(data);
            }
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Edit(MEMBER model)
        {
            if (!ModelState.IsValid) return View(model);
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var data = db.MEMBER.Where(m => m.rowid == model.rowid).FirstOrDefault();
                if (data == null) return View(model);
                data.member_name = model.member_name;
                data.birthday = model.birthday;
                data.email = model.email;
                data.phone = model.phone;
                data.gender = model.gender;
                data.occupation = model.occupation;
                data.aka = model.aka;
                db.SaveChanges();
                return RedirectToAction("List");
            }
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "Admin")]
        public ActionResult Delete(int id)
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var data = db.MEMBER.Where(m => m.rowid == id).FirstOrDefault();
                if (data != null)
                {
                    db.MEMBER.Remove(data);
                    db.SaveChanges();
                }
                return RedirectToAction("List", "User");
            }
        }
        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult ResetPassword()
        {
            ResetPasswordViewModel model = new ResetPasswordViewModel();
            return View(model);
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            using (Cryptographys cryp = new Cryptographys())
            {
                string str_password = cryp.SHA256Encode(model.NewPassword);
                using (RecipeForUEntities db = new RecipeForUEntities())
                {
                    TempData["MessageHeader"] = "使用者密碼變更";
                    var data = db.MEMBER.Where(m => m.email == UserAccount.UserEmail).FirstOrDefault();
                    if (data != null)
                    {
                        data.password = str_password;
                        db.SaveChanges();
                    }
                    TempData["MessageText"] = "密碼已更新，下次登入請使用新的密碼！";
                    return RedirectToAction("MessageText");
                }
            }
        }

        [HttpGet]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult EditMyInfo()
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var data = db.MEMBER.Where(m => m.email == UserAccount.UserEmail).FirstOrDefault();
                if (data == null) return RedirectToAction("Index");
                return View(data);
            }
        }

        [HttpPost]
        [LoginAuthorize(RoleList = "User,Admin")]
        public ActionResult EditMyInfo(MEMBER model)
        {
            if (!ModelState.IsValid) return View(model);
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var data = db.MEMBER.Where(m => m.email == UserAccount.UserEmail).FirstOrDefault();
                if (data == null) return View(model);
                data.member_name = model.member_name;
                data.birthday = model.birthday;
                data.email = model.email;
                data.phone = model.phone;
                data.gender = model.gender;
                data.occupation = model.occupation;
                data.aka = model.aka;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult MessageText()
        {
            ViewBag.MessageText = TempData["MessageText"].ToString();
            return View();
        }

        ///////////////////////////////////////////////////////////////

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            //沒有用到的remark
            string str_key = Guid.NewGuid()
                .ToString()
                .Replace("-", "")
                .ToUpper()
                .Substring(0, 20);
            MEMBER model = new MEMBER()
            {
                member_type = "U",
                birthday = DateTime.Today,
                is_valid = false
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(MEMBER model)
        {
            if (!ModelState.IsValid) return View(model);
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                var data = db.MEMBER.Where(m => m.email == model.email).FirstOrDefault();
                if (data != null)
                {
                    ModelState.AddModelError("email", "電子信箱重覆註冊!!");
                    return View(model);
                }

                using (Cryptographys cryp = new Cryptographys())
                {
                    MEMBER dataM = new MEMBER();
                    string str_password = cryp.SHA256Encode(model.password);
                    dataM.member_id = "";
                    dataM.member_name = model.member_name;
                    dataM.member_type = "U";
                    dataM.password = cryp.SHA256Encode(model.password);
                    dataM.birthday = model.birthday;
                    dataM.email = model.email;
                    dataM.phone = model.phone;
                    dataM.gender = model.gender;
                    dataM.occupation = model.occupation;
                    dataM.is_valid = model.is_valid;
                    dataM.aka = model.aka;

                    db.MEMBER.Add(dataM);
                    db.SaveChanges();

                    //寄出註冊驗證信件

                    using (AppMail mail = new AppMail())
                    {
                        mail.UserRegister(model.email);
                    }
                    TempData["HeaderText"] = "帳號註冊已完成";
                    TempData["MessageText"] = "驗證信已寄出，請前往信箱確認";
                    return RedirectToAction("MessageText");
                }

            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Verify(string id)
        {
            using (RecipeForUEntities db = new RecipeForUEntities())
            {
                bool bln_valid = false;
                TempData["MessageHeader"] = "使用者註冊電子信箱驗證";
                var data = db.MEMBER.Where(m => m.member_id == id).FirstOrDefault();
                if (data == null)
                {
                    TempData["MessageText"] = "驗證碼找不到!!";
                    return RedirectToAction("MessageText");
                }
                bln_valid = data.is_valid;
                if (bln_valid)
                {
                    TempData["MessageText"] = "帳號電子信箱已驗證過,不可重覆驗證!!";
                    return RedirectToAction("MessageText");
                }
                data.is_valid = true;
                db.SaveChanges();
                TempData["MessageText"] = "帳號電子信箱已驗證成功!!";
                return RedirectToAction("MessageText");
            }
        }
    }
}