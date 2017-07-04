using MI.Core.Domain;
using MI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application
{
    public class DormitorySummaryService:IDormitorySummaryService
    {
        private readonly IBaseRepository<t_Dormitory> _Dormitory;
        private readonly IBaseRepository<Employee> _Employee;
        private readonly IBaseRepository<SType> _SType;
        private readonly IBaseRepository<t_CoupleRegister> _coupleRegister;
        public DormitorySummaryService(IBaseRepository<t_Dormitory> Dormitory, IBaseRepository<Employee> Employee, IBaseRepository<SType> SType, IBaseRepository<t_CoupleRegister> coupleRegister)
        {
            _Dormitory = Dormitory;
            _Employee = Employee;
            _SType = SType;
            _coupleRegister = coupleRegister;
        }
        public List<DormitorySummaryViewModel> getDormitorySummary(string sCommunity, string sBuilding)
        {
            if (string.IsNullOrWhiteSpace(sCommunity) || string.IsNullOrWhiteSpace(sBuilding))
            {
                return null;
            }
            return _Dormitory.GetAll().ToList().Where(p => p.f_Community.Trim() == sCommunity && p.f_Building.Trim() == sBuilding && p.t_employeeInfo.Any(u => u.STypeWorkStatus != null && "在职,返乡".Contains(u.STypeWorkStatus.f_value)))
                     .Select(p => new DormitorySummaryViewModel
                     {
                         ID = p.f_DormitoryId,
                         Department = (p.f_department_tID == null ? "未定义部门" : _SType.GetAll().FirstOrDefault(s => s.f_tID == p.f_department_tID).f_value),
                         Community = sCommunity,
                         Building = sBuilding,
                         RoomNo = p.f_RoomNo,
                         RoomType = p.f_RoomType,
                         TotalOfPeople = p.f_totalOfPeople,
                         SumPeople = p.t_employeeInfo.Count,
                         listEmployeeInfo = p.t_employeeInfo.Where(u => u.STypeWorkStatus != null && "在职,返乡".Contains(u.STypeWorkStatus.f_value)).ToList(),
                         Remarks = p.f_Remark
                     }).ToList();
        }

        public List<DormitorySummaryViewModel> getDormitorySummary(int iCommunity, int iBuilding)
        {
            var sCommunity = _SType.GetAll().FirstOrDefault(p => p.f_tID == iCommunity && p.f_type == (int)sTypeEnum.社区类型)?.f_value?.Trim();
            var sBuilding = _SType.GetAll().FirstOrDefault(p => p.f_tID == iBuilding && p.f_type == (int)sTypeEnum.楼栋类型)?.f_value?.Trim();
            return getDormitorySummary(sCommunity, sBuilding);
        }

        public int AddCoupleRegister(t_CoupleRegister model)
        {
            int result =_coupleRegister.Insert(model);
            return result;
        }

        public string CheckCoupleRegister(t_CoupleRegister model)
        {
            if (model.f_eId1 == 0 || model.f_eId2 == 0)
            {
                return "请用下拉的方式选择员工";
            }
            if (model.f_eId1 == model.f_eId2)
            {
                return "不能选择自己";
            }
            if (_coupleRegister.GetAll().Any(p => p.f_cId == model.f_cId && p.f_eId1 == model.f_eId1 && p.f_eId2 == model.f_eId2))
            {
                return "未变动";
            }
            if (_coupleRegister.GetAll().Any(p => (p.f_cId != model.f_cId) && (p.f_eId1 == model.f_eId1 || p.f_eId2 == model.f_eId1 || p.f_eId1 == model.f_eId2 || p.f_eId2 == model.f_eId2) && p.f_eId1 != 0 && p.f_eId2 != 0))
            {
                return "已存在";
            }
            if (!_Employee.GetAll().Any(p => p.f_eid == model.f_eId1) || !_Employee.GetAll().Any(p => p.f_eid == model.f_eId1))
            {
                return "用户错误";
            }
            if (_Employee.GetEntityById(model.f_eId1).STypeWorkStatus == null ||
                (!"在职,返乡".Contains(_Employee.GetEntityById(model.f_eId1).STypeWorkStatus.f_value)) ||
                _Employee.GetEntityById(model.f_eId2).STypeWorkStatus == null ||
                (!"在职,返乡".Contains(_Employee.GetEntityById(model.f_eId2).STypeWorkStatus.f_value)))
            {
                return "选定的夫妻双方有一个在职状态不为在职或返乡";
            }
            return "OK";
        }

        public List<t_CoupleRegister> GetCoupleRegister()
        {
            return (from cp in _coupleRegister.GetAll()
                    join e in _Employee.GetAll() on
                    cp.f_eId1 equals e.f_eid
                    join e2 in _Employee.GetAll() on
                    cp.f_eId2 equals e2.f_eid
                    where (e.STypeWorkStatus != null && "在职,返乡".Contains(e.STypeWorkStatus.f_value) && e2.STypeWorkStatus != null && "在职,返乡".Contains(e2.STypeWorkStatus.f_value))
                    orderby cp.f_cId descending
                    select cp).ToList();
        }

        /// <summary>
        /// 删除夫妻双方有一个不在职的夫妻关系
        /// </summary>
        public void DeleteCoupleNotServ()
        {

            List<t_CoupleRegister> oList = (from cp in _coupleRegister.GetAll()
                                            join e in _Employee.GetAll() on
                                                cp.f_eId1 equals e.f_eid
                                            join e2 in _Employee.GetAll() on
                                                cp.f_eId2 equals e2.f_eid
                                            where
                                                (e.STypeWorkStatus == null || (!"在职,返乡".Contains(e.STypeWorkStatus.f_value)) || e2.STypeWorkStatus == null ||
                                                 (!"在职,返乡".Contains(e2.STypeWorkStatus.f_value)))
                                            select cp).ToList();
            if (oList.Any())
            {
                for (int i = 0; i < oList.Count; i++)
                {
                    _coupleRegister.Delete(oList[i]);
                }
            }
        }
        public List<CoupLeManageView> GetCoupleRegister(string keyWords, int iPageIndex, int iPageSize, out int o_Count)
        {
            if (string.IsNullOrWhiteSpace(keyWords))
            {
                var linq = (from cp in _coupleRegister.GetAll()
                            join e in _Employee.GetAll() on
                            cp.f_eId1 equals e.f_eid
                            join e2 in _Employee.GetAll() on
                            cp.f_eId2 equals e2.f_eid
                            where (e.STypeWorkStatus != null && "在职,返乡".Contains(e.STypeWorkStatus.f_value) && e2.STypeWorkStatus != null && "在职,返乡".Contains(e2.STypeWorkStatus.f_value))
                            orderby cp.f_cId descending
                            select new CoupLeManageView
                            {
                                iCid = cp.f_cId,
                                ChineseName1 = e.f_chineseName,
                                iEid1 = cp.f_eId1,
                                ChineseName2 = e2.f_chineseName,
                                iEid2 = cp.f_eId2
                            });
                o_Count = linq.Count();
                ValidatePagingWhere(linq.Count(), ref iPageIndex, iPageSize);
                return linq.Skip((iPageIndex - 1) * iPageSize).Take(iPageSize).ToList();

            }
            int[] eid = _Employee.GetAll().Where(
                p =>
                    p.f_nickname.Contains(keyWords) || p.f_passportName.Contains(keyWords) ||
                    p.f_chineseName.Contains(keyWords)).Select(s => s.f_eid).ToArray();
            var listCP = _coupleRegister.GetAll().Where(p => eid.Contains(p.f_eId1) || eid.Contains(p.f_eId2));
            var linq2 = (from cp in listCP
                         join e in _Employee.GetAll() on
                         cp.f_eId1 equals e.f_eid
                         join e2 in _Employee.GetAll() on
                         cp.f_eId2 equals e2.f_eid
                         where (e.STypeWorkStatus != null && "在职,返乡".Contains(e.STypeWorkStatus.f_value) && e2.STypeWorkStatus != null && "在职,返乡".Contains(e2.STypeWorkStatus.f_value))
                         orderby cp.f_cId descending
                         select new CoupLeManageView
                         {
                             iCid = cp.f_cId,
                             ChineseName1 = e.f_chineseName,
                             ChineseName2 = e2.f_chineseName,
                         });
            o_Count = linq2.Count();
            ValidatePagingWhere(linq2.Count(), ref iPageIndex, iPageSize);
            return linq2.Skip((iPageIndex - 1) * iPageSize).Take(iPageSize).ToList();
        }

        public int UpdateCoupleRegister(t_CoupleRegister model)
        {
            if (!_coupleRegister.GetAll().Any(p => p.f_cId == model.f_cId)) return 0;
            t_CoupleRegister old = _coupleRegister.GetEntityById(model.f_cId);
            if (old.f_eId1 == model.f_eId1 && old.f_eId2 == model.f_eId2)
            {
                return 0; //未改动
            }
            old.f_eId1 = model.f_eId1;
            old.f_eId2 = model.f_eId2;
            int result= Update(old);
            return result;
        }

        public int DeleteCoupleRegister(int iCid)
        {
            if (!_coupleRegister.GetAll().Any(p => p.f_cId == iCid))
            {
                return 0;
            }
            t_CoupleRegister model = _coupleRegister.GetEntityById(iCid);
            int result = _coupleRegister.Delete(model);
            if (result > 0)
            {
                return result;
            }
            else {
                return 0;
            }
        }
        //私有方法
        private int Update(t_CoupleRegister model)
        {
           int result = _coupleRegister.Update(model);
            return result;
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
    }
}
