using MI.Core.Common;
using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MI.Application
{
    public class Public
    {
        private IEmployeeService _employeeInfoService;
        public Public(IEmployeeService employeeInfoService)
        {
            _employeeInfoService = employeeInfoService;
        }
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="sImgName">通过时间戳在页面生成的唯一名字</param>
        /// <param name="iEid">用户id</param>
        /// <param name="sHZorRJZtype">护照(p)or入境章(e)</param>
        /// <returns></returns>
        public static ResultModel BLLUploadFile(string sImgName, int iEid, string sHZorRJZtype)
        {
            ResultModel result = new ResultModel(500, string.Empty); //返回结果

            Stream stream = null;
            Image image = null;
            Image Tbimage = null;
            string sMpath = string.Empty;
            string sMpath2 = string.Empty;
            if (sHZorRJZtype == "p")
            {
                sMpath = "~/images/User/Original/";
                sMpath2 = "~/images/User/Thumbnail/";
            }
            else if (sHZorRJZtype == "e")
            {
                sMpath = "~/images/UserES/Original/";
                sMpath2 = "~/images/UserES/Thumbnail/";
                sImgName = sImgName + "e";
            }
            else
            {
                sMpath = "~/images/UserIDC/Original/";
                sMpath2 = "~/images/UserIDC/Thumbnail/";
                sImgName = sImgName + "c";
            }
            try
            {
                if (HttpContext.Current.Request.Files.Count > 0 && !string.IsNullOrWhiteSpace(sImgName) && HttpContext.Current.Request.Files[0].ContentType.Contains("image"))
                {
                    HttpPostedFile file = HttpContext.Current.Request.Files["imgFile"];
                    if (file.ContentLength > 6 * 1024 * 1024)//文件大小超过限制要求
                    {
                        result.result = (int)ErrorEnum.ImageSize;
                        result.tips = ErrorEnum.ImageSize.ToString();
                        return result;
                    }
                    stream = file.InputStream;
                    image = Image.FromStream(stream);
                    //if (image.Width < image.Height)
                    //{
                    //    //如果宽小于高 就旋转
                    //}
                    Tbimage = image.GetThumbnailImage(44, 44, null, IntPtr.Zero);
                    string sHouz = string.Empty;
                    if (!checkFileExtension(file.FileName, out sHouz))
                    {
                        result.result = (int)ErrorEnum.ImageFormat;
                        result.tips = ErrorEnum.ImageFormat.ToString();
                        return result;
                    }

                    string sPath = HttpContext.Current.Server.MapPath(sMpath);
                    string sPath2 = HttpContext.Current.Server.MapPath(sMpath2);
                    if (!Directory.Exists(sPath))
                    {
                        Directory.CreateDirectory(sPath);
                    }
                    if (!Directory.Exists(sPath2))
                    {
                        Directory.CreateDirectory(sPath2);
                    }
                    image.Save(sPath + sImgName + sHouz);
                    Tbimage.Save(sPath2 + sImgName + sHouz);
                    result.result = (int)ErrorEnum.Success;
                    result.tips = EncryptHelper.AESEncrypt(sImgName + sHouz);
                }
            }
            catch (Exception e)
            {
                result.result = (int)ErrorEnum.Error;
                result.tips = "500";
                //写入错误日记
                AOUnity.WriteLog(e);
            }
            finally
            {
                Tbimage?.Dispose();
                image?.Dispose();
                stream?.Close();
            }
            return result;
        }

        /// <summary>
        /// 下载当前用户的文件
        /// </summary>
        /// <param name="modelUser">当前用户的model</param>
        /// <param name="sHZorRJZtype">p=护照 or e=入境章</param>
        public static void BLLDownloadFile(Employee modelUser, string sHZorRJZtype)
        {
            if (modelUser == null || (sHZorRJZtype == "p" && string.IsNullOrWhiteSpace(modelUser.f_PassportURL))
                || (sHZorRJZtype == "e" && string.IsNullOrWhiteSpace(modelUser.f_EntrystampURL)) || (sHZorRJZtype == "c" && string.IsNullOrWhiteSpace(modelUser.f_IDCardURL)))
            {
                return;
            }
            string sImgUrl = sHZorRJZtype == "p" ? modelUser.f_PassportURL : sHZorRJZtype == "e" ? modelUser.f_EntrystampURL : modelUser.f_IDCardURL;
            string Url = EncryptHelper.AESDecrypt(sImgUrl);
            string sPath = (sHZorRJZtype == "p" ? HttpContext.Current.Server.MapPath("~/images/User/Original/") : sHZorRJZtype == "e" ? HttpContext.Current.Server.MapPath("~/images/UserES/Original/") : HttpContext.Current.Server.MapPath("~/images/UserIDC/Original/")) + Url;
            string sHouz = string.Empty;
            if (System.IO.File.Exists(sPath) && checkFileExtension(Url, out sHouz))
            {
                FileStream fs = null;
                try
                {
                    string sUserName = (modelUser.f_passportName ?? DateTime.Now.Ticks.ToString()) + (sHZorRJZtype == "p" ? "_x" : sHZorRJZtype == "e" ? "_e" : "_c");
                    fs = new FileStream(sPath, FileMode.OpenOrCreate);
                    byte[] bytes = new byte[(int)fs.Length];
                    fs.Read(bytes, 0, bytes.Length);
                    fs.Close();
                    HttpContext.Current.Response.ContentType = "application/octet-stream";
                    //通知浏览器下载文件而不是打开
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;   filename=" + HttpUtility.UrlEncode(sUserName + sHouz, System.Text.Encoding.UTF8));
                    HttpContext.Current.Response.AddHeader("pragma", "no-cache");
                    HttpContext.Current.Response.AddHeader("Cache-Control", "no-store, must-revalidate");
                    HttpContext.Current.Response.BinaryWrite(bytes);
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    fs?.Dispose();
                }
            }
        }
        /// <summary>
        /// 检查文件后缀名是否符合要求
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static bool checkFileExtension(string sFileName, out string o_Extension)
        {
            //获取文件后缀
            o_Extension = System.IO.Path.GetExtension(sFileName).ToLower();
            if (o_Extension != null)
                o_Extension = o_Extension.ToLower();
            else
                return false;

            if (o_Extension != ".jpg" && o_Extension != ".jpeg" && o_Extension != ".png")
                return false;

            return true;
        }
    }
}
