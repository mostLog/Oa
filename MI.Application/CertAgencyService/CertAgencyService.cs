using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MI.Data;
using MI.Application.Dto;
using AutoMapper;
using MI.Core.Extension;
using System.Linq.Expressions;

namespace MI.Application
{
    public class CertAgencyService : BaseService<CertAgency>, ICertAgencyService
    {
        private readonly IBaseRepository<CertAgency> _repository;
        private readonly IBaseRepository<Employee> _employee;
        private readonly IBaseRepository<SType> _stype;
        private readonly ISTypeService _stypeservice;
        //登记查询服务
        private readonly IRegistTipService _registTipService;
        public CertAgencyService(IBaseRepository<CertAgency> repository,
            IBaseRepository<Employee> employee,
            ISTypeService stypeservice,
            IRegistTipService registTipService,
            IBaseRepository<SType> stype) : base(repository)
        {
            _repository = repository;
            _employee = employee;
            _stype = stype;
            _stypeservice = stypeservice;
            _registTipService = registTipService;
        }

        /// <summary>
        /// 新增证件办理
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public int AddCertAgencyData(CertAgency model)
        {
            //处理登记查询提示
            DoRegistTip(model, model.f_eId, "t_CertAgency", "证件查询");
            return _repository.Insert(model);
        }

        /// <summary>
        /// 修改证件办理
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public int EditCertAgencyData(CertAgency model)
        {
            //处理登记查询提示
            DoRegistTip(model, model.f_eId, "t_CertAgency", "证件查询");
            return _repository.Update(model);
        }

        /// <summary>
        /// 往登记查询提示表写操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">实体</param>
        /// <param name="eId">员工Id</param>
        /// <param name="strTable">登记查询表名</param>
        /// <param name="strTableName">登记查询表中文名</param>
        public virtual void DoRegistTip<T>(T model, int? eId, string strTable, string strTableName)
        {
            if (eId != null)
            {
                RegistTip regmodel = _registTipService.GetRegistTipByCondition(eId.Value, strTableName);
                if (regmodel == null)
                {
                    regmodel = new RegistTip
                    {
                        f_eId = eId.Value,
                        f_Table = strTable,
                        f_TableName = strTableName,
                        f_TipState = 1,
                        f_OperatorTime = DateTime.Now
                    };
                    _registTipService.AddRegistTipData(regmodel);
                }
                else
                {
                    regmodel = new RegistTip
                    {
                        f_eId = eId.Value,
                        f_Table = strTable,
                        f_TableName = strTableName,
                        f_TipState = 1,
                        f_OperatorTime = DateTime.Now
                    };
                    _registTipService.EditRegistTipData(regmodel);
                }
            }
        }

        /// <summary>
        /// 删除证件办理信息
        /// </summary>
        /// <param name="iId">要删除的ID</param>
        /// <returns></returns>
        public int DeleteCertAgency(int iId)
        {
            CertAgency model = _repository.GetEntityById(iId);
            return _repository.Delete(model);
        }

