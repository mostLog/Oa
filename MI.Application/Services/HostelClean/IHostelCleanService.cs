using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application
{
    /// <summary>
    /// 作者：吕秀峰 
    /// 创始时间：2017-06-24
    /// 描述：宿舍打扫接口
    /// </summary>
    public interface IHostelCleanService
    {
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="cartype"></param>
        /// <returns></returns>
        List<HostelClean> GetHostelCleanAllData(int pageIndex, int pageSize, out int count);

        /// <summary>
        /// 查询所有 导出EXCEL
        /// </summary>
        /// <returns></returns>
        List<dynamic> GetHostelCleanAllDataExportData();
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<HostelClean> GetConditionByWhere(Expression<Func<HostelClean, bool>> predicate, int pageIndex, int pageSize, out int count);

        /// <summary>
        /// 根据条件查询 导出EXCEL
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<dynamic> GetConditionByWhereExportData(Expression<Func<HostelClean, bool>> predicate);

        /// <summary>
        /// 根据id获取一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        HostelClean GetHostelCleanById(int id);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">model实体</param>
        void AddHostelCleanOneData(HostelClean model);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">model实体</param>
        void EditHostelCleanOneData(HostelClean model);
        /// <summary>
        /// 根据id删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        void DeleteHostelClean(int id);
    }
}
