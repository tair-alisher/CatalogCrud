using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CatalogCrud.BLL.DTO;
using CatalogCrud.BLL.Exceptions;
using CatalogCrud.BLL.Infrastructure;
using CatalogCrud.BLL.Interfaces;
using CatalogCrud.DAL.Entities;
using CatalogCrud.DAL.Intefaces;

namespace CatalogCrud.BLL.Services
{
    public class ValueService : IValueService
    {
        private readonly IUnitOfWork _worker;

        public ValueService(IUnitOfWork uow)
        {
            _worker = uow;
        }

        public IEnumerable<ValueDTO> GetAll()
        {
            var values = _worker.Values.GetAll().ToList();
            return Mapper.Map<IEnumerable<ValueDTO>>(values);
        }

        public ValueDTO Get(Guid? id)
        {
            if (id == null)
                throw new ArgumentNullException();

            var value = _worker.Values.Get(id);
            if (value == null)
                throw new NotFoundException();

            return Mapper.Map<ValueDTO>(value);
        }

        public void Add(ValueDTO item)
        {
            var value = Mapper.Map<Value>(item);
            value.Id = Guid.NewGuid();
            _worker.Values.Create(value);
            _worker.Save();
        }

        public void Update(ValueDTO item)
        {
            var value = Mapper.Map<Value>(item);
            _worker.Values.Update(value);
            _worker.Save();
        }

        public OperationDetails Delete(Guid? id)
        {
            if (id == null)
                return new OperationDetails(false, "Идентификатор не задан.", "");

            var value = _worker.Values.Get(id);
            if (value == null)
                return new OperationDetails(false, "Значение не найдено.", "");

            _worker.Values.Delete((Guid)id);
            _worker.Save();

            return new OperationDetails(true, "Значение удалено.", "");
        }

        public void Dispose()
        {
            _worker.Dispose();
        }
    }
}
