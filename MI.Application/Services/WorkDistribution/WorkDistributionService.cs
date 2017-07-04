using MI.Application.Dto;
using MI.Core.Common;
using MI.Core.Domain;
using MI.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MI.Application
{
    public class WorkDistributionService : BaseService<WorkDistribution>,IWorkDistributionService
    {
        /// <summary>
        /// 工作交接
        /// </summary>
        private readonly IBaseRepository<WorkDistribution> _repository;
        /// <summary>
        /// 人事信息
        /// </summary>
        private readonly IBaseRepository<Employee> _repEmployee;
        /// <summary>
        /// 权限类别
        /// </summary>
        private readonly IBaseRepository<SType> _sType;
        public WorkDistributionService(IBaseRepository<WorkDistribution> repository, 
            IBaseRepository<Employee> repEmployee, 
            IBaseRepository<SType> sType) :
            base(repository)
        {
            _repository = repository;
            _repEmployee = repEmployee;
            _sType = sType;
        }
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">每页多少数据</param>
        /// <param name="iCount">总数据</param>
        /// <returns>返回一个WorkDistribution的list集合</returns>
        public List<WorkDistribution> GetWorkDistributionAllData(int iPageIndex, int iPageSize, out int iCount)
        {
            int iWorkType = _sType.GetAll().FirstOrDefault(u => u.f_type == (int)sTypeEnum.工作类别 && "新人订餐管理".Equals(u.f_value))?.f_tID ?? 0;
            var linq = _repository.GetAll().Where(u => u.f_WorkType != iWorkType).OrderByDescending(u => u.f_RegisterDate).ThenByDescending(u => u.f_Id);
            iCount = linq.Count();
            ValidatePagingWhere(iCount, ref iPageIndex, iPageSize);
            return linq.Skip((iPageIndex - 1) * iPageSize).Take(iPageSize).ToList();
        }

        public List<WorkDistribution> GetWorkByWhere(Func<WorkDistribution, bool> predicate, int iPageIndex, int iPageSize, out int iCount)
        {
            int iWorkType = _sType.GetAll().FirstOrDefault(u => u.f_type == (int)sTypeEnum.工作类别 && "新人订餐管理".Equals(u.f_value))?.f_tID ?? 0;
            var linq = _repository.GetAll().Where(predicate).Where(u => u.f_WorkType != iWorkType).OrderByDescending(u => u.f_RegisterDate).ThenByDescending(u => u.f_Id);
            iCount = linq.Count();
            ValidatePagingWhere(iCount, ref iPageIndex, iPageSize);
            return linq.Skip((iPageIndex - 1) * iPageSize).Take(iPageSize).ToList();
        }

        public ErrorEnum AddWorkOneData(WorkDistribution model)
        {
            ErrorEnum result = ErrorEnum.Error;
            _repository.Insert(model);
            result = ErrorEnum.Success;
            return result;
        }

        public WorkDistribution GetWorkOneDataById(int iId)
        {
            return _repository.GetEntityById(iId);
        }

        public ErrorEnum UpdataOneData(WorkDistribution model)
        {
            ErrorEnum result = ErrorEnum.Error;
            _repository.Update(model);
            result = ErrorEnum.Success;
            return result;
        }

        public ErrorEnum DeleteById(int iId)
        {
            ErrorEnum result = ErrorEnum.Error;
            WorkDistribution model = _repository.GetEntityById(iId);
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
        /// <summary>
        /// 根据条件返回列表
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>返回一个WorkDistribution的list集合</returns>
        public List<WorkDistribution> GetWorkDistributionByWhere(Func<WorkDistribution, bool> predicate)
        {
            return _repository.GetAll().Where(predicate).OrderByDescending(u => u.f_Id).ToList();
        }
    }
}
