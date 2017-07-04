using MI.Application.OASession;
using MI.Core.Domain;
using MI.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application
{
    /// <summary>
    /// 新人订餐表
    /// </summary>
    public class NewOrderingEmployeesService : INewOrderingEmployeesService
    {
        /// <summary>
        /// 新人订餐
        /// </summary>
        private readonly IBaseRepository<NewOrderingEmployees> _NewOrderEmp;


        private readonly IBaseRepository<WorkDistribution> _workdbtn;

        /// <summary>
        /// 人员表
        /// </summary>
        private readonly IBaseRepository<Employee> _emoloyee;
        /// <summary>
        /// 类型表
        /// </summary>
        private readonly IBaseRepository<SType> _stype;
        /// <summary>
        /// 工作交接表
        /// </summary>
        private readonly IBaseRepository<WorkDistribution> _workdistribution;
        private readonly ISession _session;
        public NewOrderingEmployeesService(IBaseRepository<NewOrderingEmployees> NewOrderEmp, IBaseRepository<Employee> employee, IBaseRepository<SType> stype, IBaseRepository<WorkDistribution> workdbtn, IBaseRepository<WorkDistribution> workdistribution, ISession session)
        {
            _NewOrderEmp = NewOrderEmp;
            _emoloyee = employee;
            _stype = stype;
            _workdbtn = workdbtn;
            _workdistribution = workdistribution;
            _session = session;
        }

        public void AllClear(int eId)
        {
            if (eId != 0)
            {
                //删除新人订餐记录
                if (_NewOrderEmp.GetEntityById(eId)!=null)
                {
                    _NewOrderEmp.Delete(_NewOrderEmp.GetEntityById(eId));
                }
                if (_emoloyee.GetEntityById(eId)!=null)
                {
                    //更新工作记录
                    UpdateWorkDistribution(_emoloyee.GetEntityById(eId));
                }
            }
        }

        /// <summary>
        /// 更新新人订餐记录
        /// </summary>
        /// <param name="listModel"></param>
        public void UpdateOrderingEmployees(List<NewOrderingEmployees> listModel)
        {
            var iEId = listModel.FirstOrDefault()?.f_eID;
            if (iEId != null)
            {
                var oldListModel = _NewOrderEmp.GetEntityById(iEId.Value); //获取数据库中的订餐信息
                _NewOrderEmp.Delete(oldListModel);
                foreach (var item in listModel)
                {
                    _NewOrderEmp.Insert(_NewOrderEmp.GetEntityById((int)item.f_eID));
                }
            }
            UpdateWorkDistribution(_emoloyee.GetEntityById(iEId.Value));
        }
        /// <summary>
        /// 新人登記未訂餐時，添加工作交接記錄
        /// <param name="employee">員工表</param>
        /// </summary>
        public void UpdateWorkDistribution(Employee employee)
        {
            //無到達時間或小於當前時間，直接返回
            if (employee.NewEmployee?.f_flightArrivalTime == null)
            {
                return;
            }
            string tips = employee.NewEmployee.f_flightArrivalTime?.ToString("yyyy/MM/dd") + "新人餐尚有員工未設定，請於17: 00前訂餐";
            int iWorkType = _stype.GetAll().Where(u => u.f_type == (int)sTypeEnum.工作类别 && "新人订餐管理".Equals(u.f_value)).FirstOrDefault()?.f_tID ?? 0;
            var notice = _workdistribution.GetAll().FirstOrDefault(u => u.f_WorkType == iWorkType && employee.f_eid.ToString().Equals(u.f_Remarks));
            //有到達時間，無工作交接記錄
            if (notice == null)
            {
                //為訂餐，新增工作交接記錄
                if (employee.WorkDistribution == null || employee.WorkDistribution.Count == 0)
                {
                    WorkDistribution work = new WorkDistribution();
                    work.f_RegisterDate = DateTime.Now;
                    work.f_Registered = _session.GetCurrUser().NickName;
                    work.f_WorkType = iWorkType;
                    work.f_UrgentDate = employee.NewEmployee.f_flightArrivalTime;
                    work.f_Handover = tips;
                    work.f_IsComplete = false;
                    work.f_Remarks = employee.f_eid.ToString();
                    _workdistribution.Insert(work);
                }
            }
            else
            {
                //已订餐，刪除記錄
                if (employee.NewEmployee != null)
                {
                    _workdistribution.Delete(notice);
                }
                else
                {
                    notice.f_Registered = _session.GetCurrUser().NickName;
                    notice.f_UrgentDate = employee.NewEmployee.f_flightArrivalTime;
                    notice.f_Handover = tips;
                    notice.f_IsComplete = false;
                    notice.f_Remarks = employee.f_eid.ToString();
                    _workdistribution.Insert(notice);
                    WorkDistribution workbution = new WorkDistribution();
                }
            }
        }
        /// <summary>
        /// 根据员工id删除订餐记录
        /// </summary>
        /// <param name="eId"></param>
        public void DeleteAllClear(int eId)
        {
            _NewOrderEmp.Delete(_NewOrderEmp.GetEntityById(eId));
            UpdateWorkDistribution(_emoloyee.GetEntityById(eId));
        }

    }
}
