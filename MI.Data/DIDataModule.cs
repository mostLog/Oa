using Autofac;
using MI.Core;
using MI.Data.Uow;

namespace MI.Data
{
    public class DIDataModule:Module
    {
        
        protected override void Load(ContainerBuilder builder)
        {
            Config config = new Config();

            builder.RegisterType<UnitOfWorkInterceptor>().InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerLifetimeScope();

            builder.Register<IDbContext>(c => new OADbContext(config.ConnectionString)).InstancePerLifetimeScope();


            builder.RegisterType<EFUnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
