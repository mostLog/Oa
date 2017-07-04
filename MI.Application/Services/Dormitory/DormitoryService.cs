using MI.Core;
using MI.Core.Domain;
using MI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MI.Application
{
    public class DormitoryService : IDormitoryService
    {
        private readonly IBaseRepository<Dormitory> _Dormitory;
        private readonly IBaseRepository<Tariff> _Tariff;
        private readonly IBaseRepository<HostelClean> _HostelClean;
        private readonly IBaseRepository<DormitoryMaintenance> _DormitoryMaintenance;
        private readonly IBaseRepository<Employee> _Employee;
        private readonly IBaseRepository<LaundryPwd> _LaundryPwd;
        private readonly IBaseRepository<SType> _SType;
        public DormitoryService(IBaseRepository<Dormitory> Dormitory, IBaseRepository<Tariff> Tariff, IBaseRepository<HostelClean> HostelClean, IBaseRepository<DormitoryMaintenance> DormitoryMaintenance, IBaseRepository<Employee> Employee, IBaseRepository<LaundryPwd> LaundryPwd, IBaseRepository<SType> SType)
        {
            _Dormitory = Dormitory;
            _Tariff = Tariff;
            _HostelClean = HostelClean;
            _DormitoryMaintenance = DormitoryMaintenance;
            _Employee = Employee;
            _LaundryPwd = LaundryPwd;
            _SType = SType;
        }
        /// <summary>
        /// 查询宿舍的方法
        /// </summary>
        /// <returns></returns>
        public IList<Dormitory> GetConditionByWhere(int pageIndex, int pageSize, out int count)
        {
            var linq = _Dormitory.GetAll().OrderBy(u => u.f_Community).ThenBy(u => u.f_Building).ThenBy(u => u.f_RoomNo);
            count = linq.Count();
            ValidatePagingWhere(count, ref pageIndex, pageSize);
            List<Dormitory> oDolist = linq.OrderByDescending(u => u.f_DormitoryId).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            List<LaundryPwd> oPwdList = _LaundryPwd.GetAll().ToList();
            foreach (var item in oDolist)
            {
                int fcount = 0;
                for (int i = 0; i < oPwdList.Count; i++)
                {
                    if (item.f_LaundryAndPwd == "" || item.f_LaundryAndPwd == "0")
                    {
                        item.f_LaundryAndPwd = "";
                        break;
                    }
                    else if (item.f_LaundryAndPwd == oPwdList[i].f_Id.ToString())
                    {
                        item.f_LaundryAndPwd = oPwdList[i].f_NoAndPwd.ToString();
                        fcount = 0;
                    }
                    fcount++;
                }
                if (fcount == oPwdList.Count && item.f_LaundryAndPwd != "" && item.f_LaundryAndPwd != "0")
                {
                    item.f_LaundryAndPwd = "";
                }
            }
            return oDolist;
        }
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Dormitory> GetConditionByWhere(Func<Dormitory, bool> predicate, int pageIndex, int pageSize, out int count)
        {
            var linq = _Dormitory.GetAll().Where(predicate).OrderBy(u => u.f_Community).ThenBy(u => u.f_Building).ThenBy(u => ConvertRoomNoNumber(u.f_RoomNo));
            count = linq.Count();
            ValidatePagingWhere(count, ref pageIndex, pageSize);
            List<Dormitory> oDolist = linq.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            List<LaundryPwd> oPwdList = _LaundryPwd.GetAll().ToList();
            foreach (var item in oDolist)
            {
                int fcount = 0;
                for (int i = 0; i < oPwdList.Count; i++)
                {
                    if (item.f_LaundryAndPwd == "" || item.f_LaundryAndPwd == "0")
                    {
                        item.f_LaundryAndPwd = "";
                        break;
                    }
                    else if (item.f_LaundryAndPwd == oPwdList[i].f_Id.ToString())
                    {
                        item.f_LaundryAndPwd = oPwdList[i].f_NoAndPwd.ToString();
                        fcount = 0;
                    }
                    fcount++;
                }
                if (fcount == oPwdList.Count && item.f_LaundryAndPwd != "" && item.f_LaundryAndPwd != "0")
                {
                    item.f_LaundryAndPwd = "";
                }
            }
            return oDolist;
        }
        /// <summary>
        /// 把房间号中的字母转成数字
        /// </summary>
        /// <param name="sRoomNo"></param>
        /// <returns></returns>
        public int ConvertRoomNoNumber(string sRoomNo)
        {
            string number = Regex.Replace(sRoomNo, "[a-z]", "", RegexOptions.IgnoreCase);
            string sLetter = Regex.Replace(sRoomNo, "[0-9]", "", RegexOptions.IgnoreCase);
            if (sLetter != "")
            {
                byte[] array = new byte[1];   //定义一组数组array
                array = System.Text.Encoding.ASCII.GetBytes(sLetter); //string转换的字母
                string asciicode = ((array[0]) - 64).ToString();
                return Convert.ToInt32(number + (asciicode.Length == 1 ? 0 + asciicode : asciicode));
            }
            return Convert.ToInt32(number);
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
        /// 通过id获取楼栋信息
        /// 
        /// 小白-2017-6-13
        /// </summary>
        /// <param name="id"></param>
        public Dormitory GetDormitoryById(int id)
        {
            return _Dormitory.GetEntityById(id);
        }
        /// <summary>
        /// 分组查询所有社区
        /// </summary>
        /// <param name="iswhere">是否只查包含洗衣房晾衣房的</param>
        /// <returns></returns>
        public List<string> GetTariffbyCommunity(bool iswhere = false)
        {
            List<string> list = new List<string>();
            list = _Dormitory.GetAll().GroupBy(u => u.f_Community).Select(m => m.Key).ToList();
            return list;
        }
        /// <summary>
        /// 根据社区查询楼栋
        /// </summary>
        /// <param name="community">社区</param>
        /// <param name="iswhere">是否只查包含洗衣房晾衣房的</param>
        /// <returns></returns>
        public List<string> GetTariffbyCommunity(string community, bool iswhere = false)
        {
            List<string> list;
            list = _Dormitory.GetAll().Where(u => u.f_Community == community).GroupBy(u => u.f_Building).Select(m => m.Key).ToList();
            return list;
        }
        /// <summary>
        /// 根据社区，楼栋查询社区的所有房间号
        /// </summary>
        /// <param name="community">社区</param>
        /// <param name="building">楼栋</param>
        /// <param name="iswhere">是否只查包含洗衣房晾衣房的</param>
        /// <returns></returns>
        public List<string> GetTariffbyBuilding(string community, string building, bool iswhere = false)
        {
            List<string> list;
            list = _Dormitory.GetAll().Where(u => u.f_Community == community && u.f_Building == building).GroupBy(u => u.f_RoomNo).Select(m => m.Key).ToList();
            return list;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">model实体</param>
        public void EditDormitoryOneData(Dormitory model)
        {
            _Dormitory.Update(model);
        }
        /// <summary>
        /// 根据社区,楼栋,房间号查询房间id
        /// </summary>
        /// <param name="community">社区</param>
        /// <param name="building">楼栋</param>
        /// <param name="roomno">房间号</param>
        /// <returns></returns>
        public Dormitory GetTariffbyRoomNo(string community, string building, string roomno)
        {
            return _Dormitory.GetAll().FirstOrDefault(u => u.f_Community == community && u.f_Building == building && u.f_RoomNo == roomno);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">model实体</param>
        public void AddDormitoryOneData(Dormitory model)
        {
            _Dormitory.Insert(model);
        }
        /// <summary>
        /// 根据id删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        public void DeleteDormitory(int id)
        {
            int stateError = 0;
            Dormitory model = GetDormitoryById(id);
            if (_Tariff.GetAll().Where(p => p.f_DormitoryId == id).Count() > 0 || _HostelClean.GetAll().Where(p => p.f_DormitoryId == id).Count() > 0 ||
                _DormitoryMaintenance.GetAll().Where(p => p.f_DormitoryId == id).Count() > 0 || _Employee.GetAll().Where(p => p.f_dormitoryId == id).Count() > 0)
            {
                
            }
            if (model != null)
            {
                _Dormitory.Delete(model);
            }
            
        }
        /// <summary>
        /// 根据条件查询 导出EXCEL
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<dynamic> GetConditionByWhereExportData(Func<Dormitory, bool> predicate)
        {
            IEnumerable<Dormitory> dormitory1 = _Dormitory.GetAll().Where(predicate).OrderBy(u => u.f_Community).ThenBy(u => u.f_Building).ThenBy(u => u.f_RoomNo);
            List<LaundryPwd> rAndPwd = _LaundryPwd.GetAll().ToList();
            IEnumerable<dynamic> dormitory = from p in dormitory1.ToList()
                                             select new
                                             {
                                                 f_DormitoryId = p.f_DormitoryId,
                                                 f_Community = p.f_Community,
                                                 f_Building = p.f_Building,
                                                 f_RoomNo = p.f_RoomNo,
                                                 f_ContractDate = p.f_ContractDate,
                                                 f_Term = p.f_Term,
                                                 f_DueDate = p.f_DueDate,
                                                 f_Landlady = p.f_Landlady,
                                                 f_Rent = p.f_Rent,
                                                 f_IsBerth = p.f_IsBerth,
                                                 f_erthNo = p.f_erthNo,
                                                 f_RoomType = p.f_RoomType,
                                                 f_LaundryAndPwd = rAndPwd.Where(s => s.f_Id.ToString().Equals(p.f_LaundryAndPwd)).ToList().FirstOrDefault()?.f_NoAndPwd ?? "",
                                                 f_ClothesAndPwd = rAndPwd.Where(s => s.f_Id.ToString().Equals(p.f_ClothesAndPwd)).ToList().FirstOrDefault()?.f_NoAndPwd ?? "",
                                                 f_Remark = p.f_Remark,
                                                 f_operator = p.f_operator,
                                                 f_operatorTime = p.f_operatorTime,
                                                 f_department_tID = _SType.GetAll().FirstOrDefault(d => d.f_tID == p.f_department_tID)?.f_value,
                                                 f_totalOfPeople = p.f_totalOfPeople,
                                                 f_DoublesBed = p.f_DoublesBed
                                             };
            List<dynamic> list = dormitory.ToList();
            return list;
        }
        /// <summary>
        /// 查询所有 导出EXCEL
        /// </summary>
        /// <returns></returns>
        public List<dynamic> GetDormitoryAllExportData()
        {
            IEnumerable<Dormitory> dormitory1 = _Dormitory.GetAll().OrderBy(u => u.f_Community).ThenBy(u => u.f_Building).ThenBy(u => u.f_RoomNo);
            List<LaundryPwd> rAndPwd = _LaundryPwd.GetAll().ToList();
            IEnumerable<dynamic> dormitory = from p in dormitory1.ToList()
                                             select new
                                             {
                                                 f_DormitoryId = p.f_DormitoryId,
                                                 f_Community = p.f_Community,
                                                 f_Building = p.f_Building,
                                                 f_RoomNo = p.f_RoomNo,
                                                 f_ContractDate = p.f_ContractDate,
                                                 f_Term = p.f_Term,
                                                 f_DueDate = p.f_DueDate,
                                                 f_Landlady = p.f_Landlady,
                                                 f_Rent = p.f_Rent,
                                                 f_IsBerth = p.f_IsBerth,
                                                 f_erthNo = p.f_erthNo,
                                                 f_RoomType = p.f_RoomType,
                                                 f_LaundryAndPwd = rAndPwd.Where(s => s.f_Id.ToString().Equals(p.f_LaundryAndPwd)).ToList().FirstOrDefault()?.f_NoAndPwd ?? "",
                                                 f_ClothesAndPwd = rAndPwd.Where(s => s.f_Id.ToString().Equals(p.f_ClothesAndPwd)).ToList().FirstOrDefault()?.f_NoAndPwd ?? "",
                                                 f_Remark = p.f_Remark,
                                                 f_operator = p.f_operator,
                                                 f_operatorTime = p.f_operatorTime,
                                                 f_department_tID = _SType.GetAll().FirstOrDefault(d => d.f_tID == p.f_department_tID)?.f_value,
                                                 f_totalOfPeople = p.f_totalOfPeople,
                                                 f_DoublesBed = p.f_DoublesBed
                                             };
            List<dynamic> list = dormitory.ToList();
            return list;
        }
    }
}
