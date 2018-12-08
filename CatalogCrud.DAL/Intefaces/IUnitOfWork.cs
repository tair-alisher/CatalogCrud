using CatalogCrud.DAL.Entities;
using CatalogCrud.DAL.Identity;
using System;
using System.Threading.Tasks;

namespace CatalogCrud.DAL.Intefaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationUserManager UserManager { get; }
        ApplicationRoleManager RoleManager { get; }
        IRepository<Catalog> Catalogs { get; }
        IRepository<Field> Fields { get; }
        IRepository<Value> Values { get; }
        Task SaveAsync();
        void Save();
    }
}
