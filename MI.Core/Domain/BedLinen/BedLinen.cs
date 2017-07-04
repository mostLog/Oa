using System;

namespace MI.Core.Domain
{
    public class BedLinen
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public int f_Id { get; set; }
        /// <summary>
        /// 员工id（登记人）
        /// </summary>
        public int f_eid { get; set; }
        /// <summary>
        /// 登记日期
        /// </summary>
        public DateTime f_RegistrationDate { get; set; }
        /// <summary>
        /// 床位类型（1=双人床，2单人床，3=小床，4=大床）
        /// </summary>
        public int f_tID { get; set; }
        /// <summary>
        /// 床单数量
        /// </summary>
        public int? f_BunkNo { get; set; }
        /// <summary>
        /// 被单数量    
        /// </summary>
        public int? f_SheetsNo { get; set; }
        /// <summary>
        /// 被子数量
        /// </summary>
        public int? f_QuiltNo { get; set; }
        /// <summary>
        /// 枕套数量
        /// </summary>
        public int? f_PillowcaseNo { get; set; }
        /// <summary>
        /// 当月送洗次数
        /// </summary>
        public int? f_Cont { get; set; }
        /// <summary>
        /// 加送床单数量
        /// </summary>
        public int? f_AddBunkNo { get; set; }
        /// <summary>
        /// 加送被单数量
        /// </summary>
        public int? f_AddSheetsNo { get; set; }
        /// <summary>
        /// 加送被子数量
        /// </summary>
        public int? f_AddQuiltNo { get; set; }
        /// <summary>
        /// 加送枕套数量
        /// </summary>
        public int? f_AddPillowcaseNo { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string f_Remarks { get; set; }
    }
}
