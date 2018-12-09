using AutoMapper;
using CatalogCrud.BLL.DTO;
using CatalogCrud.BLL.Exceptions;
using CatalogCrud.BLL.Infrastructure;
using CatalogCrud.BLL.Interfaces;
using CatalogCrud.DAL.Entities;
using CatalogCrud.DAL.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CatalogCrud.BLL.Services
{
    public class FieldService : IFieldService
    {
        private readonly IUnitOfWork _worker;

        public FieldService(IUnitOfWork uow)
        {
            _worker = uow;
        }

        public IEnumerable<FieldDTO> GetAll()
        {
            var fields = _worker.Fields.GetAll().OrderBy(f => f.Name).ToList();
            return Mapper.Map<IEnumerable<FieldDTO>>(fields);
        }

        public FieldDTO Get(Guid? id)
        {
            if (id == null)
                throw new ArgumentNullException();

            var field = _worker.Fields.Get(id);
            if (field == null)
                throw new NotFoundException();

            return Mapper.Map<FieldDTO>(field);
        }

        public void Add(FieldDTO item)
        {
            var field = Mapper.Map<Field>(item);
            field.Id = Guid.NewGuid();
            _worker.Fields.Create(field);
            _worker.Save();
        }

        public void Update(FieldDTO item)
        {
            var field = Mapper.Map<Field>(item);
            _worker.Fields.Update(field);
            _worker.Save();
        }

        public OperationDetails Delete(Guid? id)
        {
            if (id == null)
                return new OperationDetails(false, "Идентификатор не задан.", "");

            var field = _worker.Fields.Get(id);
            if (field == null)
                return new OperationDetails(false, "Поле не найдено.", "");

            _worker.Fields.Delete((Guid)id);
            _worker.Save();

            return new OperationDetails(true, "Поле удалено.", "");
        }

        public void Dispose()
        {
            _worker.Dispose();
        }
    }
}
