using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto
{
    /// <summary>
    /// 班车搭车统计
    /// </summary>
    public class TrafficStatisticsDto
    {
        /// <summary>
        /// 搭车统计viewmodel
        /// </summary>
        /// <param name="srtype">上班1or下班2</param>
        /// <param name="ridetime">搭车时间</param>
        /// <param name="count">合计人数</param>
        /// <param name="CommunityLength">社区的长度</param>
        /// <param name="WorkLocationLength">工作地点的长度</param>
        public TrafficStatisticsDto(int srtype, DateTime? ridetime, int count, int CommunityLength, int WorkLocationLength)
        {
            Community = new int[CommunityLength];
            WorkLocation = new int[WorkLocationLength];
            srType = srtype;
            RideTime = ridetime;
            Count = count;
        }
        /// <summary>
        /// 上班1or下班2
        /// </summary>
        public int srType { get; set; }
        /// <summary>
        /// 搭车时间
        /// </summary>
        public DateTime? RideTime { get; set; }
        /// <summary>
        /// 社区人数集合
        /// </summary>
        public int[] Community { get; set; }
        /// <summary>
        /// 工作地点人数集合
        /// </summary>
        public int[] WorkLocation { get; set; }
        /// <summary>
        /// 合计人数
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 是否包括 返乡的数据  
        /// </summary>
        public bool isComprising { get; set; }
    }
}
