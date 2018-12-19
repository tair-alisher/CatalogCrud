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
            value.CreatedAt = DateTime.Now;
            value.UpdatedAt = DateTime.Now;
            _worker.Values.Create(value);
            _worker.Save();
        }

        public void Update(ValueDTO item)
        {
            var value = Mapper.Map<Value>(item);
            value.UpdatedAt = DateTime.Now;
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

        public IEnumerable<IOrderedEnumerable<ValueDTO>> GetCatalogValuesByRows(Guid? catalogId)
        {
            if (catalogId == null)
                throw new ArgumentNullException();

            var rows = (
                from value in _worker.Values.GetAll()
                join field in _worker.Fields.GetAll()
                on value.FieldId equals field.Id
                where value.CatalogId == catalogId
                select new ValueDTO
                {
                    Id = value.Id,
                    Title = value.Title,
                    Row = value.Row,
                    FieldId = value.FieldId,
                    CatalogId = (Guid)catalogId,
                    Field = new FieldDTO
                    {
                        Id = field.Id,
                        Name = field.Name
                    }
                }).GroupBy(v => v.Row).Select(r => r.OrderBy(v => v.Field.Name)).ToList();

            return rows;
        }

        public IOrderedEnumerable<ValueDTO> GetCatalogValuesByRow(Guid catalogId, int rowNumber)
        {
            var row = (
                from value in _worker.Values.GetAll()
                join field in _worker.Fields.GetAll()
                on value.FieldId equals field.Id
                where value.CatalogId == catalogId && value.Row == rowNumber
                select new ValueDTO
                {
                    Id = value.Id,
                    Title = value.Title,
                    Row = value.Row,
                    FieldId = value.FieldId,
                    CatalogId = (Guid)catalogId,
                    Field = new FieldDTO
                    {
                        Id = field.Id,
                        Name = field.Name
                    }
                }).GroupBy(v => v.Row).Select(r => r.OrderBy(v => v.Field.Name)).FirstOrDefault();

            return row;
        }

        public IEnumerable<IOrderedEnumerable<Service_ValueDTO>> GetCatalogValuesByRows(Guid catalogId)
        {
            var rows = (
                from value in _worker.Values.GetAll()
                join field in _worker.Fields.GetAll()
                on value.FieldId equals field.Id
                join catalog in _worker.Catalogs.GetAll()
                on value.CatalogId equals catalog.Id
                where value.CatalogId == catalogId
                select new Service_ValueDTO
                {
                    Id = value.Id,
                    Title = value.Title,
                    Row = value.Row,
                    FieldId = value.FieldId,
                    CatalogId = (Guid)catalogId,
                    Field = field.Name,
                    Catalog = catalog.Name
                }).GroupBy(v => v.Row).Select(r => r.OrderBy(v => v.Field)).ToList();

            return rows;
        }

        public IEnumerable<IOrderedEnumerable<Service_ValueDTO>> GetPagedByRowsCatalogValues(Guid catalogId, int page, int itemsPerPage)
        {
            var rows = (
                from value in _worker.Values.GetAll()
                join field in _worker.Fields.GetAll()
                on value.FieldId equals field.Id
                join catalog in _worker.Catalogs.GetAll()
                on value.CatalogId equals catalog.Id
                where value.CatalogId == catalogId
                select new Service_ValueDTO
                {
                    Id = value.Id,
                    Title = value.Title,
                    Row = value.Row,
                    FieldId = value.FieldId,
                    CatalogId = (Guid)catalogId,
                    Field = field.Name,
                    Catalog = catalog.Name
                }).GroupBy(v => v.Row).Select(r => r.OrderBy(v => v.Field)).OrderBy(r => r.FirstOrDefault().Row).Skip(itemsPerPage * (page - 1)).Take(itemsPerPage).ToList();

            return rows;
        }

        public OperationDetails DeleteRowAndDecrementAllFollowing(Guid catalogId, int rowNumber)
        {
            try
            {
                DeleteRow(catalogId, rowNumber);
                DecrementRowsAfterDeleted(catalogId, rowNumber);

                return new OperationDetails(true, "Строка удалена.", "");
            }
            catch (Exception)
            {
                return new OperationDetails(false, "Ошибка. Попробуйте еще раз.", "");
            }
        }

        private void DeleteRow(Guid catalogId, int rowNumber)
        {
            var values = _worker.Values.GetAll().Where(v => v.CatalogId == catalogId && v.Row == rowNumber).ToList();
            foreach (var value in values)
                _worker.Values.Delete(value.Id);
            _worker.Save();
        }

        private void DecrementRowsAfterDeleted(Guid catalogId, int deletedRow)
        {
            var values = _worker.Values.GetAll().Where(v => v.CatalogId == catalogId && v.Row > deletedRow).ToList();
            foreach(var value in values)
            {
                value.Row -= 1;
                _worker.Values.Update(value);
            }
            _worker.Save();
        }
    }
}
