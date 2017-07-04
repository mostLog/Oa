using System;

namespace MI.Application.Dto
{
    /// <summary>
    /// 查询条件
    /// 
    /// 小白-2017-6-12
    /// </summary>
    public class EmpRentPagedInputDto:PagedInputDto
    {
        /// <summary>
        /// 员工
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 楼栋
        /// </summary>
        public string Building { get; set; }
        /// <summary>
        /// 房间号
        /// </summary>
        public string RoomNo { get; set; }
        /// <summary>
        /// 缴费开始日期
        /// </summary>
        public DateTime? PaymentStartDate { get; set; }
        /// <summary>
        /// 缴费结束日期
        /// </summary>
        public DateTime? PaymentEndDate { get; set; }
        /// <summary>
        /// 是否缴费
        /// </summary>
        public int IsPayment { get; set; } = 2;
    }
}
