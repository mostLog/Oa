using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Common
{
    public enum eTipsEnum
    {

        /// <summary>
        /// 操作失败
        /// </summary>
        [Description("操作失败!")]
        FailOperation = 0,
        /// <summary>
        /// 操作成功
        /// </summary>
        [Description("操作成功!")]
        SuccessfulOperation = 200,
        /// <summary>
        /// 系统错误
        /// </summary>
        [Description("sorry!系统发生错误,请联系工程师处理!")]
        Error = 500,
        /// <summary>
        /// 只能修改自己部门的员工信息
        /// </summary>
        [Description("sorry!您的权限只能修改自己部门的员工信息")]
        InsufficientPrivileges = 502,
        /// <summary>
        /// 需要选择部门
        /// </summary>
        [Description("您好,需要选择部门!")]
        DepartmentIsNullOrEmpty = 1,
        /// <summary>
        /// 需要填写中文名称
        /// </summary>
        [Description("您好,需要填写中文名称!")]
        ChineseNameIsNullOrEmpty = 2,
        /// <summary>
        /// 需要填写护照名
        /// </summary>
        [Description("您好,需要填写护照名!")]
        PassportNameIsNullOrEmpty = 3,
        /// <summary>
        /// 需要填写国籍
        /// </summary>
        [Description("您好,需要填写国籍!")]
        InternationalIsNullOrEmpty = 4,
        /// <summary>
        /// 需要选择上班状态
        /// </summary>
        [Description("您好,需要选择上班状态!")]
        WorkStatusIsNullOrEmpty = 5,
        /// <summary>
        /// 需要选择权限等级
        /// </summary>
        [Description("您好,需要选择权限等级!")]
        LevelIsNullOrEmpty = 6,
        /// <summary>
        /// 需要选择航班日期
        /// </summary>
        [Description("您好,需要选择航空日期!")]
        FlightDateIsNullOrEmpty = 7,
        /// <summary>
        /// 需要选择航班到达时间
        /// </summary>
        [Description("您好,需要选择航班到达时间!")]
        FlightArrivalTimeIsNullOrEmpty = 8,
        /// <summary>
        /// 需要填写培训主管
        /// </summary>
        [Description("您好,需要填写培训主管!")]
        TrainingDirectorIsNullOrEmpty = 8,
        /// <summary>
        /// 添加成功
        /// </summary>
        [Description("添加成功!")]
        CreateSuccess = 9,
        /// <summary>
        /// 修改成功
        /// </summary>
        [Description("修改成功!")]
        EditSuccess = 10,
        /// <summary>
        /// 删除成功
        /// </summary>
        [Description("删除成功!")]
        DeleteSuccess = 11,
        /// <summary>
        /// 需要添加紧急联系人
        /// </summary>
        [Description("您好，需要添加紧急联系人!")]
        EmergencyContactPersonIsNull = 12,
        /// <summary>
        /// 需要添加紧急联系方式
        /// </summary>
        [Description("您好，需要添加紧急联系方式!")]
        EmergencyContactNumberIsNull = 13,
        /// <summary>
        /// 需要选择上班地点
        /// </summary>
        [Description("您好，需要选择上班地点!")]
        WorkLocationIsNull = 14,
        /// <summary>
        /// 密码位数只能3~8位!
        /// </summary>
        [Description("您好,请检查密码位数只能3~8位!")]
        pwdLengthError = 15,
        /// <summary>
        /// 正则不通过(输入了错误的数据)
        /// </summary>
        [Description("您好,输入了错误的数据!")]
        StringError = 16,
        /// <summary>
        /// 错误的旧密码
        /// </summary>
        [Description("修改失败,错误的旧密码!")]
        oldPwdError = 17,
        /// <summary>
        /// 需要填写中文名称
        /// </summary>
        [Description("您好,需要填写昵称名称!")]
        NickNameIsNullOrEmpty = 18,
        /// <summary>
        /// 系统错误
        /// </summary>
        [Description("删除失败，该数据关联了宿舍管理与费用管理的相关信息!")]
        DelError = 501,
        /// <summary>
        /// 中文名称超出长度10
        /// </summary>
        [Description("您好,中文名称最多10个字!")]
        ChineseNameLengthExceed = 111,
        /// <summary>
        /// 护照名称超出长度20
        /// </summary>
        [Description("您好,护照名称最多50个字!")]
        PassportNameLengthExceed = 112,
        /// <summary>
        /// 护照ID超出长度15
        /// </summary>
        [Description("您好,护照ID最多15位!")]
        PassportIdLengthExceed = 113,
        /// <summary>
        /// 昵称超出长度20
        /// </summary>
        [Description("您好,昵称最多20个字!")]
        NicknameLengthExceed = 114,
        /// <summary>
        /// QQ号码超出长度15
        /// </summary>
        [Description("您好,QQ号码最多15位!")]
        QqNumberLengthExceed = 115,
        /// <summary>
        /// 国内号码超出长度20
        /// </summary>
        [Description("您好,国内电话最多20位!")]
        TelNoCnLengthExceed = 116,
        /// <summary>
        /// 菲线号码超出长度20
        /// </summary>
        [Description("您好,在菲电话最多20位!")]
        TelNoPhLengthExceed = 117,
        /// <summary>
        /// 紧急联系人超出长度10
        /// </summary>
        [Description("您好,紧急联系人最多10个字!")]
        EmergencyContactPersonLengthExceed = 118,
        /// <summary>
        /// 紧急联系电话超出长度20
        /// </summary>
        [Description("您好, 紧急联系电话最多20位!")]
        EmergencyContactNumberLengthExceed = 119,
        /// <summary>
        /// 备注超出500
        /// </summary>
        [Description("您好, 备注最多500个字符!")]
        RemarkLengthExceed = 120,
        /// <summary>
        /// 配置项已存在数据
        /// </summary>
        [Description("配置项已存在考核数据")]
        ConfigHasValue = 151,
        /// <summary>
        /// 配置项已存在数据
        /// </summary>
        [Description("配置项不存在考核数据")]
        ConfigHasNoValue = 152,

    }
    public static class myEnum
    {
        /// <summary>
        /// 获取描述信息
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public static string Description(this Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return en.ToString();
        }
    }
}
