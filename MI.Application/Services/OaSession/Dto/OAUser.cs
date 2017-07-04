using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.OASession.Dto
{
    [Serializable]
    public class OAUser
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 护照英文名
        /// </summary>
        public string PassportName { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 菜单权限集合
        /// </summary>
        public IList<string> Permissions { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IsLogin { get; set; }
    }
}
