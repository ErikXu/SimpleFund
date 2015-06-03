using Autofac;
using SimpleFund.Common.Configs;
using SimpleFund.Infrastructure.Mongo;

namespace SimpleFund.Common
{
    public class Starter
    {
        public static void Initialize(ContainerBuilder builder)
        {
            //MongoDB Mapping
            Mapping.Set();
            MongoSetting.Current = new MongoSettingByConfig();
            AutofacComponentRegistrar.RegisterComponents(builder);
        }
    }
}
