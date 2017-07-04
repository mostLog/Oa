using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto
{
    public class TariffListDto
    {
        public int Id { get; set; }
        /// <summary>
        /// 社区
        /// </summary>
        public string Community { get; set; }
        /// <summary>
        /// 楼栋
        /// </summary>
        public string Building { get; set; }
        /// <summary>
        /// 房间号
        /// </summary>
        public string RoomNo { get; set; }
        /// <summary>
        /// 电费标准
        /// </summary>
        public decimal TariffStandard { get; set; }
        /// <summary>
        /// 实际账单
        /// </summary>
        public decimal RealBill { get; set; }
        /// <summary>
        /// 超支金额
        /// </summary>
        public decimal Overruns { get; set; }
        /// <summary>
        /// 登记日期
        /// </summary>

        public DateTime? TariffDate { get; set; }
        /// <summary>
        /// 登记人
        /// </summary>
        public string Registrant { get; set; }
        /// <summary>
        /// 是否缴费
        /// </summary>
        public bool IsPayment { get; set; }
        /// <summary>
        /// 收费人
        /// </summary>
        public string Rate { get; set; }
        /// <summary>
        /// 收款地点
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string Operator{get;set;}
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime? OperatorTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
