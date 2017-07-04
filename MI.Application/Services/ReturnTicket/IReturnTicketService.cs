using MI.Application.Dto;
using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MI.Application
{
    public interface IReturnTicketService
    {
        /// <summary>
        /// 根据条件查询送机派车所有数据
        /// </summary>
        /// <param name="sc">查询条件</param>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">页大小</param>
        /// <param name="o_Count">总数</param>
        /// <returns></returns>
        IList<ReturnTicketDto> GetSendCarByWhere(Expression<Func<ReturnTicket, bool>> predicate, int iPageIndex, int iPageSize, out int o_Count);
        /// <summary>
        /// 根据条件查询接机派车所有数据
        /// </summary>
        /// <param name="sc">查询条件</param>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">页大小</param>
        /// <param name="o_Count">总数</param>
        /// <returns></returns>
        IList<ReturnTicketDto> GetPickupByWhere(Expression<Func<ReturnTicket, bool>> predicate, int iPageIndex, int iPageSize, out int o_Count);

        /// <summary>
        /// 根据条件导出送机派车数据
        /// </summary>
        /// <param name="sc">查询条件</param>
        /// <returns></returns>
        List<dynamic> GetSendCarByWhereExport(Expression<Func<ReturnTicket, bool>> predicate);


        /// <summary>
        /// 根据条件导出送机派车数据
        /// </summary>
        /// <param name="sc">查询条件</param>
        /// <returns></returns>
        List<dynamic> GetPickupByWhereExport(Expression<Func<ReturnTicket, bool>> predicate);

        /// <summary>
        /// 根据id获取一条数据
        /// </summary>
        /// <param name="iId">传过来的参数</param>
        /// <returns></returns>
        ReturnTicket GetReturnTicketById(int iId);

        /// <summary>
        /// 返回送机的当前时间段最大编号
        /// </summary>
        /// <param name="dtStartDate">开始时间</param>
        /// <param name="dtEndDate">结束时间</param>
        /// <returns></returns>
        int GetSendMaxToCode(DateTime dtStartDate, DateTime dtEndDate);

        /// <summary>
        /// 返回送机的当前时间段最大编号
        /// </summary>
        /// <param name="dtStartDate">开始时间</param>
        /// <param name="dtEndDate">结束时间</param>
        /// <returns></returns>
        int GetPickupMaxToCode(DateTime dtStartDate, DateTime dtEndDate);  

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="oModel">model实体</param>
        /// <param name="bIsLogs">是否需要写入操作记录</param>
        void EditReturnTicketOneData(ReturnTicket oModel, ReturnTicket oldModel = null, bool bIsLogs = true, bool isSendCar = false);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="oModel">model实体</param>
        void AddReturnTicketOneData(ReturnTicket oModel, bool bIsLogs = true);
        /// <summary>
        /// 查询主管所在部门的所有机票信息
        /// </summary>
        /// <param name="iEid">员工id</param>
        /// <returns>返回一个ReturnTicket集合</returns>
        List<ReturnTicket> GetReturnTicketListBySector(int iEid);
        /// <summary>
        /// 查询送机派车根据员工ID
        /// </summary>
        /// <param name="iEid">员工id</param>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">每页几条数据</param>
        /// <param name="iCount">总数据</param>
        /// <returns>返回一个t_ReturnTicket集合</returns>
        List<ReturnTicket> GetSendCarByEid(int iEid, int iPageIndex, int iPageSize, out int iCount);
        /// <summary>
        /// 查询送机派车根据部门ID
        /// </summary>
        /// <param name="iDeptid">部门id</param>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">每页几条数据</param>
        /// <param name="iCount">总数据</param>
        /// <returns>返回一个t_ReturnTicket集合</returns>
        List<ReturnTicket> GetSendCarByDeptid(int iDeptid, int iPageIndex, int iPageSize, out int iCount);
        /// <summary>
        /// 查询接机派车根据员工ID
        /// </summary>
        /// <param name="iEid">员工id</param>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">每页几条数据</param>
        /// <param name="iCount">总数据</param>
        /// <returns>返回一个t_ReturnTicket集合</returns>
        List<ReturnTicket> GetPickupByEid(int iEid, int iPageIndex, int iPageSize, out int iCount);
        /// <summary>
        /// 查询接机派车根据部门ID
        /// </summary>
        /// <param name="iDeptid">部门id</param>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">每页几条数据</param>
        /// <param name="iCount">总数据</param>
        /// <returns>返回一个t_ReturnTicket集合</returns>
        List<ReturnTicket> GetPickupByDeptid(int iDeptid, int iPageIndex, int iPageSize, out int iCount);
        /// <summary>
        /// 获取类型名称的集合
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<SType> GetNameByStype(Func<SType, bool> predicate);
        /// <summary>
        /// 通过条件获取分页数据
        /// </summary>
        /// <param name="ticketDto">条件</param>
        /// <param name="iPageIndex"></param>
        /// <param name="iPageSize"></param>
        /// <param name="iCount"></param>
        /// <returns></returns>
        List<ReturnTicket> GetReturnTicketByWhere(TicketWhereDto ticketDto,int iPageIndex,int iPageSize,out int iCount);
        /// <summary>
        /// 获取所有分页数据
        /// </summary>
        /// <param name="iPageIndex"></param>
        /// <param name="iPageSize"></param>
        /// <param name="iCount"></param>
        /// <returns></returns>
        List<ReturnTicket> GetReturnTicketAllData(int iPageIndex, int iPageSize, out int iCount);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Deletet_ReturnTicket(int id);
        /// <summary>
        /// 根据条件导出
        /// </summary>
        /// <param name="oRtw">按条件查询的实体</param>
        /// <returns>返回一个匿名list的集合</returns>
        List<dynamic> ExportDataByWhere(TicketWhereDto ticketDto);
        /// <summary>
        /// 导出所有
        /// </summary>
        /// <returns></returns>
        List<dynamic> ExportData();

    }
}
