
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using MI.Core.Domain;

namespace MI.Application.Dto
{
    /// <summary>
    /// 作者： shawn 
    /// 创始时间：2017-06-15
    /// 描述：员工换房接口
     public interface IChangeRoomService
    {
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="cartype"></param>
        /// <returns></returns>
        IList<ChangeRoom> GetChangeRoomAllData(int pageIndex, int pageSize, out int count);


        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IList<ChangeRoom> GetConditionByWhere(Expression<Func<ChangeRoom, bool>> predicate, int pageIndex, int pageSize, out int count);
        /// <summary>
        /// 根据id获取一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        ChangeRoom GetChangeRoomById(int id);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">model实体</param>
        void AddChangeRoomOneData(ChangeRoom model);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">model实体</param>
        void EditChangeRoomOneData(ChangeRoom model);
        /// <summary>
        /// 根据id删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        void DeleteChangeRoom(int id);
    }
}
