using CatalogCrud.BLL.Interfaces;
using CatalogCrud.DAL.Repositories;

namespace CatalogCrud.BLL.Services
{
    public class ServiceCreator : IServiceCreator
    {
        public IUserService CreateUserService(string connection)
        {
            return new UserService(new UnitOfWork(connection));
        }

        public ICatalogService CreateCatalogService(string connection)
        {
            return new CatalogService(new UnitOfWork(connection));
        }

        public IFieldService CreateFieldService(string connection)
        {
            return new FieldService(new UnitOfWork(connection));
        }

        public IValueService CreateValueService(string connection)
        {
            return new ValueService(new UnitOfWork(connection));
        }
    }
}
