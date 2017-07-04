using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MI.Application.Dto;
using MI.Core.Domain;
using MI.Data;
using MI.Core.Extension;
using System.Data.Entity;

namespace MI.Application
{
    /// <summary>
    /// 现金登记服务
    /// </summary>
    public class CashRegisterService : BaseService<CashRegister>, ICashRegisterService
    {
        private readonly IBaseRepository<CashRegister> _repository;
        //员工
        private readonly IBaseRepository<Employee> _employeeRepository;
        private readonly IBaseRepository<SType> _typeRepository;
        public CashRegisterService(
            IBaseRepository<CashRegister> repository,
            IBaseRepository<SType> typeRepository,
            IBaseRepository<Employee> employeeRepository
            ) : base(repository)
        {
            _repository = repository;
            _typeRepository = typeRepository;
            _employeeRepository = employeeRepository;
        }
        /// <summary>
        /// 分页获取现金登记数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PageListDto<CashRegisterListDto> GetPagedCashRegisterDatas(CashRegisterPagedInputDto input)
        {
            var tempCashRegisterList = _repository
                .GetAll()
                .GroupJoin(_employeeRepository.GetAll(), cr => cr.f_Handled_f_Eid, e => e.f_eid, (cr, e) =>
                    new
                    {
                        cr = cr,
                        e = e
                    })
                .SelectMany(c => c.e.DefaultIfEmpty(), (cr, e) =>
                    new
                    {
                        cr = cr,
                        e = e
                    }
                  )
                  .WhereIf(!string.IsNullOrWhiteSpace(input.Name), c => c.e.f_chineseName.Contains(input.Name.Trim()) || c.e.f_nickname.Contains(input.Name.Trim()) || c.e.f_passportName.Contains(input.Name.Trim()))
                  .WhereIf(input.StarDate != null,c => c.cr.cr.f_Date!=null && c.cr.cr.f_Date >= input.StarDate)
                  .WhereIf(input.EndDate != null, c => c.cr.cr.f_Date != null && c.cr.cr.f_Date <= input.EndDate)
                  .WhereIf(input.TypeID != 0, c => c.cr.cr.f_workLocation_f_tid == input.TypeID)
                  .Select(tmp => new CashRegisterListDto
                  {
                      f_CId = tmp.cr.cr.f_CId,
                      f_workLocation_f_tid = tmp.cr.cr.f_workLocation_f_tid,
                      f_Date = tmp.cr.cr.f_Date,
                      f_Items = tmp.cr.cr.f_Items,
                      f_Income = tmp.cr.cr.f_Income,
                      f_Expenses = tmp.cr.cr.f_Expenses,
                      f_Handled_f_Eid = tmp.cr.cr.f_Handled_f_Eid,
                      f_Remark = tmp.cr.cr.f_Remark,
                      f_Balance = tmp.cr.cr.f_Balance,
                      f_HasReceipt = tmp.cr.cr.f_HasReceipt,
                      f_nickname = tmp.e.f_nickname
                  });

            //总记录数
            int count = tempCashRegisterList.Count();
            decimal pay = 0;
            decimal earn = 0;
            if (count > 0)
            {
                //总支出
                pay = tempCashRegisterList.Where(c => c != null).Sum(c => c.f_Expenses);
                //总收入
                earn = tempCashRegisterList.Where(c => c != null).Sum(c => c.f_Income);
            }
            var cashRegisterList = tempCashRegisterList
                .OrderByDescending(c => c.f_Date)
                .PageBy(input.iPageIndex, input.iPageSize)
                .ToList();
            return new PageListDto<CashRegisterListDto>(
                cashRegisterList,
                input.iPageIndex, 
                input.iPageSize, 
                count,
                new KeyValueDto<string, object>() { Key="pay",Value=pay},
                new KeyValueDto<string, object>() { Key= "earn",Value=earn }
                );

        }
        /// <summary>
        /// 更新汇率信息
        /// </summary>
        /// <returns></returns>
        public int UpdateRate(RateInputDto input)
        {
            IList<SType> list = _typeRepository
                .GetAll()
                .Where(p => (p.f_value.Equals("美金汇率") || p.f_value.Equals("人民币汇率") || p.f_value.Equals("台币汇率")) && p.f_type == (int)sTypeEnum.汇率类型)
                .ToList();
            int result = 0;
            foreach (var item in list)
            {
                if (item.f_value.Equals("美金汇率"))
                {
                    item.f_Remark = input.Dollar.ToString();
                }
                if (item.f_value.Equals("人民币汇率"))
                {
                    item.f_Remark = input.RMB.ToString();
                }
                if (item.f_value.Equals("台币汇率"))
                {
                    item.f_Remark = input.Taiwan.ToString();
                }
                result = _typeRepository.Update(item);
            }
            return result;
        }
        /// <summary>
        /// 获取汇率
        /// </summary>
        /// <returns></returns>
        public IList<KeyValueDto<string, string>> GetRates()
        {
            return _typeRepository
                .GetAll()
                .AsNoTracking()
                .Where(p => (p.f_value.Equals("美金汇率") || p.f_value.Equals("人民币汇率") || p.f_value.Equals("台币汇率")) && p.f_type == (int)sTypeEnum.汇率类型)
                .Select(t => new KeyValueDto<string, string>() { Key = t.f_value, Value = t.f_Remark })
                .ToList();
        }

    }
}
