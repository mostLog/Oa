using System.Text;

namespace System.Web.Mvc
{
    public static class PageHelper
    {
        /// <summary>
        /// 无条件分页查询
        /// </summary>
        public static HtmlString PageList(this HtmlHelper htmlHelper, int currentPage, int pageSize, int totalCount)
        {
            if (totalCount == 0) return new HtmlString(string.Empty);
            var redirectTo = htmlHelper.ViewContext.RequestContext.HttpContext.Request.Url.AbsolutePath;
            return new HtmlString(GetPageHtml(redirectTo, currentPage, pageSize, totalCount));
        }

        /// <summary>
        /// 单条件分页查询
        /// </summary>
        public static HtmlString PageList(this HtmlHelper htmlHelper, int currentPage, int pageSize, int totalCount, string keyWords)
        {
            if (totalCount == 0) return new HtmlString(string.Empty);
            var redirectTo = htmlHelper.ViewContext.RequestContext.HttpContext.Request.Url.AbsolutePath;
            var parameter = string.Empty;
            if (!string.IsNullOrEmpty(keyWords))
            {
                parameter = $"&keyWords={keyWords}";
            }
            return new HtmlString(GetPageHtml(redirectTo, currentPage, pageSize, totalCount, parameter));
        }
        /// <summary>
        /// 单条件分页查询
        /// 调用js的方式 不用链接
        /// </summary>
        /// <returns></returns>
        public static HtmlString PageList(this HtmlHelper htmlHelper, int currentPage, int pageSize, int totalCount, string keyWords, string JsFunctionName)
        {
            if (totalCount == 0) return new HtmlString(string.Empty);
            var redirectTo = htmlHelper.ViewContext.RequestContext.HttpContext.Request.Url.AbsolutePath;
            return new HtmlString(GetPageHtml(redirectTo, currentPage, pageSize, totalCount, keyWords, JsFunctionName));
        }
        /// <summary>
        /// 多条件分页查询
        /// </summary>
        public static HtmlString PageListFor<T>(this HtmlHelper htmlHelper, int currentPage, int pageSize, int totalCount, T t)
        {
            if (totalCount == 0) return new HtmlString(string.Empty);
            Type type = t.GetType();
            string link = string.Empty;
            var parameter = new StringBuilder();
            bool isChinese = false;            
            foreach (System.Reflection.PropertyInfo mi in type.GetProperties())
            {
                object value = mi.GetValue(t, null);
                if (value != null && value.ToString() != "")
                {
                    if (mi.PropertyType.Name == "Int32[]")
                    {
                        foreach (var item in (int[])value)
                        {
                            parameter.AppendFormat("&{0}={1}", mi.Name, item);
                        }
                        continue;
                    }
                    //中文判断及转码--兼容IE11.0.42浏览器出现中文乱码的问题
                    isChinese = false;         
                    foreach (var item in value.ToString())
                    {
                        if ((int)item > 127)
                        {
                            isChinese = true;
                            break;
                        }
                    }
                    if (isChinese)
                    {
                        parameter.AppendFormat("&{0}={1}", mi.Name,HttpUtility.UrlEncode(value.ToString()).ToUpper());
                    }
                    else
                    {
                        parameter.AppendFormat("&{0}={1}", mi.Name, value);
                    }                    
                }
            }
            var redirectTo = htmlHelper.ViewContext.RequestContext.HttpContext.Request.Url.AbsolutePath;
            return new HtmlString(GetPageHtml(redirectTo, currentPage, pageSize, totalCount, parameter.ToString()));
        }

        /// <summary>
        /// 无条件分页查询 生成HTML
        /// </summary>
        private static string GetPageHtml(string redirectTo, int currentPage, int pageSize, int totalCount)
        {
            pageSize = pageSize == 0 ? 3 : pageSize;
            var totalPages = Math.Max((totalCount + pageSize - 1) / pageSize, 1); //总页数
            var output = new StringBuilder();
            string link = string.Empty;
            string link2 = string.Empty;
            string link3 = string.Empty;
            link = "<li><a href='{0}?iPageIndex={1}&iPageSize={2}'>{3}</a></li>";
            link2 = " <li class='active'><a href='#'>{0} <span class='sr-only'>(current)</span></a></li>";
            link3 = "<li {0}><span>{1}</span></li>";
            if (totalPages > 1)
            {
                output.AppendFormat(link, redirectTo, 1, pageSize, "首页");
                output.Append(" ");
                if (currentPage > 0)
                {//处理上一页的连接
                    if (currentPage - 1 == 0)
                    {
                        output.AppendFormat(link3, " class='disabled' ", "<span aria-hidden='true'>&laquo;</span>");
                    }
                    else
                        output.AppendFormat(link, redirectTo, currentPage - 1, pageSize, "<span aria-hidden='true'>&laquo;</span>");
                }
                output.Append(" ");
                int currint = 5;
                for (int i = 0; i <= 10; i++)
                {//一共最多显示10个页码，前面5个，后面5个
                    if ((currentPage + i - currint) >= 1 && (currentPage + i - currint) <= totalPages)
                    {
                        if (currint == i)
                        {//当前页处理                            
                            output.AppendFormat(link2, currentPage);
                        }
                        else
                        {//一般页处理
                            output.AppendFormat(link, redirectTo, currentPage + i - currint, pageSize, currentPage + i - currint);
                        }
                    }
                    output.Append(" ");
                }
                if (currentPage <= totalPages)
                {//处理下一页的链接
                    if (currentPage == totalPages)
                    {
                        output.AppendFormat(link3, " class='disabled' ", "<span aria-hidden='true'>&raquo;</span>");
                    }
                    else
                        output.AppendFormat(link, redirectTo, currentPage + 1, pageSize, "<span aria-hidden='true'>&raquo;</span>");
                }
                output.Append(" ");
                if (currentPage != totalPages)
                {
                    output.AppendFormat(link, redirectTo, totalPages, pageSize,  "尾页");
                }
                output.Append(" ");
            }

            return output.ToString();
        }

