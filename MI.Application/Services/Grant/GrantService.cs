using System;
using System.Collections.Generic;
using System.Linq;
using MI.Application.Dto;
using AutoMapper;
using MI.Data;
using MI.Core.Domain;
using System.Linq.Expressions;
using MI.Core.Extension;
using MI.Application.OASession;
using MI.Application.OASession.Dto;

namespace MI.Application
{
    public class GrantService : BaseService<Grant>, IGrantService
    {
        private readonly IBaseRepository<Grant> _repository;
        private readonly ISession _session;
        public GrantService(
            IBaseRepository<Grant> repository,
            ISession session
            ):
            base(repository)
        {
            _repository = repository;
            _session = session;
        }

        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="iPageIndex">页码</param>
        /// <param name="iPageSize">页大小</param>
        /// <param name="o_Count">总数</param>
        /// <returns></returns>
        public IList<GrantDto> GetGrantByWhere(Expression<Func<Grant, bool>> predicate, int iPageIndex, int iPageSize, out int o_Count)
        {
            var lstGrant = _repository.GetAll().Where(predicate);
            var grant = lstGrant.OrderByDescending(m => m.f_GrantId)
                .PageBy(iPageIndex, iPageSize);
            IList<GrantDto> dtoGrant = Mapper.Map<IList<GrantDto>>(grant.ToList());
            o_Count = lstGrant.Count(); 
            return dtoGrant;
        }
        #region 小白-2017-6-14
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PageListDto<GrantDto> GetPagedAllGrants(GrantPagedInputDto input)
        {
            var grantList=_repository.GetAll();
            grantList = grantList.OrderBy(m => m.f_GrantId)
                .WhereIf(!input.Name.IsNullOrEmpty(), m => m.t_employeeInfo != null && (m.t_employeeInfo.f_chineseName.Contains(input.Name.Trim())) || m.t_employeeInfo.f_nickname.Contains(input.Name.Trim()) || m.t_employeeInfo.f_passportName.Contains(input.Name.Trim()))
                .WhereIf(input.IsPayment != 2, m => m.f_IsPayment == (input.IsPayment == 1 ? true : false))
                .WhereIf(input.GrantStartDate != null, m => m.f_GrantDate!=null&& m.f_GrantDate >= input.GrantStartDate)
                .WhereIf(input.GrantEndDate!=null,m=> m.f_GrantDate!=null&& m.f_GrantDate <= input.GrantEndDate);

            var list = Mapper.Map<IList<GrantDto>>(grantList.PageBy(input.iPageIndex, input.iPageSize).ToList());
            int count = grantList.Count();
            return new PageListDto<GrantDto>(list,input.iPageIndex,input.iPageSize,count);
        }

        /// <summary>
        /// 根据本月生成数据
        /// </summary>
        /// <returns></returns>
        public int GenerateCurrentMonthData()
        {
            OAUser user=_session.GetCurrUser();
            DateTime dAddMont0 = DateTime.Now.AddMonths(-1);//上个月
            DateTime dAddMont1 = DateTime.Now.AddMonths(-2);//上上个月

            DateTime dCurrentMonthTime1 = new DateTime(dAddMont0.Year, dAddMont0.Month, 1); //7-1 号 上个月1号 本月1号
            DateTime dCurrentMonthTime2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); //8-1  本月1号
            DateTime dLastMonthTime = new DateTime(dAddMont1.Year, dAddMont1.Month, 1);//6-1  //上上个月1号
            var vLastMonthData = _repository.GetAll().Where(p => p.f_GrantDate >= dLastMonthTime && p.f_GrantDate < dCurrentMonthTime1).ToList();//大于上上个月1号，并小于上月1号
            int count = 0;
            foreach (var tGrant in vLastMonthData)
            {
                //大于本月1号并小于下个月1号
                if (_repository.GetAll().Any(p => p.f_GrantDate >= dCurrentMonthTime1 && p.f_GrantDate < dCurrentMonthTime2 && p.f_eid == tGrant.f_eid)) continue;
                Grant tNewGrant = new Grant
                {
                    f_eid = tGrant.f_eid,
                    f_amount = tGrant.f_amount,
                    f_GrantDate = dCurrentMonthTime1,
                    f_IsPayment = false,
                    f_Payee = null,
                    f_operator = user.NickName,
                    f_operatorTime = DateTime.Now,
                };
                _repository.Insert(tNewGrant);
                count++;
            }
            return count;
        }
        #endregion
    }
}
