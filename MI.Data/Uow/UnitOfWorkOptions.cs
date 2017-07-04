using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Data.Uow
{
    public class UnitOfWorkOptions
    {
        /// <summary>
        /// 是否开启事务
        /// </summary>
        public bool IsTransactional { get; set; }
    }
}
