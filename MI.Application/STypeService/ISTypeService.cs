using MI.Core.Common;
using MI.Core.Domain;
using System;
using System.Collections.Generic;

namespace MI.Application
{
    public interface ISTypeService
    {
        /// <summary>
        /// 获取所有类型
        /// </summary>
        /// <returns></returns>
        IList<SType> GetSType();
        /// <summary>
        /// 根据条件查询类型
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IList<SType> GetsTypeByWhere(int predicate);
        /// <summary>
        /// 根据id获取一条数据
        /// </summary>
        /// <param name="iId"></param>
        /// <returns></returns>
        SType GetsTypeById(int iId);
        /// <summary>
        ///新增
        /// </summary>
        /// <param name="oModel"></param>
        ErrorEnum AddsTypeOneData(SType oModel);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        ErrorEnum EditOneData(SType model);
        /// <summary>
        /// 根据id删除一条数据
        /// </summary>
        /// <param name="iId">id</param>
        string DeletesType(int iId);
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IList<SType> GetTypeByWhere(Func<SType, bool> sType);
        /// <summary>
        /// 获取社区,避免大小写重复
        /// </summary>
        /// <param name="sCommunity">需要添加的社区</param>
        /// <returns>社区</returns>
        string GetReplaceCommunity(string sCommunity);
        /// <summary>
        /// 获取楼栋(忽略大小写,避免重复)
        /// </summary>
        /// <param name="sBuilding">需要添加的楼栋</param>
        /// <returns>楼栋</returns>
        string GetReplaceBuilding(string sBuilding);

        /// <summary>
        /// 根据条件查询类型
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>返回一个t_sType的list集合</returns>
        List<SType> QueryByCondition(Func<SType, bool> predicate);
    }
}
