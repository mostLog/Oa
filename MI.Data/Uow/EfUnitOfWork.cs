using System.Transactions;

namespace MI.Data.Uow
{
    public class EFUnitOfWork: UnitOfWorkBase
    {
        private readonly IDbContext _db;

        public EFUnitOfWork(IDbContext db)
        {
            _db = db;
        }
        public override void Begin(UnitOfWorkOptions options)
        {
            if (options.IsTransactional)
            {
                CurrentTransaction = new TransactionScope();
            }
        }
        public override void Complete()
        {
            _db.SaveChanges();
            if (CurrentTransaction!=null)
            {
                CurrentTransaction.Complete();
            }
        }
        public TransactionScope CurrentTransaction { get; set; }
    }
}
