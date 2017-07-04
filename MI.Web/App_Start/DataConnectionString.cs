using System.Web.Configuration;

namespace MI.Web
{
    public class DataConnectionString
    {
        /// <summary>
        /// 获取默认连接字符串
        /// </summary>
        public static string DefaultConnectionString
        {
            get {

                return WebConfigurationManager.ConnectionStrings["Default"].ToString();
            }
        }
        
    }
}
