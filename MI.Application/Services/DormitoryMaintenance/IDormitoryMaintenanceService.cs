using MI.Core;
using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application
{
    public interface IDormitoryMaintenanceService
    {
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="cartype"></param>
        /// <returns></returns>
        List<DormitoryMaintenance> GetDormitoryMaintenanceAllData(int pageIndex, int pageSize, out int count);
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<DormitoryMaintenance> GetConditionByWhere(Func<DormitoryMaintenance, bool> predicate, int pageIndex, int pageSize, out int count);
        List<DormitoryMaintenance> GetConditionByWheres(Expression<Func<DormitoryMaintenance, bool>> predicate, int pageIndex, int pageSize, out int count);
        
        /// <summary>
        /// 查询部门信息
        /// </summary>
        /// <param name="iDeptId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<DormitoryMaintenance> GetConditionByDeptId(int iDeptId, int pageIndex, int pageSize, out int count);
        /// <summary>
        /// 根据id获取一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        DormitoryMaintenance GetDormitoryMaintenanceById(int id);
        void EditDormitoryMaintenanceOneData(DormitoryMaintenance model);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">model实体</param>
        void AddDormitoryMaintenanceOneData(DormitoryMaintenance model);
        /// <summary>
        /// 根据id删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        void DeleteDormitoryMaintenance(int id);
    }
}
