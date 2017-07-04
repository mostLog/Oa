using MI.Application.ContentServerce.Dto;
using MI.Core.Domain;
using MI.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;


namespace MI.Application
{
    public class LatticeContentService : ILatticeContentService
    {
        /// <summary>
        /// 宿舍登记表
        /// </summary>
        private readonly IBaseRepository<Dormitory> _repository;
        /// <summary>
        /// 宿舍格子表
        /// </summary>
        private readonly IBaseRepository<LatticeContent> _repcontent;
        /// <summary>
        /// 类型表
        /// </summary>
        private readonly IBaseRepository<SType> _repSType;
        /// <summary>
        /// 人员表
        /// </summary>
        private readonly IBaseRepository<Employee> _repEmployee;
        private readonly IDbContext _IDc;

        public LatticeContentService(IBaseRepository<Dormitory> employeeRepository, IBaseRepository<LatticeContent> emplatticeRepository, IBaseRepository<SType> repSType, IBaseRepository<Employee> repEmployee, IDbContext IDc)
        {
            _repository = employeeRepository;
            _repcontent = emplatticeRepository;
            _repSType = repSType;
            _repEmployee = repEmployee;
            _IDc = IDc;
        }
        /// <summary>
        /// 社区
        /// </summary>
        /// <param name="iswhere"></param>
        /// <returns></returns>
        public List<string> GetCommunity(bool iswhere = false)
        {
            // 社区
            List<string> list;
            list = _repository.GetAll().Select(f => f.f_Community).Distinct().ToList();
            return list;
        }
        /// <summary>
        /// 楼栋
        /// </summary>
        /// <param name="community">社区</param>
        /// <param name="iswhere"></param>
        /// <returns></returns>
        public List<string> GetFloorDong(string community, bool iswhere = false)
        {
            List<string> list;
            list = _repository.GetAll().Where(j => j.f_Community == community).Select(c => c.f_Building).Distinct().ToList();
            return list;
        }
        /// <summary>
        /// 获取楼栋与社区id
        /// </summary>
        /// <param name="sTid">楼栋名字</param>
        /// <param name="sTid2">社区名字</param>
        /// <returns></returns>
        public List<MI.Application.ContentServerce.Dto.LatticeContentDto> GetFloorDongCommunityID(string sFloorDongName, string sCommunityName)
        {
            var floorDongID = _repSType.GetAll().FirstOrDefault(p => p.f_value == sFloorDongName && p.f_type == (int)sTypeEnum.楼栋类型);
            var communityID = _repSType.GetAll().FirstOrDefault(p => p.f_value == sCommunityName && p.f_type == (int)sTypeEnum.社区类型);
            if (floorDongID != null && communityID != null)
            {
                return GetFloorDongCommunityData(floorDongID.f_tID, communityID.f_tID);
            }
            return null;

        }
        /// <summary>
        /// 获取楼栋社区数据
        /// </summary>
        /// <param name="floorDongID">楼栋id</param>
        /// <param name="communityID">社区id</param>
        /// <returns></returns>
        public List<LatticeContentDto> GetFloorDongCommunityData(int floorDongID, int communityID)
        {

            var linqFloorDongCommunityData = from lc in _repcontent.GetAll().Where(p => p.f_tID == floorDongID && p.f_tID2 == communityID)
                                                 //宿舍登记表ID与宿舍格子表ID匹配
                                             join d in _repository.GetAll() on lc.f_dormitoryId equals d.f_DormitoryId into dy
                                             from dormitory in dy.DefaultIfEmpty()
                                             orderby lc.f_floor descending, lc.f_room ascending
                                             select new LatticeContentDto
                                             {
                                                 f_LId = lc.f_LId,
                                                 f_tID = lc.f_tID.Value,
                                                 f_floor = lc.f_floor.Value,
                                                 f_room = lc.f_room.Value,
                                                 f_dormitoryId = lc.f_dormitoryId,
                                                 f_isUnlock = lc.f_isUnlock.Value,
                                                 f_liveFewPeople = dormitory.f_totalOfPeople ?? 2,
                                                 //人员表取人数
                                                 f_sumPeople = dormitory.t_employeeInfo.Count,
                                                 f_roomNo = dormitory.f_RoomNo,
                                                 f_roomType = dormitory.f_RoomType ?? "暂无",
                                                 listEmployee = dormitory.t_employeeInfo,
                                                 f_department_tID = dormitory.f_department_tID ?? 0,
                                                 f_DoublesBed = dormitory.f_DoublesBed ?? 0,
                                                 department = dormitory.f_department_tID.Value.ToString()
                                                 //department = lstRes.FirstOrDefault(p => p.f_tID == dormitory.f_department_tID).f_value,
                                             };
            var f = linqFloorDongCommunityData.ToList().Select(p =>
            {
                LatticeContentDto l = p;
                if (!string.IsNullOrEmpty(l.department))
                {
                    int i = int.Parse(l.department);
                    SType s = _repSType.GetAll().FirstOrDefault(ss => ss.f_tID == i);
                    l.department = s?.f_value;
                }
                return l;
            }).ToList();
            return f;
        }
        /// <summary>
        /// 获取部门
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        public List<SType> GetSectorName(Func<SType, bool> predicate)
        {
            return _repSType.GetAll().Where(predicate).OrderByDescending(j => j.f_tID).ToList();
        }
        /// <summary>
        /// 获取楼栋规模
        /// </summary>
        /// <param name="sFloorDong"></param>
        /// <returns></returns>
        public SType GetFloorScale(string sFloorDong)
        {
            return _repSType.GetAll().FirstOrDefault(p => p.f_value.ToUpper() == sFloorDong.ToUpper() && p.f_type == (int)sTypeEnum.楼栋类型);
        }
        /// <summary>
        /// 一键解锁
        /// </summary>
        /// <param name="sBuilding">楼栋</param>
        /// <param name="sCommunity">社区</param>
        /// <returns></returns>
        public string AKeyUnlockDormitory(string sBuilding, string sCommunity)
        {
            return _IDc.aKeyUnlockDormitory(sBuilding, sCommunity).ToList()[0];
        }
        /// <summary>
        /// 根据条件查询类型
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>返回一个SType的list集合</returns>
        public List<SType> SelectTpBypre(Func<SType, bool> predicate)
        {
            return _repSType.GetAll().Where(predicate).ToList();
        }
        /// <summary>
        /// 通过楼栋value获取楼栋
        /// </summary>
        /// <param name="sBuilding">楼栋value</param>
        /// <returns></returns>
        public SType GetLoudDogn(string sBuilding)
        {
            return _repSType.GetAll().FirstOrDefault(p => p.f_value.ToUpper() == sBuilding.ToUpper() && p.f_type == (int)sTypeEnum.楼栋类型);
        }
        /// <summary>
        ///通过社区value获取社区
        /// </summary>
        /// <param name="sCommunity">社区value</param>
        /// <returns></returns>
        public SType GetCommunityByValue(string sCommunity)
        {
            return _repSType.GetAll().FirstOrDefault(p => p.f_value.ToUpper() == sCommunity.ToUpper() && p.f_type == (int)sTypeEnum.社区类型);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sType"></param>
        public void Update(SType sType)
        {
            var model = _repSType.GetEntityById(sType.f_tID);
            if (model != null)
            {
                model.f_value = sType.f_value;
                model.f_Remark = sType.f_Remark;
                _repSType.Update(model);
            }
        }
        /// <summary>
        /// 生成格子数据
        /// </summary>
        /// <param name="iTid"></param>
        /// <param name="iTid2"></param>
        /// <param name="iWidth"></param>
        /// <param name="iHigh"></param>
        /// <returns></returns>
        public bool GeneratingGridData(int iTid, int iTid2, int iWidth, int iHigh)
        {
            return _IDc.GenerateGridData(iTid, iTid2, iWidth, iHigh).ToList()[0].Contains("成功");
        }
        //解锁or锁定
        public int SetUnlock(int iLid, bool bVal, out string sClassName)
        {
            sClassName = string.Empty;
            LatticeContent oldLatticeContent =_repcontent.GetEntityById(iLid);
            if (oldLatticeContent != null)
            {
                if (bVal) //解锁   
                {
                    string sBuilding = string.Empty;//楼栋
                    bool isA = false; //房间号是否为字母
                    var modelBuilding = _repSType.GetEntityById((int)oldLatticeContent.f_tID);
                    if (modelBuilding != null && !string.IsNullOrWhiteSpace(modelBuilding.f_Remark))
                    {
                        sBuilding = modelBuilding.f_value;
                        string[] arr = modelBuilding.f_Remark.Split('*');
                        if (arr.Length == 3)
                        {
                            bool.TryParse(arr[2], out isA);
                        }
                    }
                    int iMaxRoomLen = (_repcontent.GetAll().Where(p => p.f_tID2 == oldLatticeContent.f_tID2 && p.f_tID == oldLatticeContent.f_tID).Max(p => p.f_room) ?? 0).ToString().Length;
                    string sRoom = oldLatticeContent.f_room < 10 ? "0" + oldLatticeContent.f_room : oldLatticeContent.f_room.ToString();
                    while (sRoom.Length < iMaxRoomLen)
                    {
                        sRoom = string.Concat("0", sRoom);
                    }
                    string sCommunity = _repSType.GetEntityById((int)oldLatticeContent.f_tID2)?.f_value; //社区
                    string sRoomNo = string.Concat(oldLatticeContent.f_floor, (isA ? AOUnity.NunberToChar(oldLatticeContent.f_room.Value) : sRoom)).Trim();
                    var dy = _repository.GetAll().OrderByDescending(p => p.f_DormitoryId).FirstOrDefault(p => p.f_Community == (sCommunity) && p.f_Building == (sBuilding) && p.f_RoomNo == (sRoomNo));
                    if (dy != null)
                    {
                        int iTotalOfPeople = (dy.f_totalOfPeople ?? 2); //如果没有设置 默认一个房间住2人
                        sClassName += sClassName == "" && dy.t_employeeInfo.Count == 0 ? "btn-success" : "";  // 表示已经绑定宿舍表ID 但是还没有住人
                        sClassName += sClassName == "" && dy.t_employeeInfo.Count < iTotalOfPeople ? "btn-info" : "";  // 表示已经绑定宿舍表ID 但是没住满
                        sClassName += sClassName == "" && dy.t_employeeInfo.Count == iTotalOfPeople ? "btn-danger" : "";  // 表示已经绑定宿舍表ID 已经刚刚住满
                        sClassName += sClassName == "" && dy.t_employeeInfo.Count > iTotalOfPeople ? "btn-warning" : "";  // 表示已经绑定宿舍表ID 已经住满 溢出

                        oldLatticeContent.f_dormitoryId = dy.f_DormitoryId;
                        oldLatticeContent.f_isUnlock = true;
                        _repcontent.Update(oldLatticeContent);
                        return 1;
                    }
                    else
                    {
                        return 402;
                    }
                }
                else //锁定
                {
                    var dy = _repository.GetEntityById((int)oldLatticeContent.f_dormitoryId);
                    if (dy != null && dy.t_employeeInfo.Count > 0)
                    {
                        return 401;
                    }
                    else
                    {
                        oldLatticeContent.f_isUnlock = false;
                        oldLatticeContent.f_dormitoryId = null;
                        _repcontent.Update(oldLatticeContent);
                        return 1;
                    }

                }
            }
            return 0;
        }
    }
}
