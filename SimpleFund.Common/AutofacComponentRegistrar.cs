using System.Linq;
using Autofac;
using SimpleFund.Domain;
using SimpleFund.Domain.Repositories;
using SimpleFund.Infrastructure.Mongo;

namespace SimpleFund.Common
{
    public class AutofacComponentRegistrar
    {
        public static void RegisterComponents(ContainerBuilder builder)
        {
            builder.RegisterModule<GenericRepositoryModule>();
            builder.RegisterModule<InfrastructureModule>();
            builder.RegisterModule<TaskModule>();
        }
    }

    public class GenericRepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(MongoRepository<>)).As(typeof(IRepository<>));
            builder.RegisterGeneric(typeof(MongoRepositoryWithTypedId<,>)).As(typeof(IRepositoryWithTypedId<,>));
        }
    }

    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(MongoRepository<>).Assembly)
                   .Where(t => t.Name.EndsWith("Repository") || t.Name.EndsWith("Util"))
                   .As(type => type.GetInterfaces().Single(i => !i.IsGenericType));
        }
    }

    public class TaskModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(Entity).Assembly)
                   .Where(t => t.Name.EndsWith("Task"))
                   .As(type => type.GetInterfaces().Single(i => i.Name.EndsWith("Task") && !i.IsGenericType));
        }
    }
}
