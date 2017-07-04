using MI.Core.Domain;
using MI.Data;
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
    public class HostelCleanService:IHostelCleanService
    {
        private readonly IBaseRepository<t_HostelClean> _hostelClear;
        private readonly IBaseRepository<t_Dormitory> _dormitory;
        public HostelCleanService(IBaseRepository<t_HostelClean> hostelClear, IBaseRepository<t_Dormitory> dormitory) {
            _hostelClear = hostelClear;
            _dormitory = dormitory;
        }
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<t_HostelClean> GetHostelCleanAllData(int pageIndex, int pageSize, out int count)
        {
            var linq = _hostelClear.GetAll().OrderBy(u => u.f_Week).ThenBy(u => u.f_Id);
            count = linq.Count();
            ValidatePagingWhere(linq.Count(), ref pageIndex, pageSize);
            return linq.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
        /// <summary>
        /// 判断分页条件pageIndex是否超出总页码
        /// </summary>
        /// <param name="sCount">数据总行数</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="PageSize">每页多少条数据</param>
        public void ValidatePagingWhere(int sCount, ref int r_PageIndex, double PageSize)
        {
            if (sCount > 0 && Math.Ceiling(sCount / PageSize) < r_PageIndex)
            {
                r_PageIndex = 1;
            }
        }
        /// <summary>
        /// 查询所有 导出EXCEL
        /// </summary>
        /// <returns></returns>
        public List<dynamic> GetHostelCleanAllDataExportData()
        {
            IEnumerable<t_HostelClean> hostelclean1 = _hostelClear.GetAll().OrderBy(u => u.f_Week);
            IEnumerable<t_Dormitory> dormitory = _dormitory.GetAll();
            IEnumerable<dynamic> hostelclean = from p in hostelclean1.ToList()
                                               join d in dormitory.ToList() on p.f_DormitoryId equals d.f_DormitoryId
                                               select new
                                               {
                                                   序號 = p.f_Id,
                                                   社區 = d.f_Community,
                                                   樓棟 = d.f_Building,
                                                   房號 = d.f_RoomNo,
                                                   星期 = WeekConverter(p.f_Week),
                                                   執行人 = p.f_Cleaner
                                               };
            List<dynamic> list = hostelclean.ToList();
            return list;
        }

        private string WeekConverter(int iWeek)
        {
            switch (iWeek)
            {
                case 1:
                    return "星期一";
                case 2:
                    return "星期二";
                case 3:
                    return "星期三";
                case 4:
                    return "星期四";
                case 5:
                    return "星期五";
                case 6:
                    return "星期六";
                case 7:
                    return "星期日";
                default:
                    return "--";

            }
        }

        public List<t_HostelClean> GetConditionByWhere(Expression<Func<t_HostelClean, bool>> predicate, int pageIndex, int pageSize, out int count)
        {
            var linq = _hostelClear.GetAll().Where(predicate).OrderBy(u => u.f_Week).ThenBy(u => u.f_Id);
            count = linq.Count();
            ValidatePagingWhere(linq.Count(), ref pageIndex, pageSize);
            return linq.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// 根据条件查询 导出EXCEL
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<dynamic> GetConditionByWhereExportData(Expression<Func<t_HostelClean, bool>> predicate)
        {
            IEnumerable<t_HostelClean> hostelclean1 = _hostelClear.GetAll().Where(predicate).OrderBy(u => u.f_Week);
            IEnumerable<t_Dormitory> dormitory = _dormitory.GetAll();
            IEnumerable<dynamic> hostelclean = from p in hostelclean1.ToList()
                                               join d in dormitory.ToList() on p.f_DormitoryId equals d.f_DormitoryId
                                               select new
                                               {
                                                   序號 = p.f_Id,
                                                   社區 = d.f_Community,
                                                   樓棟 = d.f_Building,
                                                   房號 = d.f_RoomNo,
                                                   星期 = WeekConverter(p.f_Week),
                                                   執行人 = p.f_Cleaner
                                               };
            List<dynamic> list = hostelclean.ToList();
            return list;
        }

        /// <summary>
        /// 根据id获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public t_HostelClean GetHostelCleanById(int id)
        {
            return _hostelClear.GetEntityById(id);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">model实体</param>
        public void AddHostelCleanOneData(t_HostelClean model)
        {
             _hostelClear.Insert(model);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">model实体</param>
        public void EditHostelCleanOneData(t_HostelClean model)
        {
            _hostelClear.Update(model);
        }
        /// <summary>
        /// 根据id删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        public void DeleteHostelClean(int id)
        {
            t_HostelClean model = GetHostelCleanById(id);
            if (model != null)
            {
                 _hostelClear.Delete(model);
                
            }
            else {
            }
        }
    }
}
