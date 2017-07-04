using MI.Application.OASession;
using MI.Core;
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
    public class DormitoryMaintenanceService : BaseService<DormitoryMaintenance>, IDormitoryMaintenanceService
    {
        private readonly IBaseRepository<DormitoryMaintenance> _repository;
        private readonly ISession _session;
        private readonly IBaseRepository<Employee> _ELS;
        private readonly IRegistTipService _regservice;
        public DormitoryMaintenanceService(
            IBaseRepository<DormitoryMaintenance> repository,
            IBaseRepository<Employee> ELS,
            IRegistTipService regservice,
            ISession session) :
            base(repository)
        {
            _repository = repository;
            _regservice = regservice;
            _session = session;
            _ELS = ELS;
        }
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<DormitoryMaintenance> GetDormitoryMaintenanceAllData(int pageIndex, int pageSize, out int count)
        {
            var linq = _repository.GetAll().OrderByDescending(u => u.f_Id);
            count = linq.Count();
            ValidatePagingWhere(linq.Count(), ref pageIndex, pageSize);
            return linq.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<DormitoryMaintenance> GetConditionByWhere(Func<DormitoryMaintenance, bool> predicate, int pageIndex, int pageSize, out int count)
        {
            var linq = _repository.GetAll().Where(predicate).OrderByDescending(u => u.f_Id);
            count = linq.Count();
            ValidatePagingWhere(linq.Count(), ref pageIndex, pageSize);
            return linq.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<DormitoryMaintenance> GetConditionByWheres(Expression<Func<DormitoryMaintenance, bool>> predicate, int pageIndex, int pageSize, out int count)
        {
            var linq = _repository.GetAll().Where(predicate).OrderByDescending(u => u.f_Id);
            count = linq.Count();
            ValidatePagingWhere(linq.Count(), ref pageIndex, pageSize);
            return linq.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// 查询部门信息
        /// </summary>
        /// <param name="iDeptId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<DormitoryMaintenance> GetConditionByDeptId(int iDeptId, int pageIndex, int pageSize, out int count)
        {
            var lstRes = (from s in _ELS.GetAll().ToList()
                          where s.f_department_tID == iDeptId
                          select new { s.f_dormitoryId }).Distinct();

            var linq = (from c in _repository.GetAll().ToList()
                        join s in lstRes on c.f_DormitoryId equals s.f_dormitoryId
                        select c).OrderByDescending(c => c.f_Id);
            count = linq.Count();
            ValidatePagingWhere(linq.Count(), ref pageIndex, pageSize);
            return linq.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
        /// <summary>
        /// 根据id获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DormitoryMaintenance GetDormitoryMaintenanceById(int id)
        {
            return _repository.GetEntityById(id);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">model实体</param>
        public void EditDormitoryMaintenanceOneData(DormitoryMaintenance model)
        {
            //处理登记查询提示
            List<Employee> listEmp = _ELS.GetAll().Where(p => p.f_dormitoryId == model.f_DormitoryId).ToList();
            if (listEmp.Count > 0)
            {
                foreach (var emp in listEmp)
                {
                    _regservice.DoRegistTip(model, emp.f_eid, "DormitoryMaintenance", "维修查询");
                }
            }
            DormitoryMaintenance dm = new DormitoryMaintenance();
            dm.Entry(model).State = System.Data.Entity.EntityState.Modified;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">model实体</param>
        public void AddDormitoryMaintenanceOneData(DormitoryMaintenance model)
        {
            //处理登记查询提示
            List<Employee> listEmp = _ELS.GetAll().Where(p => p.f_dormitoryId == model.f_DormitoryId).ToList();
            if (listEmp.Count > 0)
            {
                foreach (var emp in listEmp)
                {
                    _regservice.DoRegistTip(model, emp.f_eid, "DormitoryMaintenance", "维修查询");
                }
            }
            _repository.Insert(model);
        }
        /// <summary>
        /// 根据id删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        public void DeleteDormitoryMaintenance(int id)
        {
            DormitoryMaintenance model = GetDormitoryMaintenanceById(id);
            if (model != null)
                _repository.Delete(model);
        }
    }
}
