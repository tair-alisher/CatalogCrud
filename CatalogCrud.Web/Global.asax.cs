using AutoMapper;
using CatalogCrud.BLL.Infrastructure;
using CatalogCrud.BLL.MappingProfiles;
using CatalogCrud.Web.MappingProfiles;
using CatalogCrud.Web.Util;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CatalogCrud.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfiguration.Configure();

            NinjectModule webModule = new WebModule();
            NinjectModule serviceModule = new ServiceModule("DefaultConnection");
            var kernel = new StandardKernel(webModule, serviceModule);
            kernel.Unbind<ModelValidatorProvider>();
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }

    class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AllowNullCollections = true;
                x.AddProfile<BLLMappingProfile>();
                x.AddProfile<WebMappingProfile>();
            });

            Mapper.Configuration.AssertConfigurationIsValid();
        }
    }
}
