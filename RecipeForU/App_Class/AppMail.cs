using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RecipeForU.Models;

/// <summary>
/// 系統發出電子信件類別
/// </summary>
public class AppMail : BaseClass
{
    /// <summary>
    /// 使用者註冊信件
    /// </summary>
    /// <param name="email">使用者帳號</param>
    /// <returns></returns>
    public string UserRegister(string email)
    {
        using (RecipeForUEntities db = new RecipeForUEntities())
        {
            var data = db.MEMBER.Where(m => m.email == email).FirstOrDefault();
            if (data == null) return string.Format( "查無使用者代號:{0}!!" , email);
            if (string.IsNullOrEmpty(data.email)) return "使用者電子信箱空白,無法寄出!!";
            using (GmailService gmail = new GmailService())
            {
                var str_url = string.Format("/User/Verify/{0}", data.member_id);
                var str_link = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, str_url);
                string str_subject = string.Format("{0} - 帳號 {1} 成功建立通知!!",  AppService.AppName , email);
                string str_body = "<br/><br/>";
                str_body += "很高興告訴您，您的 " + AppService.AppName + " 帳戶已經成功建立. <br/>";
                str_body += "請按下下方連結完成驗證您的帳號程序!!<br/><br/>";
                str_body += "<a href='" + str_link + "'>" + str_link + "</a> ";
                str_body += "<br/><br/>";
                str_body += "本信件由電腦系統自動寄出,請勿回信!!<br/><br/>";
                str_body += string.Format("{0} 系統開發團隊敬上", AppService.AppName);

                gmail.ReceiveEmail = data.email;
                gmail.Subject = str_subject;
                gmail.Body = str_body;
                gmail.Send();
                return gmail.MessageText;
            }
        }
    }
}
