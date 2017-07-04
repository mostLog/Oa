using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Data.Uow
{
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        public virtual void Begin(UnitOfWorkOptions options)
        {
            
        }

        public virtual void Complete()
        {
           
        }
    }
}
