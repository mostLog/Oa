using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MI.Core.Domain;
using MI.Data;
using MI.Application.Dto;
using AutoMapper;

namespace MI.Application
{
    /// <summary>
    /// 车辆管理服务
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly IBaseRepository<CarRegister> _employeeRepository;
        private readonly IBaseRepository<SType> _stype;
        public CategoryService(IBaseRepository<CarRegister> employeeRepository, IBaseRepository<SType> stype)
        {
            _employeeRepository = employeeRepository;
            _stype = stype;
        }
        /// <summary>
        /// 查询车辆表的方法
        /// </summary>
        /// <returns></returns>
        public IList<CarRegisterDto> GetAll()
        {
            IList<CarRegisterDto> employee = Mapper.Map<IList<CarRegisterDto>>(_employeeRepository.GetAll().ToList()); 
            return employee;
        }
        /// <summary>
        /// 车辆管理新增方法
        /// </summary>
        /// <param name="oModel"></param>
        public int AddCarRegisterOneData(CarRegister oModel) {
            int result = _employeeRepository.Insert(oModel);
            return result;
        }
        /// <summary>
        /// 车辆管理修改方法
        /// </summary>
        /// <param name="oModel"></param>
        /// <returns></returns>
        public int EditCarRegisterOneData(CarRegister oModel) {
            int result = _employeeRepository.Update(oModel);
            return result;
        }
        /// <summary>
        /// 根据id查询车辆实体类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CarRegister GetCarRegisterById(int id) {
            CarRegister register = _employeeRepository.GetEntityById(id);
            return register;
        }
        /// <summary>
        /// 删除车辆
        /// </summary>
        /// <param name="oModel"></param>
        /// <returns></returns>
        public int DeleteCarRegister(CarRegister oModel) {
            int result = _employeeRepository.Delete(oModel);
            return result;
        }
        /// <summary>
        /// 根据条件返回列表
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>返回t_sType集合</returns>
        public List<SType> GetsTypeByWhere(Func<SType, bool> predicate)
        {
            return _stype.GetAll().Where(predicate).OrderByDescending(u => u.f_tID).ToList();
        }

    }
}
