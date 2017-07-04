using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Domain
{
    public class OrderingEmployeeCollectHistory
    {
        /// <summary>
        /// 
        /// </summary>
        public int f_id { get; set; }
        /// <summary>
        /// 社区
        /// </summary>
        public string f_Community { get; set; }
        /// <summary>
        /// 楼栋
        /// </summary>
        public string f_Building { get; set; }
        /// <summary>
        /// 房间号
        /// </summary>
        public string f_RoomNo { get; set; }
        /// <summary>
        /// 中餐(普通)
        /// </summary>
        public Nullable<int> f_LuchRegular { get; set; }
        /// <summary>
        /// 中餐(塑料袋)
        /// </summary>
        public Nullable<int> f_LuchOnlyRicePlasticBag { get; set; }
        /// <summary>
        /// 中餐(盒装)
        /// </summary>
        public Nullable<int> f_LuchOnlyRiceBox { get; set; }
        /// <summary>
        /// 中餐(不要肉)
        /// </summary>
        public Nullable<int> f_LuchNoMeat { get; set; }
        /// <summary>
        /// 晚餐(普通)
        /// </summary>
        public Nullable<int> f_DinnerRegular { get; set; }
        /// <summary>
        /// 晚餐(塑料袋)
        /// </summary>
        public Nullable<int> f_DinnerOnlyRicePlasticBag { get; set; }
        /// <summary>
        /// 晚餐(盒装)
        /// </summary>
        public Nullable<int> f_DinnerOnlyRiceBox { get; set; }
        /// <summary>
        /// 晚餐(不要肉)
        /// </summary>
        public Nullable<int> f_DinnerNoMeat { get; set; }
        /// <summary>
        /// 夜宵(普通)
        /// </summary>
        public Nullable<int> f_MidnightRegular { get; set; }
        /// <summary>
        /// 宵夜(塑料袋)
        /// </summary>
        public Nullable<int> f_MidnightOnlyRicePlasticBag { get; set; }
        /// <summary>
        /// 宵夜(盒装)
        /// </summary>
        public Nullable<int> f_MidnightOnlyRiceBox { get; set; }
        /// <summary>
        /// 宵夜(不要肉)
        /// </summary>
        public Nullable<int> f_MidnightNoMeat { get; set; }
        /// <summary>
        /// 
        /// </summary日期>
        public Nullable<System.DateTime> f_Date { get; set; }
    }
}
