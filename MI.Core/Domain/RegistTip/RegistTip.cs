using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Domain
{
    /// <summary>
    /// 登记查询提示表
    /// </summary>
    public class RegistTip
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public int f_Id { get; set; }
        /// <summary>
        /// 员工id
        /// </summary>
        public Nullable<int> f_eId { get; set; }
        /// <summary>
        /// 登记查询表名
        /// </summary>
        public string f_Table { get; set; }
        /// <summary>
        /// 登记查询表中文名
        /// </summary>
        public string f_TableName { get; set; }
        /// <summary>
        /// 提示状态，0不提示
        /// </summary>
        public Nullable<int> f_TipState { get; set; }
        /// <summary>
        /// 表的记录数
        /// </summary>
        public Nullable<int> f_RowsCount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string f_Remark { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public Nullable<System.DateTime> f_OperatorTime { get; set; }
    }
}
