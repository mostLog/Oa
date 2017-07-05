using MI.Application.Dto;
using MI.Core.Common;
using MI.Core.Domain;
using MI.Data.Uow;
using System;
using System.Collections.Generic;

namespace MI.Application
{
    [UnitOfWork]
    public interface IReturnHandoverService
    {
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        IList<ReturnHandover> GetAllReturnData();
        /// <summary>
        /// 根据id获取一条数据
        /// </summary>
        /// <param name="iId"></param>
        /// <returns></returns>
        ReturnHandover GetReturnOneDataById(int iId);
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <returns></returns>
        IList<ReturnHandover> GetReturnByWhere(Func<ReturnHandover, bool> predicate);
        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        ErrorEnum AddReturnOneData(ReturnHandover model);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ErrorEnum UpdateReturnOneData(ReturnHandover model);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        ErrorEnum DeleteReturnOneData(int Id);
    }
}
