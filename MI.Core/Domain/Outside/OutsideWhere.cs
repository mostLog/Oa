using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Domain
{
    /// <summary>
    /// 员工外租查询条件
    /// SHAWN 2016.6.28
    /// </summary>
    public class OutsideWhere
    {
        /// <summary>
        /// 名字(中文名字or 护照名字or 昵称)
        /// </summary>
        public string Name { get; set; }

        public bool isValid => !string.IsNullOrWhiteSpace(Name);
    }
}
