using MI.Core.Domain;
using MI.Data;
using System.Collections.Generic;
using System.Linq;
using System;
using MI.Core.Common;

namespace MI.Application
{
    public class STypeService : ISTypeService
    {
        private readonly IBaseRepository<SType> _repository;
        private readonly IBaseRepository<Employee> _employee;
        private readonly IBaseRepository<CarRegister> _carregister;
        private readonly IBaseRepository<WorkDistribution> _workdistribution;
        private readonly IBaseRepository<CompanyOfFood> _companyoffood;
        public STypeService(IBaseRepository<SType> repository,
            IBaseRepository<CarRegister> carregister,
            IBaseRepository<WorkDistribution> workdistribution,
            IBaseRepository<CompanyOfFood> companyoffood,
            IBaseRepository<Employee> employee)
        {
            _repository = repository;
            _employee = employee;
            _carregister = carregister;
            _workdistribution = workdistribution;
            _companyoffood = companyoffood;
        }

        public IList<SType> GetSType()
        {
            return _repository.GetAll().ToList();
        }
        /// <summary>
        /// 根据id获取一条数据
        /// </summary>
        /// <param name="iId"></param>
        /// <returns></returns>
        public SType GetsTypeById(int iId)
        {
            return _repository.GetEntityById(iId);
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IList<SType> GetsTypeByWhere(int predicate)
        {
            IList<SType> STypeList = _repository.GetAll()
                .Where(m => m.f_type == predicate)
                .ToList();
            return STypeList;
        }
        /// <summary>
        /// 根据id修改一条数据
        /// </summary>
        /// <param name="model"></param>
        public ErrorEnum EditOneData(SType model)
        {
            ErrorEnum result = ErrorEnum.Error;
            _repository.Update(model);
            result = ErrorEnum.Success;
            return result;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        public ErrorEnum AddsTypeOneData(SType model)
        {
            ErrorEnum result = ErrorEnum.Error;
            _repository.Insert(model);
            result = ErrorEnum.Success;
            return result;
        }
        /// <summary>
        /// 根据id删除一条数据
        /// </summary>
        /// <param name="iId">id</param>
        public string DeletesType(int iId)
        {
            string tables = string.Empty;
            int count = 0;
            SType model = GetsTypeById(iId);
            if (model != null)
            {
                switch (model.f_type.ToString())
                {
                    case "0":
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                        count = _employee.GetAll().Count(u => u.f_level_tID == model.f_tID || u.f_periodType_tID == model.f_tID || u.f_department_tID == model.f_tID || u.f_workStatus_tID == model.f_tID || u.f_workLocation_tID == model.f_tID) + _companyoffood.GetAll().Count(u => u.f_type_tID == model.f_tID);
                        if (count > 0)
                        {
                            tables = "人事汇总表,";
                        }
                        break;
                    case "23":
                        count = _carregister.GetAll().Count(u => u.f_CarType == model.f_tID);
                        if (count > 0)
                        {
                            tables = tables + "车辆管理表,";
                        }
                        break;
                    case "24":
                        count = _workdistribution.GetAll().Count(u => u.f_Treat == model.f_tID);
                        if (count > 0)
                        {
                            tables = tables + "工作交接表";
                        }
                        break;
                }
                if (count == 0)
                {
                    _repository.Delete(model);
                }
            }
            return tables;
        }

        public IList<SType> GetTypeByWhere(Func<SType, bool> sType)
        {
            return _repository.GetAll().Where(sType).ToList();
        }
        /// <summary>
        /// 根据条件返回列表
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>返回t_sType集合</returns>
        public IList<SType> GetsTypeByWhere(Func<SType, bool> predicate)
        {
            return _repository.GetAll().Where(predicate).OrderByDescending(u => u.f_tID).ToList();
        }
        /// <summary>
        /// 获取社区,避免大小写重复
        /// </summary>
        /// <param name="sCommunity">社区</param>
        /// <returns>社区名</returns>
        public string GetReplaceCommunity(string sCommunity)
        {
            var model = _repository.GetAll().FirstOrDefault(p => p.f_value.ToUpper() == sCommunity.ToUpper() && p.f_type == (int)sTypeEnum.社区类型);
            return model?.f_value;
        }
        /// <summary>
        /// 获取楼栋,避免大小写重复
        /// </summary>
        /// <param name="sBuilding">需要添加的楼栋</param>
        /// <returns>楼栋</returns>
        public string GetReplaceBuilding(string sBuilding)
        {
            var model = _repository.GetAll().FirstOrDefault(p => p.f_value.ToUpper() == sBuilding.ToUpper() && p.f_type == (int)sTypeEnum.楼栋类型);
            return model?.f_value;
        }
        /// <summary>
        /// 根据条件查询类型
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>返回一个sType的list集合</returns>
        public List<SType> QueryByCondition(Func<SType, bool> predicate)
        {
            return _repository.GetAll().Where(predicate).ToList();
        }
    }
}
