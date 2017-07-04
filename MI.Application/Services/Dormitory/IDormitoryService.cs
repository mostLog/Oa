using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MI.Core.Domain;

namespace MI.Application
{
   public interface IDormitoryService
    {
        IList<Dormitory> GetConditionByWhere(int pageIndex, int pageSize, out int o_Count);
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<Dormitory> GetConditionByWhere(Func<Dormitory, bool> predicate, int pageIndex, int pageSize, out int count);
        /// <summary>
        /// 通过id获取楼栋信息
        /// 
        /// 小白-2017-6-13
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Dormitory GetDormitoryById(int id);
        /// <summary>
        /// 分组查询所有社区
        /// </summary>
        /// <param name="iswhere">是否只查包含洗衣房晾衣房的</param>
        /// <returns></returns>
        List<string> GetTariffbyCommunity(bool iswhere = false);
        /// <summary>
        /// 根据社区查询楼栋
        /// </summary>
        /// <param name="community">社区</param>
        /// <param name="iswhere">是否只查包含洗衣房晾衣房的</param>
        /// <returns></returns>
        List<string> GetTariffbyCommunity(string community, bool iswhere = false);

        /// <summary>
        /// 根据社区，楼栋查询社区的所有房间号
        /// </summary>
        /// <param name="community">社区</param>
        /// <param name="building">楼栋</param>
        /// <param name="iswhere">是否只查包含洗衣房晾衣房的</param>
        /// <returns></returns>
        List<string> GetTariffbyBuilding(string community, string building, bool iswhere = false);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">model实体</param>
        void EditDormitoryOneData(Dormitory model);
        /// <summary>
        /// 根据社区,楼栋,房间号查询房间id
        /// </summary>
        /// <param name="community">社区</param>
        /// <param name="building">楼栋</param>
        /// <param name="roomno">房间号</param>
        /// <returns></returns>
        Dormitory GetTariffbyRoomNo(string community, string building, string roomno);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">model实体</param>
        void AddDormitoryOneData(Dormitory model);
        /// <summary>
        /// 根据id删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        void DeleteDormitory(int id);

        /// <summary>
        /// 根据条件查询 导出EXCEL
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<dynamic> GetConditionByWhereExportData(Func<Dormitory, bool> predicate);

        /// <summary>
        /// 查询所有(EXCEL)
        /// </summary>
        /// <returns></returns>
        List<dynamic> GetDormitoryAllExportData();
    }
}
