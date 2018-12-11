
using CatalogCrud.DAL.EF;
using CatalogCrud.DAL.Intefaces;
using System;
using System.Data.Entity;
using System.Linq;

namespace CatalogCrud.DAL.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        private CatalogContext Context;
        private DbSet<T> DbSet;

        public BaseRepository(CatalogContext context)
        {
            Context = context;
            DbSet = Context.Set<T>();
        }

        public void Create(T item)
        {
            DbSet.Add(item);
        }

        public void Delete(Guid id)
        {
            T item = DbSet.Find(id);
            if (item != null)
                DbSet.Remove(item);
        }

        public IQueryable<T> Find(Func<T, bool> predicate)
        {
            return GetAll().Where(predicate).AsQueryable();
        }

        public IQueryable<T> GetAll()
        {
            return DbSet;
        }

        public T Get(Guid? id)
        {
            return DbSet.Find(id);
        }

        public void Update(T item)
        {
            var entry = Context.Entry(item);
            if (entry.State == EntityState.Detached)
                DbSet.Attach(item);
            entry.State = EntityState.Modified;
        }
    }
}
