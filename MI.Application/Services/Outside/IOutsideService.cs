﻿using MI.Core.Domain;
using MI.Data.Uow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application
{
    [UnitOfWork]
    /// <summary>
    /// 员工外租接口
    /// 创建人：吕秀峰
    /// 创建时间：2017-06-23
    /// </summary>
    public interface IOutsideService
    {
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<Outside> GetConditionByWhere(Expression<Func<Outside, bool>> predicate, int pageIndex, int pageSize, out int count);
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="cartype"></param>
        /// <returns></returns>
        List<Outside> GetOutsideAllData(int pageIndex, int pageSize, out int count);
        /// <summary>
        /// 根据id获取一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        Outside GetOutsideById(int id);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">model实体</param>
        void AddOutsideOneData(Outside model);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">model实体</param>
        void EditOutsideOneData(Outside model);
        /// <summary>
        /// 根据id删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        void DeleteOutside(int id);
    }
}
