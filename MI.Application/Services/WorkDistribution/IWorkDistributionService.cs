using MI.Application.Dto;
using MI.Core.Common;
using MI.Core.Domain;
using System;
using System.Collections.Generic;

namespace MI.Application
{
    public interface IWorkDistributionService
    {
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">每页多少数据</param>
        /// <param name="iCount">总数据</param>
        /// <returns>返回一个WorkDistribution的list集合</returns>
        List<WorkDistribution> GetWorkDistributionAllData(int iPageIndex, int iPageSize, out int iCount);
        /// <summary>
        /// 根据id获取一条数据
        /// </summary>
        /// <param name="iId"></param>
        /// <returns></returns>
        WorkDistribution GetWorkOneDataById(int iId);
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns></returns>
        List<WorkDistribution> GetWorkByWhere(Func<WorkDistribution, bool> predicate,int iPageIndex, int iPageSize, out int iCount);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ErrorEnum AddWorkOneData(WorkDistribution model);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ErrorEnum UpdataOneData(WorkDistribution model);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="iId"></param>
        /// <returns></returns>
        ErrorEnum DeleteById(int iId);
        /// <summary>
        /// 根据条件返回列表
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>返回一个WorkDistribution的list集合</returns>
        List<WorkDistribution> GetWorkDistributionByWhere(Func<WorkDistribution, bool> predicate);
    }
}
