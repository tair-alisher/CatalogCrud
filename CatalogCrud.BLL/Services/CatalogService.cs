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
    public class CatalogService : ICatalogService
    {
        private readonly IUnitOfWork _worker;

        public CatalogService(IUnitOfWork uow)
        {
            _worker = uow;
        }

        public IEnumerable<CatalogDTO> GetAll()
        {
            var catalogs = _worker.Catalogs.GetAll().OrderBy(c => c.Name).ToList();
            return Mapper.Map<IEnumerable<CatalogDTO>>(catalogs);
        }

        public CatalogDTO Get(Guid? id)
        {
            if (id == null)
                throw new ArgumentNullException();

            var catalog = _worker.Catalogs.Get(id);
            if (catalog == null)
                throw new NotFoundException();

            return Mapper.Map<CatalogDTO>(catalog);
        }

        public void Add(CatalogDTO item)
        {
            var catalog = Mapper.Map<Catalog>(item);
            catalog.Id = Guid.NewGuid();
            _worker.Catalogs.Create(catalog);
            _worker.Save();
        }

        public void Update(CatalogDTO item)
        {
            var catalog = Mapper.Map<Catalog>(item);
            _worker.Catalogs.Update(catalog);
            _worker.Save();
        }

        public OperationDetails Delete(Guid? id)
        {
            if (id == null)
                return new OperationDetails(false, "Идентификатор не задан.", "");

            var catalog = _worker.Catalogs.Get(id);
            if (catalog == null)
                return new OperationDetails(false, "Справочник не найден.", "");

            _worker.Catalogs.Delete((Guid)id);
            _worker.Save();

            return new OperationDetails(true, "Справочник удален.", "");
        }

        public void Dispose()
        {
            _worker.Dispose();
        }

        public IEnumerable<FieldDTO> GetCatalogFields(Guid catalogId)
        {
            var fields = _worker.Catalogs.Get(catalogId).Fields.ToList();
            return Mapper.Map<IEnumerable<FieldDTO>>(fields);
        }

        public OperationDetails AttachField(Guid catalogId, Guid fieldId)
        {
            var catalog = _worker.Catalogs.Get(catalogId);
            var field = _worker.Fields.Get(fieldId);
            if (catalog == null || field == null)
                return new OperationDetails(false, "Идентификатор не задан.", "");

            catalog.Fields.Add(field);
            _worker.Save();

            return new OperationDetails(true, "Поле закреплено.", "");
        }
    }
}
