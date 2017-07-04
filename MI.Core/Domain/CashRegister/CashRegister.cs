using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Domain
{
    /// <summary>
    /// 现金登记
    /// </summary>
    public class CashRegister
    {
        /// <summary>
        /// 
        /// </summary>
        public int f_CId { get; set; }
        /// <summary>
        /// 据点  类型表
        /// </summary>
        public int f_workLocation_f_tid { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public Nullable<System.DateTime> f_Date { get; set; }
        /// <summary>
        /// 品项
        /// </summary>
        public string f_Items { get; set; }
        /// <summary>
        /// 收入
        /// </summary>
        public decimal f_Income { get; set; }
        /// <summary>
        /// 支出
        /// </summary>
        public decimal f_Expenses { get; set; }
        /// <summary>
        /// 经手人(操作人)
        /// </summary>
        public int f_Handled_f_Eid { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string f_Remark { get; set; }
        /// <summary>
        /// 剩余
        /// </summary>
        public decimal f_Balance { get; set; }
        /// <summary>
        /// 是否有收据
        /// </summary>
        public Nullable<bool> f_HasReceipt { get; set; }
    }
}
