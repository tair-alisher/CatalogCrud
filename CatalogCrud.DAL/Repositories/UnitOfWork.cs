using CatalogCrud.DAL.EF;
using CatalogCrud.DAL.Entities;
using CatalogCrud.DAL.Identity;
using CatalogCrud.DAL.Intefaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Threading.Tasks;

namespace CatalogCrud.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CatalogContext context;

        private BaseRepository<Catalog> catalogRepository;
        private BaseRepository<Field> fieldRepository;
        private BaseRepository<Value> valueRepository;

        public UnitOfWork(string connectionString)
        {
            context = new CatalogContext(connectionString);
            UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            RoleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context));

            UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager)
            {
                AllowOnlyAlphanumericUserNames = false
            };
            UserManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireDigit = true
            };
        }

        public ApplicationUserManager UserManager { get; }
        public ApplicationRoleManager RoleManager { get; }

        public IRepository<Catalog> Catalogs
        {
            get
            {
                if (catalogRepository == null)
                    catalogRepository = new BaseRepository<Catalog>(context);
                return catalogRepository;
            }
        }

        public IRepository<Field> Fields
        {
            get
            {
                if (fieldRepository == null)
                    fieldRepository = new BaseRepository<Field>(context);
                return fieldRepository;
            }
        }

        public IRepository<Value> Values
        {
            get
            {
                if (valueRepository == null)
                    valueRepository = new BaseRepository<Value>(context);
                return valueRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    UserManager.Dispose();
                    RoleManager.Dispose();
                    context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
