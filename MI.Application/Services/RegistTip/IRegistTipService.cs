using MI.Core.Domain;
using MI.Data.Uow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application
{
    [UnitOfWork]
    public interface IRegistTipService
    {
        /// <summary>
        /// 根据员工和表中文名查询
        /// </summary>
        /// <param name="iEid">员工Id</param>
        /// <param name="strTableName">中文名</param>
        /// <returns></returns>
        RegistTip GetRegistTipByCondition(int iEid, string strTableName);

         /// <summary>
         /// 修改
         /// </summary>
         /// <param name="model">model实体</param>
        void EditRegistTipData(RegistTip model); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">model实体</param>
        void AddRegistTipData(RegistTip model);
        void DoRegistTip<T>(T model, int? eId, string strTable, string strTableName);
        /// <summary>
        /// 登记查询提示的数量
        /// </summary>
        /// <param name="iEid">员工ID</param>
        /// <returns></returns>
        int TipsCount(int iEid);
    } 
}
