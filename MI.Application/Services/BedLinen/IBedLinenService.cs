using MI.Application.Dto;
using MI.Core.Common;
using MI.Core.Domain;
using System.Collections.Generic;

namespace MI.Application
{
    public interface IBedLinenService
    {
        /// <summary>
        /// 根据id获取一条数据
        /// </summary>
        /// <param name="iId">主键id</param>
        /// <returns>返回BedLinen实体</returns>
        BedLinen GetBedLinenById(int iId);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="oModel">BedLinen实体</param>
        ErrorEnum AddBedLinenOneData(BedLinen oModel);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="oModel">BedLinen实体</param>
        ErrorEnum EditBedLinenOneData(BedLinen oModel);
        /// <summary>
        /// 根据员工id查询
        /// </summary>
        /// <param name="iEid">员工id</param>
        /// <returns>返回一个BedLinenDto</returns>
        List<BedLinenDto> GetBedLinenListByEid(int iEid);
        /// <summary>
        /// 按条件查询所有床单送洗数据
        /// </summary>
        /// <param name="queryModel">条件</param>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">每页多少条数据</param>
        /// <param name="o_Count">总数</param>
        /// <param name="bIstrue">是否需要分页</param>
        /// <returns>返回BedLinenViewModel集合</returns>
        List<BedLinenViewModel> GetBedLinenAllData(BedLinenWhere queryModel, int iPageIndex, int iPageSize,
            out int o_Count, bool bIstrue = false);
        /// <summary>
        /// 根据id删除一条数据
        /// </summary>
        /// <param name="iId">主键id</param>
        ErrorEnum DeleteBedLinen(int iId);
        /// <summary>
        /// 按条件导出所有床单送洗数据
        /// </summary>
        /// <param name="queryModel">条件</param>
        /// <param name="bIsSummary">是否导出的汇总数据</param>
        /// <returns>返回一个匿名集合</returns>
        List<dynamic> ExportBedLinenAllData(BedLinenWhere queryModel, bool bIsSummary = false);
        /// <summary>
        /// 根据宿舍获取总的送洗项目数量
        /// </summary>
        /// <param name="queryModel">条件</param>
        /// <returns></returns>
        List<BedLinenViewModel> GetStatsBedLinen(BedLinenWhere queryModel);

    }
}
