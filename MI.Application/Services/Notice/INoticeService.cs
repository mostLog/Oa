using MI.Core.Common;
using MI.Core.Domain;
using MI.Data.Uow;
using System;
using System.Collections.Generic;

namespace MI.Application
{
    [UnitOfWork]
    public interface INoticeService
    {
        /// <summary>
        /// 公告管理
        /// </summary>
        /// <returns>返回t_Notice集合</returns>
        List<Notice> GetNoticeAllData();

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>返回t_Notice集合</returns>
        List<Notice> GetConditionByWhere(Func<Notice, bool> predicate);

        /// <summary>
        /// 根据id获取一条数据
        /// </summary>
        /// <param name="iId">id</param>
        /// <returns>返回一个t_Notice实体</returns>
        Notice GetNoticeById(int iId);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">t_Notice实体</param>
        ErrorEnum AddNoticeOneData(Notice model);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">t_Notice实体</param>
        ErrorEnum EditNoticeOneData(Notice model);

        /// <summary>
        /// 根据id删除一条数据
        /// </summary>
        /// <param name="iId">Id</param>
        ErrorEnum DeleteNotice(int iId);

        /// <summary>
        /// 置顶
        /// </summary>
        /// <param name="iId">id</param>
        /// <param name="istop">当前置顶状态</param>
        ErrorEnum IsTop(int iId, string istop);
    }
}
