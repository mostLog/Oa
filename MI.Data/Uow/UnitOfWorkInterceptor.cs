using Castle.DynamicProxy;

namespace MI.Data.Uow
{
    public class UnitOfWorkInterceptor : IInterceptor
    {
        private readonly IUnitOfWork _unitOfWork;
        public UnitOfWorkInterceptor(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void Intercept(IInvocation invocation)
        {
            _unitOfWork.Begin(new UnitOfWorkOptions());
            invocation.Proceed();
            _unitOfWork.Complete();

        }
    }
}
