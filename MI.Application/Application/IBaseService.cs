using MI.Application.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application
{
    public interface IBaseService<T>
    {
        /// <summary>
        /// 通过id获取实体对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetObjectById(int id);
        /// <summary>
        /// 添加实体对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        void AddObject(T model);
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        void UpdateObject(T model);
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void DeleteObject(int id);
    }
}
