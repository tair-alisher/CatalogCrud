using CatalogCrud.BLL.Interfaces;
using CatalogCrud.BLL.Services;
using Ninject.Modules;

namespace CatalogCrud.Web.Util
{
    public class WebModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICatalogService>().To<CatalogService>();
            Bind<IFieldService>().To<FieldService>();
            Bind<IValueService>().To<ValueService>();
        }
    }
}