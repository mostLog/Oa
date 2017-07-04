using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MI.Core.Domain;
using MI.Application.Dto;

namespace MI.Application
{
    /// <summary>
    /// 车辆管理接口
    /// </summary>
    public interface ICategoryService
    {
        IList<CarRegisterDto> GetAll();
        /// <summary>
        /// 车辆新增
        /// </summary>
        /// <param name="oModel">t_CarRegister实体</param>
        int AddCarRegisterOneData(CarRegister oModel);
        /// <summary>
        /// 车辆修改
        /// </summary>
        /// <param name="oModel"></param>
        /// <returns></returns>
        int EditCarRegisterOneData(CarRegister oModel);
        /// <summary>
        /// 查询实体类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CarRegister GetCarRegisterById(int id);

        int DeleteCarRegister(CarRegister oModel);
        /// <summary>
        /// 根据条件返回列表
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>返回t_sType集合</returns>
        List<SType> GetsTypeByWhere(Func<SType, bool> predicate);


    }
}
