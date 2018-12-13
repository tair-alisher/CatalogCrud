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
            field.CreatedAt = DateTime.Now;
            field.UpdatedAt = DateTime.Now;
            _worker.Fields.Create(field);
            _worker.Save();
        }

        public void Update(FieldDTO item)
        {
            var field = Mapper.Map<Field>(item);
            field.UpdatedAt = DateTime.Now;
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

            if (HasRelations((Guid)id))
                return new OperationDetails(false, "У объекта имеются связи. Удаление невозможно.", "");

            _worker.Fields.Delete((Guid)id);
            _worker.Save();

            return new OperationDetails(true, "Поле удалено.", "");
        }

        private bool HasRelations(Guid id)
        {
            var relationsCount = _worker.Fields.Get(id).Catalogs.Count();

            return relationsCount > 0;
        }

        public void Dispose()
        {
            _worker.Dispose();
        }

        public IEnumerable<FieldDTO> FindFields(string value)
        {
            var foundFields = _worker.Fields.GetAll().Where(f => f.Name.Contains(value)).ToList();
            return Mapper.Map<IEnumerable<FieldDTO>>(foundFields);
        }
    }
}
