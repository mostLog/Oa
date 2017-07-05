using MI.Core.Domain;
using MI.Data.Uow;
using System;
using System.Collections.Generic;

namespace MI.Application
{
    [UnitOfWork]
    public interface ILatticeContentService
    {
        /// <summary>
        /// 查询社区
        /// </summary>
        /// <param name="iswhere">是否包含晾衣房洗衣房</param>
        /// <returns></returns>
        List<string> GetCommunity(bool iswhere = false);
        /// <summary>
        /// 查询楼栋
        /// </summary>
        /// <param name="community">社区</param>
        /// <param name="iswhere">是否包含晾衣房洗衣房</param>
        /// <returns></returns>
        List<string> GetFloorDong(string community, bool iswhere = false);
        /// <summary>
        /// 获取楼栋、社区的ID
        /// </summary>
        /// <param name="sTid">楼栋名字</param>
        /// <param name="sTid2">社区名字</param>
        /// <returns></returns>
        List<MI.Application.ContentServerce.Dto.LatticeContentDto> GetFloorDongCommunityID(string sFloorDongName, string sCommunityName);
        /// <summary>
        /// 获取楼栋社区数据
        /// </summary>
        /// <param name="sTid">楼栋id</param>
        /// <param name="sTid2">社区</param>
        /// <returns></returns>
        List<MI.Application.ContentServerce.Dto.LatticeContentDto> GetFloorDongCommunityData(int sTid, int sTid2);
        /// <summary>
        /// 获取部门
        /// </summary>
        /// <returns></returns>
        List<SType> GetSectorName(Func<SType, bool> predicate);
        /// <summary>
        /// 获取楼栋规模
        /// </summary>
        /// <param name="sFloorDong">楼栋名称</param>
        /// <returns></returns>
        SType GetFloorScale(string sFloorDong);
        /// <summary>
        /// 一键解锁
        /// </summary>
        /// <param name="sBuilding">楼栋</param>
        /// <param name="sCommunity">社区</param>
        /// <returns></returns>
        string AKeyUnlockDormitory(string sBuilding, string sCommunity);

        /// <summary>
        /// 根据条件查询类型
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>返回一个SType的list集合</returns>
        List<SType> SelectTpBypre(Func<SType, bool> predicate);
        /// <summary>
        /// 根据楼栋value获取楼栋
        /// </summary>
        /// <param name="sBuilding">楼栋value</param>
        /// <returns></returns>
        SType GetLoudDogn(string sBuilding);
        /// <summary>
        /// 通过社区value获取社区
        /// </summary>
        /// <param name="sCommunity">社区value</param>
        /// <returns></returns>
        SType GetCommunityByValue(string sCommunity);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sType"></param>
        void Update(SType sType);
        /// <summary>
        ///  生成格子数据
        /// </summary>
        /// <param name="iTid">楼栋ID</param>
        /// <param name="iTid2">社区ID</param>
        /// <param name="iWidth">每层有几间（宽）</param>
        /// <param name="iHigh">有几层（高）</param>
        /// <returns></returns>
        bool GeneratingGridData(int iTid, int iTid2, int iWidth, int iHigh);
        /// <summary>
        /// 解锁or锁定
        /// </summary>
        /// <param name="iLid">格子ID</param>
        /// <param name="bVal">true为解锁，反之</param>
        /// <param name="o_ClassName"></param>
        /// <returns></returns>
        int SetUnlock(int iLid, bool bVal, out string o_ClassName);
    }
}
