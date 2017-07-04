using MI.Application.OASession.Dto;
using System.Web;


namespace MI.Application.OASession
{
    /// <summary>
    /// 登陆用户信息
    /// </summary>
    public class Session : ISession
    {
        public OAUser GetCurrUser()
        {
            var currUser = HttpContext.Current.Session["CurrUser"];
            if (currUser != null)
            {
                return currUser as OAUser;
            }
            else
            {
                return null;
            }
        }
    }
}