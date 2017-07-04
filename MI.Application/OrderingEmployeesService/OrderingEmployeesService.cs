using MI.Application.OASession;
using MI.Core.Domain;
using MI.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MI.Application
{
    public class OrderingEmployeesService:IOrderingEmployeesService
    {
        private readonly IBaseRepository<OrderingEmployees> _orderEmp;
        private readonly IBaseRepository<Employee> _employee;
        private readonly IBaseRepository<SType> _sType;
        private readonly IBaseRepository<WorkDistribution> _workdbtn;
        private readonly ISession _session;
        public OrderingEmployeesService(IBaseRepository<OrderingEmployees> orderEmp, IBaseRepository<Employee> employee, IBaseRepository<SType> sType, IBaseRepository<WorkDistribution> workdbtn, ISession session) {
            _orderEmp = orderEmp;
            _employee = employee;
            _sType = sType;
            _workdbtn = workdbtn;
            _session = session;
        }
        public void UpdateOrderingEmployees(List<OrderingEmployees> listModel)
        {
            var iEId = listModel.FirstOrDefault()?.f_eID;
            if (iEId != null)
            {
                var oldListModel = _orderEmp.GetAll().Where(l=>l.f_eID==iEId); //取数据库中的订餐信息
                _orderEmp.Delete(oldListModel);
                for (int i = 0; i < listModel.Count; i++)
                {
                    _orderEmp.Insert(listModel[i]);
                }
            }
            UpdateWorkDistribution(_employee.GetEntityById((int)iEId));
        }
        public void AllClear(int eId)
        {
            OrderingEmployees model = _orderEmp.GetAll().Where(p => p.f_eID == eId) as OrderingEmployees;
          //  UpdateWorkDistribution(_employee.GetEntityById(eId));
        }
        /// <summary>
        /// 新人登記未訂餐時，添加工作交接記錄
        /// <param name="emp">員工實體</param>
        /// </summary>
        public void UpdateWorkDistribution(Employee modelEmployeeInfo)
        {
            //無到達時間或小於當前時間，直接返回
            if (modelEmployeeInfo.NewEmployee?.f_flightArrivalTime == null)
            {
                return;
            }
            string tips = modelEmployeeInfo.NewEmployee?.f_flightArrivalTime?.ToString("yyyy/MM/dd") + "新人餐尚有員工未設定，請於17: 00前訂餐";
            int iWorkType = _sType.GetAll().Where(u => u.f_type == (int)sTypeEnum.工作类别 && "新人订餐管理".Equals(u.f_value)).FirstOrDefault()?.f_tID ?? 0;
            var notice = _workdbtn.GetAll().FirstOrDefault(u => u.f_WorkType == iWorkType && modelEmployeeInfo.f_eid.ToString().Equals(u.f_Remarks));
            //有到達時間，無工作交接記錄
            if (notice == null)
            {
                //為訂餐，新增工作交接記錄
                if (modelEmployeeInfo.OrderingEmployees == null || modelEmployeeInfo.OrderingEmployees?.Count == 0)
                {
                    WorkDistribution work = new WorkDistribution();
                    work.f_RegisterDate = DateTime.Now;
                    work.f_Registered = _session.GetCurrUser().NickName;
                    work.f_WorkType = iWorkType;
                    work.f_UrgentDate = modelEmployeeInfo.NewEmployee?.f_flightArrivalTime;
                    work.f_Handover = tips;
                    work.f_IsComplete = false;
                    work.f_Remarks = modelEmployeeInfo.f_eid.ToString();
                    _workdbtn.Insert(work);
                }
            }
            else
            {
                //已订餐，刪除記錄
                if (modelEmployeeInfo.OrderingEmployees != null && modelEmployeeInfo.OrderingEmployees?.Count != 0)
                {
                    _workdbtn.Delete(notice);
                }
                else
                {
                    notice.f_Registered = _session.GetCurrUser().NickName; 
                    notice.f_UrgentDate = modelEmployeeInfo?.NewEmployee.f_flightArrivalTime;
                    notice.f_Handover = tips;
                    notice.f_IsComplete = false;
                    notice.f_Remarks = modelEmployeeInfo.f_eid.ToString();
                    _workdbtn.Update(notice);
                }
            }
        }
        /// <summary>
        /// 根据id删除订餐记录
        /// </summary>
        /// <param name="eId"></param>
        public void DeleteAllClear(int eId)
        {
            OrderingEmployees order = _orderEmp.GetEntityById(eId);
            if (order != null)
            {
                _orderEmp.Delete(_orderEmp.GetEntityById(eId));
            }
        }
    }
}
