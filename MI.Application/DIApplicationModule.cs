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
            //注册服务层
            builder.RegisterAssemblyTypes(ThisAssembly)
             .Where(t => t.Name.EndsWith("Service"))
             .EnableInterfaceInterceptors()
             .AsImplementedInterfaces();

            builder.RegisterType<Session>().As<ISession>().InstancePerRequest();

            //注入Mapper
            builder.RegisterType<ApplicationMapperConfiguraton>()
                .As<IMapperConfiguration>()
                .InstancePerLifetimeScope();
            base.Load(builder);
        }
    }
}
