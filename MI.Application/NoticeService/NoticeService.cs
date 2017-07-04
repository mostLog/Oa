using MI.Core.Common;
using MI.Core.Domain;
using MI.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application
{
    public class NoticeService : INoticeService
    {
        private readonly IBaseRepository<Notice> _notice;
        public NoticeService(IBaseRepository<Notice> notice)
        {
            _notice = notice;
        }
        /// <summary>
        /// 公告管理
        /// </summary>
        /// <returns>返回Notice集合</returns>
        public List<Notice> GetNoticeAllData()
        {
            return _notice.GetAll().OrderBy(u => u.f_IsTop).ThenBy(u => u.f_EndDate).ToList();
        }
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>返回Notice集合</returns>
        public List<Notice> GetConditionByWhere(Func<Notice, bool> predicate)
        {
            return _notice.GetAll().Where(predicate).OrderBy(u => u.f_IsTop).ThenBy(u => u.f_EndDate).ToList();
        }
        /// <summary>
        /// 根据id获取一条数据
        /// </summary>
        /// <param name="iId">id</param>
        /// <returns>返回一个Notice实体</returns>
        public Notice GetNoticeById(int iId)
        {
            return _notice.GetEntityById(iId);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">Notice实体</param>
        public ErrorEnum AddNoticeOneData(Notice model)
        {
            ErrorEnum result = ErrorEnum.Error;
            JObject newjson = new JObject
            {
                {"公告类型", model.f_Type ? "紧急公告" : "一般公告"},
                {"公告内容", model.f_Content},
                {"添加时间", model.f_AddDate.ToString("yyyy-MM-dd")},
                {"添加人", model.f_Registrant},
                {"有效开始时间", model.f_StartDate.ToString("yyyy-MM-dd")},
                {"有效结束时间", model.f_EndDate.ToString("yyyy-MM-dd")},
                {"数据ID", model.f_Id}
            };
            _notice.Insert(model);
            result = ErrorEnum.Success;
            return result;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">Notice实体</param>
        public ErrorEnum EditNoticeOneData(Notice model)
        {
            ErrorEnum result = ErrorEnum.Error;
            var odldata = GetNoticeById(model.f_Id);
            odldata.f_Content = model.f_Content;
            odldata.f_StartDate = model.f_StartDate;
            odldata.f_EndDate = model.f_EndDate;
            odldata.f_Type = model.f_Type;
            _notice.Update(odldata);
            result = ErrorEnum.Success;
            return result;
        }
        /// <summary>
        /// 根据id删除一条数据
        /// </summary>
        /// <param name="iId">Id</param>
        public ErrorEnum DeleteNotice(int iId)
        {
            ErrorEnum result = ErrorEnum.Error;
            Notice model = GetNoticeById(iId);
            if (model != null)
                _notice.Delete(model);
            result = ErrorEnum.Success;
            return result;
        }
        /// <summary>
        /// 将实体转化成json数据，新人数据
        /// </summary>
        /// <param name="oModel">员工实体</param>
        /// <returns>json数据</returns>
        public string GetJObjectData(Notice oModel)
        {
            return new JObject
            {
                {"公告类型", oModel.f_Type ? "紧急公告" : "一般公告"},
                {"公告内容", oModel.f_Content},
                {"添加时间", oModel.f_AddDate.ToString("yyyy-MM-dd")},
                {"添加人", oModel.f_Registrant},
                {"有效开始时间", oModel.f_StartDate.ToString("yyyy-MM-dd")},
                {"有效结束时间", oModel.f_EndDate.ToString("yyyy-MM-dd")},
                {"数据ID", oModel.f_Id}
            }.ToString();
        }
        /// <summary>
        /// 置顶
        /// </summary>
        /// <param name="iId">id</param>
        /// <param name="istop">当前置顶状态</param>
        public ErrorEnum IsTop(int iId, string istop)
        {
            ErrorEnum result = ErrorEnum.Error;
            Notice oOdlData = GetNoticeById(iId);
            oOdlData.f_IsTop = istop != "True";
            _notice.Update(oOdlData);
            result = ErrorEnum.Success;
            return result;
        }
    }
}
