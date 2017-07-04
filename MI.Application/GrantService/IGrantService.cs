using MI.Application.Dto;
using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MI.Application
{ 
    public interface IGrantService: IBaseService<Grant>
    {
        /// <summary>
        /// 分页查询外租补助信息
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="iPageIndex">当前页</param>
        /// <param name="iPageSize">页大小</param>
        /// <param name="o_Count">总数</param>
        /// <returns></returns>
        IList<GrantDto> GetGrantByWhere(Expression<Func<Grant,bool>> predicate,int iPageIndex,int iPageSize,out int o_Count);
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PageListDto<GrantDto> GetPagedAllGrants(GrantPagedInputDto input);
        /// <summary>
        /// 根据本月生成数据
        /// </summary>
        /// <returns></returns>
        int GenerateCurrentMonthData();
    }
}
