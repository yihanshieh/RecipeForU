using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipeForU.Models.MenuModel
{
    public class TopMenu
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string ItemName { get; set; }
    }

    public class TopMenuList
    {
        public static string CurrentControllerName { get; set; } = "Members";
        public static string CurrentActionName { get; set; } = "Index";
        public static string CurrentItemName { get; set; } = "首頁";
        public static void SetCurrent(string controllerName, string actionName)
        {
            CurrentControllerName = controllerName;
            CurrentActionName = actionName;
            var model = GetMenuList()
                .Where(m => m.ControllerName == controllerName)
                .Where(m => m.ActionName == actionName)
                .FirstOrDefault();
            CurrentItemName = (model == null) ? "" : model.ItemName;
        }
        
        public static List<TopMenu> GetMenuList()
        {
            List<TopMenu> lists = new List<TopMenu>();
            LoginAuthorize lt = new LoginAuthorize();
            if (lt.RoleList == "Admin" )
            {
                lists.Add(new TopMenu() { ControllerName = "Member", ActionName = "List", ItemName = "會員管理" });
                lists.Add(new TopMenu() { ControllerName = "Signup", ActionName = "List", ItemName = "報名管理" });
                lists.Add(new TopMenu() { ControllerName = "Agenda", ActionName = "Index", ItemName = "議程管理" });
                lists.Add(new TopMenu() { ControllerName = "News", ActionName = "AdminList", ItemName = "最新消息" });
                lists.Add(new TopMenu() { ControllerName = "Contact", ActionName = "Index", ItemName = "聯絡資訊" });
                lists.Add(new TopMenu() { ControllerName = "HomePage", ActionName = "Index", ItemName = "首頁資訊" });
                lists.Add(new TopMenu() { ControllerName = "Country", ActionName = "List", ItemName = "國家資料" });
                lists.Add(new TopMenu() { ControllerName = "Member", ActionName = "Logout", ItemName = "登出後台" });
            }
            else
            {
                lists.Add(new TopMenu() { ControllerName = "Home", ActionName = "Index", ItemName = "首頁" });
                lists.Add(new TopMenu() { ControllerName = "News", ActionName = "List", ItemName = "最新消息" });
                lists.Add(new TopMenu() { ControllerName = "Home", ActionName = "Agenda", ItemName = "議程" });
                lists.Add(new TopMenu() { ControllerName = "Signup", ActionName = "Create", ItemName = "線上報名" });
                lists.Add(new TopMenu() { ControllerName = "Home", ActionName = "Contact", ItemName = "聯絡資訊" });
                if (lt.RoleList == "User")
                {
                    lists.Add(new TopMenu() { ControllerName = "Member", ActionName = "Index", ItemName = "會員專區" });
                    lists.Add(new TopMenu() { ControllerName = "Member", ActionName = "Logout", ItemName = "登出" });
                }
                else
                {
                    lists.Add(new TopMenu() { ControllerName = "Member", ActionName = "Login", ItemName = "投稿" });
                }
            }
            return lists;
        }
    }
}