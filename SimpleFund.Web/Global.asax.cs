using System.Reflection;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Configuration;
using Autofac.Integration.WebApi;
using MongoDB.Bson;
using SimpleFund.Common;
using SimpleFund.Web.Common.Filters;
using SimpleFund.Web.Common.ModelBinders;

namespace SimpleFund.Web
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AutofacRegister(GlobalConfiguration.Configuration);
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            GlobalConfiguration.Configuration.EnsureInitialized();
        }

        private void AutofacRegister(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            Starter.Initialize(builder);
            builder.RegisterModule(new ConfigurationSettingsReader("autofac"));
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);

            RegisterFilters(builder);
            RegisterModelBinders(builder);

            var container = builder.Build();
            var resolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }

        /// <summary>
        /// Register autofac provided filters IAutofacActionFilter, IAutofacAuthorizationFilter, IAutofacExceptionFilter.
        /// </summary>
        /// <param name="builder"></param>
        private static void RegisterFilters(ContainerBuilder builder)
        {
            builder.RegisterType<TransformWebApiErrorAttribute>().AsWebApiExceptionFilterFor<ApiController>();
        }

        private static void RegisterModelBinders(ContainerBuilder builder)
        {
            builder.RegisterWebApiModelBinderProvider();
            builder.RegisterType<ObjectIdModelBinder>().AsModelBinderForTypes(typeof(ObjectId));
        }
    }
}