        /// <summary>
        /// 生成HTML
        /// </summary>
        private static string GetPageHtml(string redirectTo, int currentPage, int pageSize, int totalCount, string keyWords)
        {
            pageSize = pageSize == 0 ? 3 : pageSize;
            var totalPages = Math.Max((totalCount + pageSize - 1) / pageSize, 1); //总页数
            var output = new StringBuilder();
            string link = string.Empty;
            string link2 = string.Empty;
            string link3 = string.Empty;
            link = "<li><a href='{0}?iPageIndex={1}&iPageSize={2}{3}'>{4}</a></li>";
            link2 = " <li class='active'><a href='#'>{0} <span class='sr-only'>(current)</span></a></li>";
            link3 = "<li {0}><span>{1}</span></li>";
            if (totalPages > 1)
            {
                output.AppendFormat(link, redirectTo, 1, pageSize, keyWords, "首页");
                output.Append(" ");
                if (currentPage > 0)
                {//处理上一页的连接
                    if (currentPage - 1 == 0)
                    {
                        output.AppendFormat(link3, " class='disabled' ", "<span aria-hidden='true'>&laquo;</span>");
                    }
                    else
                        output.AppendFormat(link, redirectTo, currentPage - 1, pageSize, keyWords, "<span aria-hidden='true'>&laquo;</span>");
                }
                output.Append(" ");
                int currint = 5;
                for (int i = 0; i <= 10; i++)
                {//一共最多显示10个页码，前面5个，后面5个
                    if ((currentPage + i - currint) >= 1 && (currentPage + i - currint) <= totalPages)
                    {
                        if (currint == i)
                        {//当前页处理                            
                            output.AppendFormat(link2, currentPage);
                        }
                        else
                        {//一般页处理
                            output.AppendFormat(link, redirectTo, currentPage + i - currint, pageSize, keyWords, currentPage + i - currint);
                        }
                    }
                    output.Append(" ");
                }
                if (currentPage <= totalPages)
                {//处理下一页的链接
                    if (currentPage == totalPages)
                    {
                        output.AppendFormat(link3, " class='disabled' ", "<span aria-hidden='true'>&raquo;</span>");
                    }
                    else
                        output.AppendFormat(link, redirectTo, currentPage + 1, pageSize, keyWords, "<span aria-hidden='true'>&raquo;</span>");
                }
                output.Append(" ");
                if (currentPage != totalPages)
                {
                    output.AppendFormat(link, redirectTo, totalPages, pageSize, keyWords, "尾页");
                }
                output.Append(" ");
            }

            return output.ToString();
        }


        /// <summary>
        /// 生成HTML 调用js方法 不用链接的方式
        /// </summary>
        private static string GetPageHtml(string redirectTo, int currentPage, int pageSize, int totalCount, string keyWords, string JsFunctionName)
        {
            pageSize = pageSize == 0 ? 3 : pageSize;
            var totalPages = Math.Max((totalCount + pageSize - 1) / pageSize, 1); //总页数
            var output = new StringBuilder();
            const string link = "<li><a href='javascript:{5}(\"{0}\",{1},{2},\"{3}\")'>{4}</a></li>";
            const string link2 = " <li class='active'><a href='javascript:#'>{0} <span class='sr-only'>(current)</span></a></li>";
            const string link3 = "<li {0}><span>{1}</span></li>";
            if (totalPages > 1)
            {
                output.AppendFormat(link, redirectTo, 1, pageSize, keyWords, "首页", JsFunctionName);
                output.Append(" ");
                if (currentPage > 0)
                {//处理上一页的连接
                    if (currentPage - 1 == 0)
                    {
                        output.AppendFormat(link3, " class='disabled' ", "<span aria-hidden='true'>&laquo;</span>");
                    }
                    else
                        output.AppendFormat(link, redirectTo, currentPage - 1, pageSize, keyWords, "<span aria-hidden='true'>&laquo;</span>",JsFunctionName);
                }
                output.Append(" ");
                int currint = 5;
                for (int i = 0; i <= 10; i++)
                {//一共最多显示10个页码，前面5个，后面5个
                    if ((currentPage + i - currint) >= 1 && (currentPage + i - currint) <= totalPages)
                    {
                        if (currint == i)
                        {//当前页处理                            
                            output.AppendFormat(link2, currentPage);
                        }
                        else
                        {//一般页处理
                            output.AppendFormat(link, redirectTo, currentPage + i - currint, pageSize, keyWords, currentPage + i - currint, JsFunctionName);
                        }
                    }
                    output.Append(" ");
                }
                if (currentPage <= totalPages)
                {//处理下一页的链接
                    if (currentPage == totalPages)
                    {
                        output.AppendFormat(link3, " class='disabled' ", "<span aria-hidden='true'>&raquo;</span>");
                    }
                    else
                        output.AppendFormat(link, redirectTo, currentPage + 1, pageSize, keyWords, "<span aria-hidden='true'>&raquo;</span>", JsFunctionName);
                }
                output.Append(" ");
                if (currentPage != totalPages)
                {
                    output.AppendFormat(link, redirectTo, totalPages, pageSize, keyWords, "尾页", JsFunctionName);
                }
                output.Append(" ");
            }

            return output.ToString();
        }
    }
}