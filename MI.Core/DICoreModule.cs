using Autofac;
using Castle.DynamicProxy;

namespace MI.Core
{
    public class DICoreModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //单利注入
            builder.RegisterType<Config>().As<IConfig>().SingleInstance();

            base.Load(builder);
        }
    }
}
