using MI.Application.Dto;
using MI.Core.Common;
using MI.Core.Domain;
using MI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.BedLinenService
{
    public class BedLinenService : BaseService<BedLinen>, IBedLinenService
    {
        private readonly IBaseRepository<BedLinen> _repository;
        private readonly IBaseRepository<Employee> _employee;
        private readonly IBaseRepository<SType> _stype;
        private readonly ISTypeService _stypeservice;
        public readonly IBaseRepository<t_Dormitory> _Dormitory;
        public BedLinenService(IBaseRepository<BedLinen> repository,
            IBaseRepository<Employee> employee,
            ISTypeService stypeservice,
            IBaseRepository<SType> stype,
            IBaseRepository<t_Dormitory> Dormitory) : base(repository)
        {
            _repository = repository;
            _employee = employee;
            _stype = stype;
            _stypeservice = stypeservice;
            _Dormitory = Dormitory;
        }
        /// <summary>
        /// 根据id获取一条数据
        /// </summary>
        /// <param name="iId">主键id</param>
        /// <returns>返回BedLinen实体</returns>
        public BedLinen GetBedLinenById(int iId)
        {
            return _repository.GetEntityById(iId);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="oModel">BedLinen实体</param>
        public ErrorEnum AddBedLinenOneData(BedLinen oModel)
        {
            ErrorEnum result = ErrorEnum.Error;
            if (oModel != null)
            {
                _repository.Insert(oModel);
                result = ErrorEnum.Success;
            }
            else
                result = ErrorEnum.Fail;
            return result;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="oModel">BedLinen实体</param>
        public ErrorEnum EditBedLinenOneData(BedLinen oModel)
        {
            ErrorEnum result = ErrorEnum.Error;
            if (oModel != null)
            {
                _repository.Update(oModel);
                result = ErrorEnum.Success;
            }
            else
                result = ErrorEnum.Fail;
            return result;
        }
        /// <summary>
        /// 根据id删除一条数据
        /// </summary>
        /// <param name="iId">主键id</param>
        public ErrorEnum DeleteBedLinen(int iId)
        {
            ErrorEnum result = ErrorEnum.Error;
            BedLinen model = GetBedLinenById(iId);

            if (model != null)
            {
                _repository.Delete(model);
            }
            return result;
        }
        /// <summary>
        /// 根据员工id查询
        /// </summary>
        /// <param name="iEid">员工id</param>
        /// <returns>返回一个BedLinenViewModel</returns>
        public List<BedLinenDto> GetBedLinenListByEid(int iEid)
        {
            var BedLinenList = _repository.GetEntityById(iEid);
            string stype = "";
            if (BedLinenList != null)
            {
                 stype = _stypeservice.GetsTypeByWhere((int)sTypeEnum.床位类型).FirstOrDefault(u => u.f_tID == BedLinenList.f_tID).f_value;
            }
            List<BedLinenDto> bedlinenList = (from a in _repository.GetAll()
                                              join b in _employee.GetAll() on a.f_eid equals b.f_eid
                                              where b.f_eid == iEid
                                              select new BedLinenDto
                                              {
                                                  Id = a.f_Id,
                                                  Eid = a.f_eid,
                                                  NickName = b.f_nickname,
                                                  RegistrationDate = a.f_RegistrationDate,
                                                  Community = b.t_Dormitory.f_Community,
                                                  Building = b.t_Dormitory.f_Building,
                                                  RoomNo = b.t_Dormitory.f_RoomNo,
                                                  Hostels = stype,
                                                  BunkNo = a.f_BunkNo,
                                                  SheetsNo = a.f_SheetsNo,
                                                  QuiltNo = a.f_QuiltNo,
                                                  PillowcaseNo = a.f_PillowcaseNo,
                                                  Cont = a.f_Cont,
                                                  AddBunkNo = a.f_AddBunkNo,
                                                  AddSheetsNo = a.f_AddSheetsNo,
                                                  AddQuiltNo = a.f_AddQuiltNo,
                                                  AddPillowcaseNo = a.f_AddPillowcaseNo,
                                                  Remarks = a.f_Remarks
                                              }
                    ).ToList();
            return bedlinenList.OrderByDescending(u => u.Id).ToList();
        }
        /// <summary>
        /// 按条件查询所有床单送洗数据
        /// </summary>
        /// <param name="queryModel">条件</param>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">每页多少条数据</param>
        /// <param name="o_Count">总数</param>
        /// <param name="bIstrue">是否需要分页</param>
        /// <returns>返回BedLinenViewModel集合</returns>
        public List<BedLinenViewModel> GetBedLinenAllData(BedLinenWhere queryModel, int iPageIndex, int iPageSize, out int o_Count, bool bIstrue = false)
        {
            List<BedLinenViewModel> bedlinenList;
            //条件查询
            if (queryModel.IsValid)
            {
                bedlinenList = (from a in _repository.GetAll()
                                join b in _employee.GetAll() on a.f_eid equals b.f_eid
                                join c in _stype.GetAll() on a.f_tID equals c.f_tID
                                where
                                    ((queryModel.RegistrationStarDate == null ||
                                      a.f_RegistrationDate >= queryModel.RegistrationStarDate) &&
                                     (queryModel.RegistrationEndDate == null ||
                                      a.f_RegistrationDate <= queryModel.RegistrationEndDate) &&
                                     (string.IsNullOrEmpty(queryModel.Name) || (b.f_nickname.Contains(queryModel.Name)) ||
                                      b.f_chineseName.Contains(queryModel.Name)) &&
                                     (string.IsNullOrEmpty(queryModel.Building) ||
                                      b.t_Dormitory.f_Building.Contains(queryModel.Building)) &&
                                     (string.IsNullOrEmpty(queryModel.Community) ||
                                      b.t_Dormitory.f_Community.Contains(queryModel.Community)) &&
                                     (string.IsNullOrEmpty(queryModel.RoomNo) || b.t_Dormitory.f_RoomNo.Contains(queryModel.RoomNo))
                                        )
                                select new BedLinenViewModel
                                {
                                    Id = a.f_Id,
                                    Eid = a.f_eid,
                                    NickName = b.f_nickname,
                                    RegistrationDate = a.f_RegistrationDate,
                                    Community = b.t_Dormitory.f_Community,
                                    Building = b.t_Dormitory.f_Building,
                                    RoomNo = b.t_Dormitory.f_RoomNo,
                                    Hostels = c.f_value,
                                    BunkNo = a.f_BunkNo,
                                    SheetsNo = a.f_SheetsNo,
                                    QuiltNo = a.f_QuiltNo,
                                    PillowcaseNo = a.f_PillowcaseNo,
                                    Cont = a.f_Cont,
                                    AddBunkNo = a.f_AddBunkNo,
                                    AddSheetsNo = a.f_AddSheetsNo,
                                    AddQuiltNo = a.f_AddQuiltNo,
                                    AddPillowcaseNo = a.f_AddPillowcaseNo,
                                    Remarks = a.f_Remarks
                                }
                    ).ToList();
            }
            else
            {
                bedlinenList = (from a in _repository.GetAll()
                                join b in _employee.GetAll() on a.f_eid equals b.f_eid
                                join c in _stype.GetAll() on a.f_tID equals c.f_tID
                                select new BedLinenViewModel
                                {
                                    Id = a.f_Id,
                                    Eid = a.f_eid,
                                    NickName = b.f_nickname,
                                    RegistrationDate = a.f_RegistrationDate,
                                    Community = b.t_Dormitory.f_Community,
                                    Building = b.t_Dormitory.f_Building,
                                    RoomNo = b.t_Dormitory.f_RoomNo,
                                    Hostels = c.f_value,
                                    BunkNo = a.f_BunkNo,
                                    SheetsNo = a.f_SheetsNo,
                                    QuiltNo = a.f_QuiltNo,
                                    PillowcaseNo = a.f_PillowcaseNo,
                                    Cont = a.f_Cont,
                                    AddBunkNo = a.f_AddBunkNo,
                                    AddSheetsNo = a.f_AddSheetsNo,
                                    AddQuiltNo = a.f_AddQuiltNo,
                                    AddPillowcaseNo = a.f_AddPillowcaseNo,
                                    Remarks = a.f_Remarks
                                }
                    ).ToList();
            }
            o_Count = bedlinenList.Count();
            if (bIstrue)
            {
                return bedlinenList;
            }
            else
            {
                ValidatePagingWhere(bedlinenList.Count, ref iPageIndex, iPageSize);
                return bedlinenList.OrderByDescending(u => u.Id).Skip((iPageIndex - 1) * iPageSize).Take(iPageSize).ToList();
            }
        }
        /// <summary>
        /// 按条件导出所有床单送洗数据
        /// </summary>
        /// <param name="queryModel">条件</param>
        /// <param name="bIsSummary">是否导出汇总的数据</param>
        /// <returns>返回一个匿名集合</returns>
        public List<dynamic> ExportBedLinenAllData(BedLinenWhere queryModel, bool bIsSummary = false)
        {
            return bIsSummary ? getExportSummary(queryModel) : getExport(queryModel);
        }
        /// <summary>
        /// 导出清单 
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        private List<dynamic> getExport(BedLinenWhere queryModel)
        {
            int iIndex = 1, iConut = 0;
            var bedlinenList = GetBedLinenAllData(queryModel, 0, 0, out iConut, true).Select(p => new
            {
                序号 = iIndex++,
                中文名 = _employee.GetEntityById(p.Eid)?.f_chineseName ?? "",
                英文名 = _employee.GetEntityById(p.Eid)?.f_passportName ?? "",
                社区COMMUNITY = p.Community,
                楼栋BUILDING = p.Building,
                房间号ROOM_NUMBER = p.RoomNo,
                床位类型BED_SIZE = p.Hostels,
                床单数量BED_SHEET = p.BunkNo,
                被单数量COMFORTER_SHEET = p.SheetsNo,
                被子数量COMFORTER = p.QuiltNo,
                枕头套数量PILLOW_CASE = p.PillowcaseNo,
                当月送洗次数HOW_MANY_TIMES = p.Cont,
                加送床单数量ADDITIONAL_BED_SHEET = p.AddBunkNo,
                加送被子数量ADDITIONAL_COMFORTER = p.AddSheetsNo,
                加送枕头套数量ADDITIONAL_PILLOW_CASE = p.AddPillowcaseNo,
                备注REMARKS = p.Remarks
            }).ToList<dynamic>();
            return bedlinenList;
        }
        /// <summary>
        /// 导出汇总 
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        private List<dynamic> getExportSummary(BedLinenWhere queryModel)
        {
            int iIndex = 1;
            var bedlinenList = GetStatsBedLinen(queryModel).Select(p => new
            {
                序号 = iIndex++,
                社区COMMUNITY = p.Community,
                楼栋BUILDING = p.Building,
                房间号ROOM_NUMBER = p.RoomNo,
                床位类型BED_SIZE = p.Hostels,
                床单数量BED_SHEET = p.BunkNo,
                被单数量COMFORTER_SHEET = p.SheetsNo,
                被子数量COMFORTER = p.QuiltNo,
                枕头套数量PILLOW_CASE = p.PillowcaseNo,
                当月送洗次数HOW_MANY_TIMES = p.Cont,
                加送床单数量ADDITIONAL_BED_SHEET = p.AddBunkNo,
                加送被子数量ADDITIONAL_COMFORTER = p.AddSheetsNo,
                加送枕头套数量ADDITIONAL_PILLOW_CASE = p.AddPillowcaseNo,
                备注REMARKS = string.Join("\r\n/", p.listRemarks)
            }).ToList<dynamic>();
            return bedlinenList;
        }
        /// <summary>
        /// 根据宿舍获取总的送洗项目数量
        /// </summary>
        /// <param name="queryModel">条件</param>
        /// <returns></returns>
        public List<BedLinenViewModel> GetStatsBedLinen(BedLinenWhere queryModel)
        {
            List<BedLinenViewModel> bedlinenList;
            //条件查询
            if (queryModel.IsValid)
            {
                bedlinenList = (from a in _repository.GetAll()
                                join b in _employee.GetAll() on a.f_eid equals b.f_eid
                                join d in _stype.GetAll() on a.f_tID equals d.f_tID
                                where
                                    ((queryModel.RegistrationStarDate == null ||
                                      a.f_RegistrationDate >= queryModel.RegistrationStarDate) &&
                                     (queryModel.RegistrationEndDate == null ||
                                      a.f_RegistrationDate <= queryModel.RegistrationEndDate) &&
                                     (string.IsNullOrEmpty(queryModel.Name) || (b.f_nickname.Contains(queryModel.Name)) ||
                                      b.f_chineseName.Contains(queryModel.Name)) &&
                                     (string.IsNullOrEmpty(queryModel.Building) ||
                                      b.t_Dormitory.f_Building.Contains(queryModel.Building)) &&
                                     (string.IsNullOrEmpty(queryModel.Community) ||
                                      b.t_Dormitory.f_Community.Contains(queryModel.Community)) &&
                                     (string.IsNullOrEmpty(queryModel.RoomNo) || b.t_Dormitory.f_RoomNo.Contains(queryModel.RoomNo))
                                        )
                                group a by new { b.t_Dormitory.f_Community, b.t_Dormitory.f_Building, b.t_Dormitory.f_RoomNo, a.f_Cont, a.f_tID,d.f_value } into c
                                select new BedLinenViewModel
                                {
                                    Community = c.Key.f_Community,
                                    Building = c.Key.f_Building,
                                    RoomNo = c.Key.f_RoomNo,
                                    BunkNo = c.Sum(u => u.f_BunkNo),
                                    SheetsNo = c.Sum(u => u.f_SheetsNo),
                                    QuiltNo = c.Sum(u => u.f_QuiltNo),
                                    PillowcaseNo = c.Sum(u => u.f_PillowcaseNo),
                                    Cont = c.Key.f_Cont,
                                    AddBunkNo = c.Sum(u => u.f_AddBunkNo),
                                    AddSheetsNo = c.Sum(u => u.f_AddSheetsNo),
                                    AddQuiltNo = c.Sum(u => u.f_AddQuiltNo),
                                    AddPillowcaseNo = c.Sum(u => u.f_AddPillowcaseNo),
                                    Hostels = c.Key.f_value,
                                    listRemarks = c.Select(p => p.f_Remarks).ToList()
                                }
                    ).ToList();
            }
            else
            {
                bedlinenList = (from a in _repository.GetAll()
                                join b in _employee.GetAll() on a.f_eid equals b.f_eid
                                join d in _stype.GetAll() on a.f_tID equals d.f_tID
                                group a by new { b.t_Dormitory.f_Community, b.t_Dormitory.f_Building, b.t_Dormitory.f_RoomNo, a.f_Cont, a.f_tID,d.f_value } into c
                                select new BedLinenViewModel
                                {
                                    Community = c.Key.f_Community,
                                    Building = c.Key.f_Building,
                                    RoomNo = c.Key.f_RoomNo,
                                    BunkNo = c.Sum(u => u.f_BunkNo),
                                    SheetsNo = c.Sum(u => u.f_SheetsNo),
                                    QuiltNo = c.Sum(u => u.f_QuiltNo),
                                    PillowcaseNo = c.Sum(u => u.f_PillowcaseNo),
                                    Cont = c.Key.f_Cont,
                                    AddBunkNo = c.Sum(u => u.f_AddBunkNo),
                                    AddSheetsNo = c.Sum(u => u.f_AddSheetsNo),
                                    AddQuiltNo = c.Sum(u => u.f_AddQuiltNo),
                                    AddPillowcaseNo = c.Sum(u => u.f_AddPillowcaseNo),
                                    Hostels = c.Key.f_value,
                                    listRemarks = c.Select(p => p.f_Remarks).ToList()
                                }
                    ).ToList();
            }
            return bedlinenList.OrderBy(p => p.Community).ThenBy(p => p.Building).ThenBy(p => p.RoomNo).ThenBy(p => p.Cont).ToList();
        }


    }
}
