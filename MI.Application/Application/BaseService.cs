using MI.Data;
using System;

namespace MI.Application
{
    public class BaseService<T> : IBaseService<T> where T : class
    {

        private readonly IBaseRepository<T> _repository;
        public BaseService(IBaseRepository<T> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 通过id获取实体对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetObjectById(int id)
        {
            return _repository.GetEntityById(id);
        }
        /// <summary>
        /// 添加实体对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void AddObject(T model)
        {
            _repository.Insert(model);
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void UpdateObject(T model)
        {
            _repository.Update(model);
        }
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void DeleteObject(int id)
        {
            T model = _repository.GetEntityById(id);
            _repository.Delete(model);
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
