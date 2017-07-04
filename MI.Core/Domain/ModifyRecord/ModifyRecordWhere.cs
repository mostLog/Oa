using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Domain
{
    public class ModifyRecordWhere
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime f_ChangeTime { get; set; }

        public DateTime f_EndChangeTime { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public int f_ActionStatus { get; set; }
        /// <summary>
        /// 操作项目
        /// </summary>
        public int f_ItemCategory { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string f_Content { get; set; }
        public bool isValid
        {
            get
            {
                return (f_ChangeTime != null || f_EndChangeTime != null || f_ActionStatus != 0 || f_ItemCategory != 0 || !string.IsNullOrWhiteSpace(f_Content));
            }
        }
    }
}
