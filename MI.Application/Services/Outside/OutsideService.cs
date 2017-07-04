using MI.Core.Domain;
using MI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application
{
    public class OutsideService:IOutsideService
    {
        public readonly IBaseRepository<Outside> _outside;
        public OutsideService(IBaseRepository<Outside> outside) {
            _outside = outside;
        }
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Outside> GetOutsideAllData(int pageIndex, int pageSize, out int count)
        {
            var linq = _outside.GetAll().OrderByDescending(u => u.f_Id);
            count = linq.Count();
            ValidatePagingWhere(count, ref pageIndex, pageSize);
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
        public List<Outside> GetConditionByWhere(Expression<Func<Outside, bool>> predicate, int pageIndex, int pageSize, out int count)
        {
            var linq = _outside.GetAll().Where(predicate).OrderByDescending(u => u.f_Id);
            count = linq.Count();
            ValidatePagingWhere(count, ref pageIndex, pageSize);
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
        public Outside GetOutsideById(int id)
        {
            return _outside.GetEntityById(id);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">model实体</param>
        public void AddOutsideOneData(Outside model)
        {
           _outside.Insert(model);
            
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">model实体</param>
        public void EditOutsideOneData(Outside model)
        {
            _outside.Update(model);
        }
        /// <summary>
        /// 根据id删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        public void DeleteOutside(int id)
        {
            Outside model = GetOutsideById(id);
            if (model != null)
            {
                _outside.Delete(model);
                
            }
            else {
                
            }
            
        }
    }
}
