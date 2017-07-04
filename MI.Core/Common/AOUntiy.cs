
using MI.Core.Common;
using MI.Core.Domain;
using SB.Common;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


public class AOUnity
{
    /// <summary>
    /// 写入文本
    /// </summary>
    /// <param name="path">写入路径</param>
    /// <param name="content">写入内容</param>
    public static void WriteLog(string path, string content)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        path = Path.Combine(path, DateTime.Now.ToString("yyyy-MM-dd-HH-mm") + ".txt");
        FileStream fs = new FileStream(path, FileMode.Append);
        StreamWriter sw = new StreamWriter(fs);
        //开始写入
        sw.Write(content);
        //清空缓冲区
        sw.Flush();
        //关闭流
        sw.Close();
        fs.Close();
    }
    /// <summary>
    /// 快速记录错误日记
    /// </summary>
    /// <param name="e">错误</param>
    public static void WriteLog(Exception ex)
    {
        string sPath = System.Web.HttpContext.Current.Server.MapPath(GetErrorLogPath);
        string sErrorUrl = System.Web.HttpContext.Current.Request.Url.ToString();
        if (!Directory.Exists(sPath))
        {
            Directory.CreateDirectory(sPath);
        }
        sPath = Path.Combine(sPath, DateTime.Now.ToString("yyyy-MM-dd-HH-mm") + ".txt");
        FileStream fs = new FileStream(sPath, FileMode.Append);
        StreamWriter sw = new StreamWriter(fs);
        StringBuilder str = new StringBuilder();
        str.Append("\r\n" + DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
        str.Append("\r\n\tURL：" + sErrorUrl);
        str.Append("\r\n\t錯誤信息：" + ex.Message + "\r\n\t\t ***" + ex?.InnerException?.Message + "\r\n\t\t **" + ex?.InnerException?.InnerException?.Message);
        str.Append("\r\n\t錯誤源：" + ex.Source);
        str.Append("\r\n\t異常方法：" + ex.TargetSite);
        str.Append("\r\n\t堆棧信息：" + ex.StackTrace);
        str.Append(
            "\r\n--------------------------------------------------------------------------------------------------");
        //开始写入
        sw.Write(str.ToString());
        //清空缓冲区
        sw.Flush();
        //关闭流
        sw.Close();
        fs.Close();
    }

    /// <summary>
    /// 根据key获取配置
    /// </summary>
    /// <param name="sStr"></param>
    /// <returns></returns>
    public static string GetAppSettings(string sKey)
    {
        return System.Web.Configuration.WebConfigurationManager.AppSettings[sKey];
    }

    /// <summary>
    /// 获取错误日记的配置路径
    /// </summary>
    public static string GetErrorLogPath
    {
        get
        {
            return System.Web.Configuration.WebConfigurationManager.AppSettings["errorLogPath"];
        }
    }

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
    /// 获取操作类型文本
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public static string GetActionStatus(int i)
    {
        var ActionStatus = "";
        switch (i)
        {
            case 1:
                ActionStatus = "新增";
                break;
            case 2:
                ActionStatus = "删除";
                break;
            case 3:
                ActionStatus = "修改";
                break;
            default:
                ActionStatus = "请选择";
                break;
        }
        return ActionStatus;
    }
    /// <summary>
    /// 获取操作项目文本
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public static string GetCategoryItem(int i)
    {
        var CategoryItem = "";
        switch (i)
        {
            case 1:
                CategoryItem = "车辆管理";
                break;
            case 2:
                CategoryItem = "宿舍管理";
                break;
            case 3:
                CategoryItem = "餐饮管理";
                break;
            case 4:
                CategoryItem = "工作交接";
                break;
            case 5:
                CategoryItem = "员工管理";
                break;
            case 6:
                CategoryItem = "个人资料";
                break;
            case 7:
                CategoryItem = "公告管理";
                break;
            case 8:
                CategoryItem = "类别管理";
                break;
            case 9:
                CategoryItem = "新人登记";
                break;
            case 10:
                CategoryItem = "机票登记";
                break;
            case 11:
                CategoryItem = "现金登记";
                break;
            case 12:
                CategoryItem = "派车安排";
                break;
            case 13:
                CategoryItem = "费用管理";
                break;
            default:
                CategoryItem = "请选择";
                break;

        }
        return CategoryItem;
    }
    /// <summary>
    /// 获取完整客户端Ip
    /// </summary>
    /// <returns></returns>
    public static string GetClientIp()
    {
        string ip = null;
        System.Web.HttpRequest request = System.Web.HttpContext.Current.Request;
        string forward = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (String.IsNullOrEmpty(forward))
        {
            ip = request.ServerVariables["REMOTE_ADDR"];
        }
        else if (forward.IndexOf(",") > 0)
        {
            ip = forward.Substring(1, forward.IndexOf(",") - 1);
        }
        else if (forward.IndexOf(";") > 0)
        {
            ip = forward.Substring(1, forward.IndexOf(";") - 1);
        }
        else
        {
            ip = forward;
        }

        return ip.Replace(" ", String.Empty);
    }
    /// <summary>
    /// 获取部门简称缩写
    /// </summary>
    /// <param name="id">部门id</param>
    /// <returns></returns>
    public static string GetShorthand(int id)
    {
        string sShorthand = string.Empty;
        switch (id)
        {
            case 9:
                sShorthand = "XZ";
                break;
            case 10:
                sShorthand = "QC";
                break;
            case 11:
                sShorthand = "ZQ";
                break;
            case 12:
                sShorthand = "ZH";
                break;
            case 13:
                sShorthand = "JL";//
                break;
            case 14:
                sShorthand = "KF";
                break;
            case 15:
                sShorthand = "KC";
                break;
            case 16:
                sShorthand = "RM";
                break;
            case 17:
                sShorthand = "RD";
                break;
            case 18:
                sShorthand = "YJ";
                break;
            case 19:
                sShorthand = "SX";
                break;
            case 20:
                sShorthand = "HG";
                break;
            default:
                sShorthand = "QT";
                break;
        }
        return sShorthand;
    }
    /// <summary>
    /// 数字换成字母
    /// </summary>
    /// <param name="number">数字(只能是1~26的范围)</param>
    /// <returns></returns>
    public static string NunberToChar(int number)
    {
        if (1 <= number && 26 >= number)
        {
            int num = number + 64;
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            byte[] btNumber = new byte[] { (byte)num };
            return asciiEncoding.GetString(btNumber).ToUpper();
        }
        return number.ToString();
    }

    /// <summary>
    /// 字母换成数字
    /// </summary>
    /// <param name="sChar">字母(单个)</param>
    /// <returns></returns>
    public static int CharToNunber(string sChar)
    {
        if (sChar.Length == 1)
        {
            byte[] array = System.Text.Encoding.ASCII.GetBytes(sChar);
            int asciicode = (short)(array[0]);
            return asciicode - 64;
        }
        return 0;
    }

    /// <summary>
    /// 获取字符串中的字母，大小写，汉字,数字
    /// </summary>
    /// <param name="sChar">字符串</param>
    /// <returns></returns>
    public static string GetStringByLetters(string sChar)
    {
        Regex re = new Regex(@"[a-zA-Z]|[0-9\u4E00-\u9FA5]+");
        MatchCollection mc = re.Matches(sChar);
        return mc.Cast<Match>().Aggregate(String.Empty, (current, m) => current + m.Value);
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
    /// <summary>
    /// 获取人事表字段的说明文字
    /// </summary>
    /// <param name="sField"></param>
    /// <returns></returns>
    public static string GetEmpFieldSibling(string sField)
    {
        string sSibling = string.Empty;
        if (!string.IsNullOrWhiteSpace(sField))
        {
            switch (sField)
            {
                case "f_eid":
                    sSibling = "员工ID";
                    break;
                case "f_periodType_tID":
                    sSibling = "班次";
                    break;
                case "f_department_tID":
                    sSibling = "部门";
                    break;
                case "f_chineseName":
                    sSibling = "中文名";
                    break;
                case "f_sex":
                    sSibling = "性别";
                    break;
                case "f_passportID":
                    sSibling = "护照ID";
                    break;
                case "f_passportName":
                    sSibling = "护照名英文/越南文";
                    break;
                case "f_PassportURL":
                    sSibling = "存储护照照片URL";
                    break;
                case "f_nickname":
                    sSibling = "昵称";
                    break;
                case "f_international":
                    sSibling = "国籍";
                    break;
                case "f_QQNumber":
                    sSibling = "QQ号码";
                    break;
                case "f_TelNoPH":
                    sSibling = "菲律宾联系电话";
                    break;
                case "f_TelNoCN":
                    sSibling = "国内电话";
                    break;
                case "f_emergencyContactPerson":
                    sSibling = "紧急联系人";
                    break;
                case "f_emergencyContactNumber":
                    sSibling = "紧急联系人电话";
                    break;
                case "f_Wechat":
                    sSibling = "微信";
                    break;
                case "f_workStatus_tID":
                    sSibling = "上班状态";
                    break;
                case "f_rideWorkTime":
                    sSibling = "上班搭车时间";
                    break;
                case "f_rideOffWorkTime":
                    sSibling = "下班搭车时间";
                    break;
                case "f_workLocation_tID":
                    sSibling = "工作地点";
                    break;
                case "f_dormitoryId":
                    sSibling = "宿舍表ID";
                    break;
                case "f_Remark":
                    sSibling = "备注";
                    break;
                case "f_pwd":
                    sSibling = "密码";
                    break;
                case "f_level_tID":
                    sSibling = "等级权限";
                    break;
                case "f_nid":
                    sSibling = "新人表中的id";
                    break;
                case "f_IsPHStaff":
                    sSibling = "是否为菲律宾员工";
                    break;
                case "f_Salary":
                    sSibling = "薪资";
                    break;
                case "f_AccountName":
                    sSibling = "OA帐号";
                    break;
                case "f_CreateDate":
                    sSibling = "创建时间";
                    break;
                case "f_VisaType_tID":
                    sSibling = "签证类型";
                    break;
                case "f_EntrystampURL":
                    sSibling = "存储入境章照片URL";
                    break;
                case "f_passportDate":
                    sSibling = "签证到期时间";
                    break;
                case "f_HireDate":
                    sSibling = "入职日期";
                    break;
                case "f_DateOfBirth":
                    sSibling = "出生日期";
                    break;
                case "f_IsRemove":
                    sSibling = "是否已移走";
                    break;
                case "f_IsNewEmp":
                    sSibling = "是否为新员工";
                    break;
                case "f_ICardDate":
                    sSibling = "I卡或者I卡证明到期时间";
                    break;
                case "f_SeparationDate":
                    sSibling = "离职日期";
                    break;
                case "f_OldReservation":
                    sSibling = "旧的订餐信息";
                    break;
                case "f_JLlevel_tID":
                    sSibling = "金流客服权限";
                    break;
                case "f_JLStage":
                    sSibling = "金流客服等级";
                    break;
                case "f_JLgroup":
                    sSibling = "组别";
                    break;
                case "f_JLdirector_eId":
                    sSibling = "直属主管";
                    break;
                //新人表的字段
                case "f_flightTypt_tID":
                    sSibling = "航班类型";
                    break;
                case "f_flightDate":
                    sSibling = "航班起飞时间";
                    break;
                case "f_flightArrivalTime":
                    sSibling = "航班抵达时间";
                    break;
                case "f_LoanAmount":
                    sSibling = "借款金额";
                    break;
                case "f_GiftCard":
                    sSibling = "储值卡";
                    break;
                case "f_TelCatd":
                    sSibling = "电话卡";
                    break;
                case "f_signature":
                    sSibling = "是否确认";
                    break;
                case "f_newRemark":
                    sSibling = "新人备注";
                    break;
                case "f_IDCardURL":
                    sSibling = "存储身份证照片URL";
                    break;
                case "f_mutilpleGroup":
                    sSibling = "组长组别";
                    break;
                default:
                    sSibling = "";
                    break;
            }
        }
        return sSibling;
    }

    /// <summary>
    /// 獲取金流客服權限字符串(publicConstant中寫死)
    /// </summary>
    /// <param name="iStage">級別備註</param>
    /// <param name="sDepartmentItem">部門</param>
    /// <returns></returns>
    public static string GetLevelRemarksByJL(int iStage, string sDepartmentItem, string sAccountName = "")
    {
        if (sAccountName == PublicConstant.HIGHEST_ACCOUNTNAME)
        {
            //最高權限
            return PublicConstant.HIGHEST_REMARKS;
        }
        switch (sDepartmentItem?.Trim())
        {
            case "金流":
                if (iStage == PublicConstant.MANAGER)
                {
                    return PublicConstant.JL_MANAGER_REMARKS;
                }
                else if (iStage == PublicConstant.DIRECTOR)
                {
                    return PublicConstant.JL_DIRECTOR_REMARKS;
                }
                else if (iStage == PublicConstant.LEADER || iStage == PublicConstant.VICELEADER)
                {
                    return PublicConstant.JL_LEADER_REMARKS;
                }
                else
                {
                    return PublicConstant.JL_EMPLOYEE_REMARKS;
                }

            case "客服":
                if (iStage == PublicConstant.MANAGER)
                {
                    return PublicConstant.KF_MANAGER_REMARKS;
                }
                else if (iStage == PublicConstant.DIRECTOR || iStage == PublicConstant.VICEDIRECTOR)
                {
                    return PublicConstant.KF_DIRECTOR_REMARKS;
                }
                else if (iStage == PublicConstant.LEADER || iStage == PublicConstant.VICELEADER)
                {
                    return PublicConstant.KF_LEADER_REMARKS;
                }
                else
                {
                    return PublicConstant.KF_EMPLOYEE_REMARKS;
                }
            case "行政":
                return PublicConstant.XZ_EMPLOYEE_REMARKS;
            default:
                return "";
        }
    }


    /// <summary>
    /// 删除密码
    /// </summary>
    public static string GetDelPwd => System.Web.Configuration.WebConfigurationManager.AppSettings["_DelPwd"] == null ? "walsg" : System.Web.Configuration.WebConfigurationManager.AppSettings["_DelPwd"].Trim();

    /// <summary>
    /// 判断分页条件pageIndex是否超出总页码
    /// </summary>
    /// <param name="sCount">数据总行数</param>
    /// <param name="PageIndex">页码</param>
    /// <param name="PageSize">每页多少条数据</param>
    public static void ValidatePagingWhere(int sCount, ref int r_PageIndex, double PageSize)
    {
        if (sCount > 0 && Math.Ceiling(sCount / PageSize) < r_PageIndex)
        {
            r_PageIndex = 1;
        }
    }
    /// <summary>
    /// 添加一条操作记录数据
    /// </summary>
    /// <param name="oldEntity">旧数据</param>
    /// <param name="newEntity">新数据</param>
    /// <param name="action">操作类型（修改，新增，删除）</param>
    /// <param name="category">修改项目</param>
    /// <param name="Name">操作者</param>
    /// <param name="tableName">操作的表名</param>
    /// <returns></returns>
    public static void AddModifyRecord(string oldEntity, string newEntity, ActionItem action, CategoryItem category, string tableName, string userName)
    {
        t_ModifyRecord record = new t_ModifyRecord
        {
            f_ActionStatus = (int)action,
            f_ItemCategory = (int)category,
            f_ByIP = AOUnity.GetClientIp(),
            f_ByWho = userName,
            f_NewData = newEntity,
            f_OldData = oldEntity,
            f_ChangeTime = DateTime.Now,
            f_TableName = tableName
        };
    }
}
