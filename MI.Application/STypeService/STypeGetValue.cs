using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application
{
    public class STypeGetValue
    {
        /// <summary>
        /// 获取大类型的文本值
        /// </summary>
        /// <param name="iType">大类型</param>
        /// <returns></returns>
        public static string GetValueByType(string iType)
        {
            string values = "";
            if (!string.IsNullOrWhiteSpace(iType))
            {
                switch (iType)
                {
                    case "0":
                        values = "权限类型";
                        break;
                    case "1":
                        values = "班次类型";
                        break;
                    case "2":
                        values = "部门类型";
                        break;
                    case "3":
                        values = "在职状态类型";
                        break;
                    case "4":
                        values = "上班地点类型";
                        break;
                    case "5":
                        values = "订餐类型";
                        break;
                    case "6":
                        values = "订餐要求类型";
                        break;
                    case "20":
                        values = "公司用餐统计部门班次类型";
                        break;
                    case "21":
                        values = "公司用餐时间类型";
                        break;
                    case "22":
                        values = "房间类型";
                        break;
                    case "23":
                        values = "车辆类型";
                        break;
                    case "24":
                        values = "工作类别";
                        break;
                    case "25":
                        values = "国籍";
                        break;
                    case "26":
                        values = "航班类型";
                        break;
                    case "29":
                        values = "签证类型";
                        break;
                    case "27":
                        values = "社区类型";
                        break;
                    case "30":
                        values = "证件类型";
                        break;
                    case "31":
                        values = "办理进度";
                        break;
                    case "32":
                        values = "床位类型";
                        break;
                    case "33":
                        values = "银行储值类型";
                        break;
                    case "35":
                        values = "月均儲值兌獎";
                        break;
                    case "36":
                        values = "金流客服等级类型";
                        break;
                    case "37":
                        values = "金流客服权限控制";
                        break;
                    case "51":
                        values = "上车地点配置";
                        break;
                    case "52":
                        values = "宿舍wifi配置";
                        break;
                    case "53":
                        values = "行政部联系方式配置";
                        break;
                    case "54":
                        values = "宿舍订餐领便当处";
                        break;
                    case "55":
                        values = "宿舍饮水订购方式";
                        break;
                    case "56":
                        values = "宿舍瓦斯订购方式";
                        break;
                    case "57":
                        values = "宿舍维修对接";
                        break;
                }
            }
            return values;
        }
        /// <summary>
        /// 验证字符串长度是否超过指定长度
        /// mu
        /// 2016.9.5
        /// </summary>
        /// <param name="sItemName">指定项目</param>
        /// <param name="sChar">需要验证的字符串</param>
        /// <param name="iLength">指定长度</param>
        /// <param name="sReturnChar">前台弹出信息</param>
        /// <returns></returns>
        public static bool ValidationLength(string sItemName, string sChar, int iLength, out string sReturnChar)
        {
            bool bIsTrue = false;
            if (!string.IsNullOrWhiteSpace(sChar))
            {
                bIsTrue = sChar.Length > iLength;
            }
            if (bIsTrue)
            {
                sReturnChar = sItemName + "    不能超过" + iLength + "" + "个字";
            }
            else
            {
                sReturnChar = "";
            }
            return bIsTrue;
        }
    }
}
