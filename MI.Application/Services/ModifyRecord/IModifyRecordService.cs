
using MI.Application.Dto;
using MI.Core.Common;
using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application
{
     public interface IModifyRecordService
    {
        IList<ModifyRecordDto> GetEmpRent();

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">model实体</param>
        void AddModifyRecordData(t_ModifyRecord model);
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">每页多少条数据</param>
        /// <param name="iCount">总数</param>
        /// <returns>返回t_ModifyRecord集合</returns>
        List<t_ModifyRecord> GetModifyRecordAllData(int iPageIndex, int iPageSize, out int iCount);

        /// <summary>
        /// 根据条件返回列表
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">每页多少数据</param>
        /// <param name="iCount">总数</param>
        /// <returns>返回t_ModifyRecord集合</returns>
        List<t_ModifyRecord> GetModifyRecordByWhere(Func<t_ModifyRecord, bool> predicate, int iPageIndex, int iPageSize, out int iCount);

        /// <summary>
        /// 恢复数据
        /// </summary>
        /// <param name="iId">id</param>
        ErrorEnum Recovery(int iId);
    }
}
