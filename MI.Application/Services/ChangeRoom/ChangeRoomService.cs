using MI.Core.Domain;
using MI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MI.Application.Dto
{
    /// <summary>
    /// 员工换房服务
    /// </summary>
    public class ChangeRoomService:IChangeRoomService
    {
        private readonly IBaseRepository<t_ChangeRoom> _ChangeRoom;
        public ChangeRoomService(IBaseRepository<t_ChangeRoom> ChangeRoom)
        {
            _ChangeRoom = ChangeRoom;
        }

        public IList<t_ChangeRoom> GetChangeRoomAllData(int pageIndex, int pageSize, out int count)
        {
            var linq = _ChangeRoom.GetAll().OrderByDescending(u => u.f_Id);
            count = linq.Count();
            ValidatePagingWhere(linq.Count(), ref pageIndex, pageSize);
            return linq.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// 根据条件返回列表
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public IList<t_ChangeRoom> GetConditionByWhere(Expression<Func<t_ChangeRoom, bool>> predicate, int pageIndex, int pageSize, out int count)
        {

            var linq = _ChangeRoom.GetAll().Where(predicate).OrderByDescending(u => u.f_Id);
            count = linq.Count();
            ValidatePagingWhere(linq.Count(), ref pageIndex, pageSize);
            return linq.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
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
        /// 根据id获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public t_ChangeRoom GetChangeRoomById(int id)
        {
            return _ChangeRoom.GetEntityById(id);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">model实体</param>
        public void AddChangeRoomOneData(t_ChangeRoom model)
        {
            _ChangeRoom.Insert(model);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">model实体</param>
        public void EditChangeRoomOneData(t_ChangeRoom model)
        {
            _ChangeRoom.Update(model);
        }
        /// <summary>
        /// 根据id删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        public void DeleteChangeRoom(int id)
        {
            t_ChangeRoom model = GetChangeRoomById(id);
            if (model != null)
                _ChangeRoom.Delete(model);
        }
    }
}
