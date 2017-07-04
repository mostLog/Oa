using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.ISType
{
    public enum sTypeEnum
    {
        /// <summary>
        /// 权限类型
        /// </summary>
        权限类型 = 0,
        班次类型 = 1,
        部门类型 = 2,
        上班状态类型 = 3,
        上班地点类型 = 4,
        /// <summary>
        /// 表示订餐的种类(中晚宵)
        /// </summary>
        订餐类型 = 5,
        订餐要求类型 = 6,
        /// <summary>
        /// 有些部门区分早中晚班次
        /// </summary>
        公司用餐统计部门班次类型 = 20,
        公司用餐时间类型 = 21,
        /// <summary>
        /// 房间类型
        /// </summary>
        房间类型 = 22,
        车辆类型 = 23,
        工作类别 = 24,
        国籍 = 25,
        /// <summary>
        /// 南航/菲航 等..
        /// </summary>
        航班类型 = 26,
        /// <summary>
        /// 社区类型
        /// </summary>
        社区类型 = 27,
        /// <summary>
        /// 楼栋类型
        /// </summary>
        楼栋类型 = 28,
        /// <summary>
        /// 签证类型
        /// </summary>
        签证类型 = 29,
        /// <summary>
        /// 证件类型
        /// </summary>
        证件类型 = 30,
        /// <summary>
        /// 办理进度
        /// </summary>
        办理进度 = 31,
        /// <summary>
        /// 床位类型
        /// </summary>
        床位类型 = 32,
        /// <summary>
        /// 银行储值类型
        /// </summary>
        银行储值类型 = 33,
        /// <summary>
        /// 考勤能见部门 存的部门ID
        /// </summary>
        考勤能见部门 = 34,
        /// <summary>
        /// 月均储值兑奖列
        /// </summary>
        月均儲值兌獎 = 35,
        /// <summary>
        ///金流客服等级类型 
        /// </summary>
        金流客服等级类型 = 36,
        /// <summary>
        /// 金流客服权限控制
        /// </summary>
        金流客服权限控制 = 37,
        /// <summary>
        /// 组别类型
        /// </summary>
        组别类型 = 39,
        /// <summary>
        /// 汇率类型
        /// </summary>
        汇率类型 = 40,
        /// <summary>
        /// 上车地点配置
        /// </summary>
        上车地点配置 = 51,
        /// <summary>
        /// 宿舍wifi配置
        /// </summary>
        宿舍wifi配置 = 52,
        /// <summary>
        /// 行政部联系方式配置
        /// </summary>
        行政部联系方式配置 = 53,
        /// <summary>
        /// 宿舍订餐领便当处 
        /// </summary>
        宿舍订餐领便当处 = 54,
        /// <summary>
        /// 宿舍饮水订购方式  
        /// </summary>
        宿舍饮水订购方式 = 55,
        /// <summary>
        /// 宿舍瓦斯订购方式
        /// </summary>
        宿舍瓦斯订购方式 = 56,
        /// <summary>
        /// 宿舍维修对接
        /// </summary>
        宿舍维修对接 = 57,
    }
}
