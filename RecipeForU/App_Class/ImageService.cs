﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

public static class ImageService
{
    public static string ImageTitle { get; set; } = "圖片上傳";
    public static string ImageFolder { get; set; } = "~/Images";
    public static string ImageSubFolder { get; set; } = "App";
    public static string ImageName { get; set; } = "001";
    public static string ImageExtention { get; set; } = "jpg";
    public static string ReturnAreaName { get; set; } = "";
    public static string ReturnControllerName { get; set; } = "Home";
    public static string ReturnActionName { get; set; } = "Index";
    public static string ReturnParmName { get; set; } = "";
    public static string ReturnParmValue { get; set; } = "";
    public static string ImageFolderName { get { return string.Format("{0}/{1}", ImageFolder, ImageSubFolder); } }
    public static string ImageFileName { get { return string.Format("{0}.{1}", ImageName, ImageExtention); } } 
    public static string ImageUrl
    {
        get
        {
            return string.Format("~/{0}/{1}/{2}.{3}?t={4}", ImageFolder, ImageSubFolder, ImageName, ImageExtention, DateTime.Now.ToString("yyyyMMddHHmmss"));
        }
    }
    public static string RecipeImageUrl { get { return string.Format("{0}/{1}/{2}.jpg?t={3}", ImageFolder, ImageSubFolder, RecipeService.RecipeID,DateTime.Now.ToString("yyyyMMddHHmmss")); } }
    public static string GetImageUrl(int imageName)
    {
        return string.Format("~/{0}/{1}/{2}.{3}?t={4}", ImageFolder, ImageSubFolder, imageName, ImageExtention, DateTime.Now.ToString("yyyyMMddHHmmss"));
    }

    public static void CheckImageFolder()
    {
        if (!Directory.Exists(HttpContext.Current.Server.MapPath(ImageFolderName)))
            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(ImageFolderName));
    }

   public static bool UploadImageMode { get; set; } = false;
    public static void ReturnAction(string areaName ,string controllerName , string actionName , string parmName , string parmValue)
    {
        ReturnAreaName = areaName;
        ReturnControllerName = controllerName;
        ReturnActionName = actionName;
        ReturnParmName = parmName;
        ReturnParmValue = parmValue;
    }
    public static void UserUploadimage(HttpPostedFileBase file)
    {
        if (file != null)
        {
            if (file.ContentLength > 0)
            {
                var path = Path.Combine(HttpContext.Current.Server.MapPath(ImageFolderName), ImageFileName);
                if (File.Exists(path)) File.Delete(path);
                file.SaveAs(path);
            }
        }
        UploadImageMode = false;
    }
}
