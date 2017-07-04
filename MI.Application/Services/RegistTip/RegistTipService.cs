using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MI.Core.Domain;
using MI.Data;
using System.Data.SqlClient;

namespace MI.Application
{
    public class RegistTipService : IRegistTipService
    {
        private readonly IBaseRepository<RegistTip> _repository;
        private readonly IDbContext _dbcontext;
        public RegistTipService(IBaseRepository<RegistTip> repository,
            IDbContext dbcontext)
        {
            _repository = repository;
            _dbcontext = dbcontext;
        }

        public void AddRegistTipData(RegistTip model)
        {
            _repository.Insert(model);
        }

        public void EditRegistTipData(RegistTip model)
        {
            var olddata = GetRegistTipByCondition(model.f_eId.Value, model.f_TableName);
            olddata.f_eId = model.f_eId;
            olddata.f_Table = model.f_Table;
            olddata.f_TableName = model.f_TableName;
            olddata.f_TipState = model.f_TipState;
            olddata.f_OperatorTime = model.f_OperatorTime;
            _repository.Update(olddata);

        }

        public RegistTip GetRegistTipByCondition(int iEid, string strTableName)
        {
            var linq = _repository.GetAll().Where(u => ((u.f_eId == iEid) && (u.f_TableName == strTableName)));
            if (linq.ToList().Count > 0)
            {
                return linq.ToList()[0];
            }
            return null;
        }
        public virtual void DoRegistTip<T>(T model, int? eId, string strTable, string strTableName)
        {
            #region 往登记查询提示表写操作

            if (eId != null)
            {
                RegistTip regmodel = GetRegistTipByCondition(eId.Value, strTableName);
                if (regmodel == null)
                {
                    regmodel = new RegistTip
                    {
                        f_eId = eId.Value,
                        f_Table = strTable,
                        f_TableName = strTableName,
                        f_TipState = 1,
                        f_OperatorTime = DateTime.Now
                    };
                    AddRegistTipData(regmodel);
                }
                else
                {
                    regmodel = new RegistTip
                    {
                        f_eId = eId.Value,
                        f_Table = strTable,
                        f_TableName = strTableName,
                        f_TipState = 1,
                        f_OperatorTime = DateTime.Now
                    };
                    EditRegistTipData(regmodel);
                }

            }

            #endregion
        }

        /// <summary>
        /// 登记查询提示的数量
        /// </summary>
        /// <param name="iEid">员工ID</param>
        /// <returns></returns>
        public int TipsCount(int iEid)
        {

            var sumcount = _dbcontext.SqlQuery<int>("select isnull(sum(f_TipState),0) as sumcount  from [dbo].[t_RegistTip] WHERE f_eId=@Eid",
                    new SqlParameter("@Eid", iEid));
            return sumcount.First();
        }
    }
}
