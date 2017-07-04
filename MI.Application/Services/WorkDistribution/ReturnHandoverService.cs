using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MI.Core.Domain;
using MI.Data;
using MI.Application.Dto;
using MI.Core.Common;

namespace MI.Application
{
    public class ReturnHandoverService : IReturnHandoverService
    {
        /// <summary>
        /// 工作交接
        /// </summary>
        private readonly IBaseRepository<ReturnHandover> _repository;
        /// <summary>
        /// 人事信息
        /// </summary>
        private readonly IBaseRepository<Employee> _repEmployee;
        /// <summary>
        /// 权限类别
        /// </summary>
        private readonly IBaseRepository<SType> _sType;
        public ReturnHandoverService(IBaseRepository<ReturnHandover> repository, IBaseRepository<Employee> repEmployee, IBaseRepository<SType> sType)
        {
            _repository = repository;
            _repEmployee = repEmployee;
            _sType = sType;
        }

        public IList<ReturnHandover> GetAllReturnData()
        {
            IList<ReturnHandover> ReturnList = _repository.GetAll().ToList();
            return ReturnList;
        }

        public IList<ReturnHandover> GetReturnByWhere(Func<ReturnHandover, bool> predicate)
        {
            IList<ReturnHandover> ReturnList = _repository.GetAll().Where(u => u.t_employeeInfo.f_IsNewEmp ? false : true).Where(predicate).ToList();
            return ReturnList;
        }

        public ReturnHandover GetReturnOneDataById(int iId)
        {
            return _repository.GetEntityById(iId);
        }

        public ErrorEnum AddReturnOneData(ReturnHandover model)
        {
            ErrorEnum result = ErrorEnum.Error;
            _repository.Insert(model);
            result = ErrorEnum.Success;
            return result;
        }

        public ErrorEnum UpdateReturnOneData(ReturnHandover model)
        {
            ErrorEnum result = ErrorEnum.Error;
            _repository.Update(model);
            result = ErrorEnum.Success;
            return result;
        }

        public ErrorEnum DeleteReturnOneData(int Id)
        {
            ErrorEnum result = ErrorEnum.Error;
            ReturnHandover model = _repository.GetEntityById(Id);
            if (model != null)
            {
                _repository.Delete(model);
                result = ErrorEnum.Success;
            }
            else
            {
                result = ErrorEnum.Fail;
            }
            return result;
        }
    }
}
