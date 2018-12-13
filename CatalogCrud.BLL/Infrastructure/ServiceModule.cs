using CatalogCrud.DAL.Intefaces;
using CatalogCrud.DAL.Repositories;
using Ninject.Modules;

namespace CatalogCrud.BLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        private string connectionString;
        public ServiceModule(string connection)
        {
            connectionString = connection;
        }
        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>().WithConstructorArgument(connectionString);
        }
    }
}
