using Autofac;
using Autofac.Extras.DynamicProxy;
using MI.Application.Mappers;
using MI.Application.OASession;
using MI.Data.Uow;
using System.Linq;

namespace MI.Application
{
    public class DIApplicationModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
             .Where(t => t.Name.EndsWith("Service"))
             .AsImplementedInterfaces()
             .EnableInterfaceInterceptors();

            builder.RegisterType<Session>().As<ISession>().InstancePerRequest();

            //注入Mapper
            builder.RegisterType<ApplicationMapperConfiguraton>()
                .As<IMapperConfiguration>()
                .InstancePerLifetimeScope();
            base.Load(builder);
        }
    }
}
