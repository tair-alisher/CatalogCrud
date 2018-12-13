using CatalogCrud.BLL.Infrastructure;
using System;
using System.Collections.Generic;

namespace CatalogCrud.BLL.Interfaces
{
    public interface IService<T> : IDisposable where T : class
    {
        IEnumerable<T> GetAll();
        T Get(Guid? id);
        void Add(T item);
        void Update(T item);
        OperationDetails Delete(Guid? id);
    }
}
