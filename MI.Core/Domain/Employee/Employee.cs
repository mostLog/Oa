
using MI.Core.Common;
using System;
using System.Collections.Generic;

namespace MI.Core.Domain
{
    /// <summary>
    /// 员工表
    /// </summary>
    public class Employee
    {
        public int f_eid { get; set; }
        /// <summary>
        /// 外键 对应sType 班次类型
        /// </summary>
        public Nullable<int> f_periodType_tID { get; set; }
        /// <summary>
        /// 外键 
        /// </summary>
        public int f_department_tID { get; set; }
        /// <summary>
        /// 中文名
        /// </summary>
        public string f_chineseName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public byte f_sex { get; set; }
        /// <summary>
        /// 护照id
        /// </summary>
        public string f_passportID { get; set; }
        /// <summary>
        /// 护照英文名
        /// </summary>
        public string f_passportName { get; set; }
        /// <summary>
        /// 存储护照照片URL
        /// </summary>
        public string f_PassportURL { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string f_nickname { get; set; }
        /// <summary>
        /// 国籍
        /// </summary>
        public string f_international { get; set; }
        /// <summary>
        /// QQ号码
        /// </summary>
        public string f_QQNumber { get; set; }
        /// <summary>
        /// 菲律宾联系电话
        /// </summary>
        public string f_TelNoPH { get; set; }
        /// <summary>
        /// 国内电话
        /// </summary>
        public string f_TelNoCN { get; set; }
        /// <summary>
        /// 紧急联系人
        /// </summary>
        public string f_emergencyContactPerson { get; set; }
        /// <summary>
        /// 紧急联系人电话
        /// </summary>
        public string f_emergencyContactNumber { get; set; }
        /// <summary>
        /// 微信
        /// </summary>
        public string f_Wechat { get; set; }
        /// <summary>
        /// 宿舍外键id
        /// </summary>
        public Nullable<int> f_dormitoryId { get; set; }
        /// <summary>
        /// 外键  
        /// </summary>
        public Nullable<int> f_workStatus_tID { get; set; }
        /// <summary>
        /// 上班搭车时间
        /// </summary>
        public Nullable<System.DateTime> f_rideWorkTime { get; set; }
        /// <summary>
        /// 下班搭车时间
        /// </summary>
        public Nullable<System.DateTime> f_rideOffWorkTime { get; set; }
        /// <summary>
        /// 外键 工作地点  1.PBCOM。2.RCBC。 3.新据点
        /// </summary>
        public Nullable<int> f_workLocation_tID { get; set; }

        /// <summary>
        /// 备注；(可记录其他信息)
        /// </summary>
        public string f_Remark { get; set; }
        /// <summary>
        /// 密码，因为人事信息一般是由行政或者领导新增。可需设置默认密码
        /// </summary>
        public string f_pwd { get; set; }
        /// <summary>
        /// 外键 等级权限，0 小菲(没有权限) ，1普通员工(最低) ，2高层(主管)，3.行政部门(最高)。 
        /// </summary>
        public int f_level_tID { get; set; }
        /// <summary>
        /// 外键 新人表中的id
        /// </summary>
        public Nullable<int> f_nid { get; set; }
        /// <summary>
        /// 是否为菲律宾员工（0=不是，1=是）
        /// </summary>
        public bool f_IsPHStaff { get; set; }
        /// <summary>
        /// 薪资（此项只有菲律宾员工才会用到）
        /// </summary>
        public Nullable<decimal> f_Salary { get; set; }
        /// <summary>
        /// 帐号
        /// </summary>
        public string f_AccountName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime f_CreateDate { get; set; }
        /// <summary>
        /// 签证类型
        /// </summary>
        public Nullable<int> f_VisaType_tID { get; set; }
        /// <summary>
        /// 存储入境章照片URL
        /// </summary>
        public string f_EntrystampURL { get; set; }
        /// <summary>
        /// 签证到期时间
        /// </summary>
        public Nullable<System.DateTime> f_passportDate { get; set; }
        /// <summary>
        /// 入职日期
        /// </summary>
        public Nullable<System.DateTime> f_HireDate { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public Nullable<System.DateTime> f_DateOfBirth { get; set; }
        /// <summary>
        /// 是否已移走0：没有移走1，已移走
        /// </summary>
        public bool f_IsRemove { get; set; }
        /// <summary>
        /// 是否为新员工0：非新员工，1新员工
        /// </summary>
        public bool f_IsNewEmp { get; set; }
        /// <summary>
        /// I卡或者I卡证明到期时间
        /// </summary>
        public Nullable<System.DateTime> f_ICardDate { get; set; }
        /// <summary>
        /// 护照英文名别名
        /// </summary>
        public string f_passportAlis { get; set; }
        /// <summary>
        /// 离职日期
        /// </summary>
        public Nullable<System.DateTime> f_SeparationDate { get; set; }
        /// <summary>
        /// 保存旧的订餐信息（为新员工的时候的订餐信息）
        /// </summary>
        public string f_OldReservation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int f_JLStage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int f_JLgroup { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int f_JLdirector_eId { get; set; }
        /// <summary>
        /// 保存金流组长多个组别
        /// </summary>
        public string f_mutilpleGroup { get; set; }
        /// <summary>
        /// 证件照URL
        /// </summary>
        public string f_IDCardURL { get; set; }
        /// <summary>
        /// 新员工信息表
        /// </summary>
        public virtual NewEmployee NewEmployee { get; set; }
        /// <summary>
        /// 班次类型
        /// </summary>
        public virtual SType STypePeriodType { get; set; }
        /// <summary>
        /// 员工部门
        /// </summary>
        public virtual SType STypeDepartment { get; set; }
        /// <summary>
        /// 工作状态
        /// </summary>
        public virtual SType STypeWorkStatus { get; set; }
        /// <summary>
        /// 工作地点
        /// </summary>
        public virtual SType STypeWorkLocation { get; set; }
        /// <summary>
        /// 员工等级
        /// </summary>
        public virtual SType STypeLevel { get; set; }
        /// <summary>
        /// 关联外键  员工id  
        /// </summary>
        public virtual ICollection<EmpRent> EmpRents { get; set; }

        /// <summary>
        ///外键关联  f_dormitoryId
        /// </summary>
        public virtual t_Dormitory t_Dormitory { get; set; }
        /// <summary>
        /// 关联表Grant  对应员工id
        /// </summary>
        public virtual ICollection<Grant> t_grant { get; set; }

        /// <summary>
        /// 工作交接
        /// </summary>
        public virtual ICollection<WorkDistribution> WorkDistribution { get; set; }

        /// <summary>
        /// ReturnTicket  对应员工id
        /// </summary>
        public virtual ICollection<ReturnTicket> t_ReturnTicket { get; set; }
        /// <summary>
        /// 返乡交接
        /// </summary>
        public virtual ICollection<ReturnHandover> ReturnHandover { get; set; }
        /// <summary>
        /// 订餐表泛型集合
        /// </summary>
        public virtual ICollection<OrderingEmployees> OrderingEmployees { get; set; }
        /// <summary>
        /// 新人订餐泛型集合
        /// </summary>
        public virtual ICollection<NewOrderingEmployees> NewOrderingEmoloyees { get; set; }
        /// <summary>
        /// 关联类型表
        /// </summary>
        public virtual SType t_sType { get; set; }
        /// <summary>
        /// 员工机票差额统计
        /// </summary>
        public virtual ICollection<FlightFee> FlightFee { get; set; }

    
        public virtual ICollection<t_ChangeRoom> t_ChangeRoom { get; set; }
        public virtual ICollection<t_Outside> t_Outside { get; set; }
        public virtual ICollection<t_Outside> t_Outside1 { get; set; }
    }
}
