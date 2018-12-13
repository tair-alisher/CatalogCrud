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
    }
}
