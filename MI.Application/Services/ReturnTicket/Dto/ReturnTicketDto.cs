using MI.Core.Domain;
using System;

namespace MI.Application.Dto
{
    public class ReturnTicketDto
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public int f_Id { get; set; }
        /// <summary>
        /// 员工Id
        /// </summary>
        public Nullable<int> f_eId { get; set; }
        /// <summary>
        /// 返乡日期
        /// </summary>
        public Nullable<System.DateTime> f_ToDate { get; set; }
        /// <summary>
        /// 返乡航空公司
        /// </summary>
        public Nullable<int> f_ToAirlineType_Id { get; set; }
        /// <summary>
        /// 返乡航班时间
        /// </summary>
        public string f_ToFlight { get; set; }
        /// <summary>
        /// 返乡文件路径
        /// </summary>
        public string f_ToFile { get; set; }
        /// <summary>
        /// 返乡备注
        /// </summary>
        public string f_ToRemark { get; set; }
        /// <summary>
        /// 返菲日期
        /// </summary>
        public Nullable<System.DateTime> f_FromDate { get; set; }
        /// <summary>
        /// 返菲航空公司
        /// </summary>
        public Nullable<int> f_FromAirlineType_Id { get; set; }
        /// <summary>
        /// 返菲航班时间
        /// </summary>
        public string f_FromFlight { get; set; }
        /// <summary>
        /// 返菲文件路径
        /// </summary>
        public string f_FromFile { get; set; }
        /// <summary>
        /// 返菲备注
        /// </summary>
        public string f_FromRemark { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string f_Operation { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public System.DateTime f_OperationDate { get; set; }
        /// <summary>
        /// 送机出发时间
        /// </summary>
        public string f_ToDropOffTime { get; set; }
        /// <summary>
        /// 接机出发时间
        /// </summary>
        public string f_FromDropOffTime { get; set; }
        /// <summary>
        /// 返乡 菲航站楼
        /// </summary>
        public string f_ToTerminal { get; set; }
        /// <summary>
        /// 返菲  菲航站楼
        /// </summary>
        public string f_FromTerminal { get; set; }
        /// <summary>
        /// 送机派车备注
        /// </summary>
        public string f_ToSendACarRemark { get; set; }
        /// <summary>
        /// 接机派车备注
        /// </summary>
        public string f_FromSendACarRemark { get; set; }
        /// <summary>
        /// 返乡是否派车-0为没有派车
        /// </summary>
        public bool f_ToIsSendACar { get; set; }
        /// <summary>
        /// 返菲是否派车-0为没有派车
        /// </summary>
        public bool f_FromIsSendACar { get; set; }
        /// <summary>
        /// 返乡资料是否提示
        /// </summary>
        public bool f_ToIsTips { get; set; }
        /// <summary>
        /// 返菲资料是否提示
        /// </summary>
        public bool f_FromIsTips { get; set; }
        /// <summary>
        /// 是否为新人那边添加的机票记录
        /// </summary>
        public bool f_IsNewEmp { get; set; }
        /// <summary>
        /// 操作人Id
        /// </summary>
        public int f_OperationId { get; set; }
        /// <summary>
        /// 送机编号
        /// </summary>
        public Nullable<int> f_ToCode { get; set; }
        /// <summary>
        /// 接机编号
        /// </summary>
        public Nullable<int> f_FromCode { get; set; }
        /// <summary>
        /// 返乡地点栏
        /// </summary>
        public string f_ToAddress { get; set; }
        /// <summary>
        /// 返菲地点栏
        /// </summary>
        public string f_FromAddress { get; set; }

        /// <summary>
        /// 关联外键   员工id(f_eid)
        /// </summary>
        public virtual Employee t_employeeInfo { get; set; }
    }
}
