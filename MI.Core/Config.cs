using System;
using System.Configuration;
using System.Web;

namespace MI.Core
{
    public class Config : IConfig
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["Default"].ToString();
            }
        }
        /// <summary>
        /// 临时文件夹
        /// </summary>
        public string TempFoldPath
        {
            get
            {
                return HttpContext.Current.Request.MapPath("~/Temp/Downloads");

            }
        }
    }
}
