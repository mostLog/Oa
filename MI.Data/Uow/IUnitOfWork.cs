using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Data.Uow
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// 
        /// </summary>
        void Begin(UnitOfWorkOptions options);
        /// <summary>
        /// 
        /// </summary>
        void Complete();
    }
}