        /// <summary>
        /// 根据员工id查询证件信息
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        public List<CertAgencyDto> GetAgencyByEid(int eid)
        {
            var a = _repository.GetEntityById(eid);
            var Certificates = _stypeservice.GetsTypeByWhere((int)sTypeEnum.证件类型).FirstOrDefault(u => u.f_tID == a.f_CertificateTypeId).f_tID;
            var progress = _stypeservice.GetsTypeByWhere((int)sTypeEnum.办理进度).FirstOrDefault(u => u.f_tID == a.f_ProgressId).f_tID;
            return _repository.GetAll().Where(u => u.f_eId == eid).OrderByDescending(u => u.f_OperatorTime).ToList().Select(p => new CertAgencyDto
            {
                f_Id = p.f_Id,
                Eid = p.f_eId ?? 0,
                Nickname = _employee.GetEntityById(eid)?.f_nickname ?? "",
                Name = _employee.GetEntityById(eid)?.f_chineseName ?? "",
                WorkLocation = _stype.GetEntityById(_employee.GetEntityById(eid)?.f_workLocation_tID ?? 0).f_value,
                Sector = _stype.GetEntityById(_employee.GetEntityById(eid)?.f_department_tID ?? 0).f_value,
                f_CertificateType = _stype.GetEntityById(p.f_CertificateTypeId.HasValue ? Certificates : 0)?.f_value ?? "",
                f_Progress = _stype.GetEntityById(p.f_ProgressId.HasValue ? progress : 0)?.f_value ?? "",
                f_Operator = p.f_Operator,
                f_OperatorTime = p.f_OperatorTime ?? null,
                f_Remark = p.f_Remark,
                f_FileName = p.f_FileName,
                f_DownTips = p.f_DownTips
            }).ToList();
        }
        /// <summary>
        /// 查询主管所在部门的所有证件信息
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        public List<CertAgencyDto> GetAgencyBySector(int eid)
        {
            Employee emp = _employee.GetAll().FirstOrDefault();
            CertAgency cert = _repository.GetAll().FirstOrDefault();
            List<CertAgencyDto> list = new List<CertAgencyDto>();
            if (emp != null)
            {
                var progress = _stype.GetAll().FirstOrDefault(u => u.f_tID == cert.f_ProgressId).f_value;
                var Certificates = _stype.GetAll().FirstOrDefault(u => u.f_tID == cert.f_CertificateTypeId).f_value;
                var locations = _stype.GetAll().FirstOrDefault(u => u.f_tID == emp.f_workLocation_tID).f_value;
                var department = _stype.GetAll().FirstOrDefault(u => u.f_tID == emp.f_department_tID).f_value;
                list = (from a in _repository.GetAll()
                        join b in _employee.GetAll() on a.f_eId equals b.f_eid
                        where b.f_department_tID == emp.f_department_tID
                        orderby a.f_OperatorTime
                        select new CertAgencyDto
                        {
                            f_Id = a.f_Id,
                            Eid = a.f_eId ?? 0,
                            Nickname = b.f_nickname,
                            Name = b.f_chineseName,
                            WorkLocation = locations,
                            Sector = department,
                            f_CertificateType = Certificates,
                            f_Progress = progress,
                            f_Operator = a.f_Operator,
                            f_OperatorTime = a.f_OperatorTime ?? null,
                            f_Remark = a.f_Remark,
                            f_FileName = a.f_FileName,
                            f_DownTips = a.f_DownTips
                        }).ToList();
            }
            return list;
        }

        /// <summary>
        /// 根据ID获取一条数据
        /// </summary>
        /// <param name="id">查询参数</param>
        /// <returns></returns>
        public CertAgency GetCertAgencyById(int id)
        {
            return _repository.GetEntityById(id);
        }

        /// <summary>
        /// 根据条件获得证件办理数据
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">页大小</param>
        /// <param name="o_Count">总数</param>
        /// <returns></returns>
        public IList<CertAgency> GetCertAgencyByWhere(CertAgencyWhereDto caw, int iPageIndex, int iPageSize, out int o_Count)
        {
            var linq = _repository.GetAll().OrderByDescending(u => u.f_Id);
            if (!string.IsNullOrWhiteSpace(caw.Name))
            {
                linq = _repository.GetAll().Join(_employee.GetAll().Where(u => u.f_chineseName.Contains(caw.Name.Trim())), p => p.f_eId.Value, r => r.f_eid, (p, r) => p).OrderByDescending(p => p.f_Id);
            }
            if (!(caw.FDepartmentId == null || caw.FDepartmentId == 0))
            {
                linq = linq.Join(_employee.GetAll().Where(u => u.f_department_tID == caw.FDepartmentId), p => p.f_eId.Value, r => r.f_eid, (p, r) => p).OrderByDescending(p => p.f_Id);
            }
            if (!(caw.WorkLocationId == null || caw.WorkLocationId == -1))
            {
                linq = linq.Join(_employee.GetAll().Where(u => u.f_workLocation_tID == caw.WorkLocationId), p => p.f_eId.Value, r => r.f_eid, (p, r) => p).OrderByDescending(p => p.f_Id);
            }
            o_Count = linq.Count();
            return linq.Skip((iPageIndex - 1) * iPageSize).Take(iPageSize).ToList();
        }


    }
}
