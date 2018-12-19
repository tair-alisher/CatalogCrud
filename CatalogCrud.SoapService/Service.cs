using AutoMapper;
using CatalogCrud.BLL.DTO;
using CatalogCrud.BLL.MappingProfiles;
using CatalogCrud.BLL.Services;
using CatalogCrud.SoapService.DTO;
using CatalogCrud.SoapService.MappingProfiles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CatalogCrud.SoapService
{
    public class Service : IDisposable, IService
    {
        BLL.Interfaces.IServiceCreator ServiceCreator;
        BLL.Interfaces.ICatalogService CatalogServ;
        BLL.Interfaces.IValueService ValueServ;

        public Service()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            Mapper.Initialize(x =>
            {
                x.AllowNullCollections = true;
                x.AddProfile<BLLMappingProfile>();
                x.AddProfile<ServiceMappingProfile>();
            });
            Mapper.Configuration.AssertConfigurationIsValid();

            ServiceCreator = new ServiceCreator();
            CatalogServ = ServiceCreator.CreateCatalogService(connectionString);
            ValueServ = ServiceCreator.CreateValueService(connectionString);
        }

        public IEnumerable<Catalog> GetAllCatalogList()
        {
            return Mapper.Map<IEnumerable<Catalog>>(CatalogServ.GetAll().ToList());
        }

        public Catalog GetCatalogById(Guid id)
        {
            return Mapper.Map<Catalog>(CatalogServ.Get(id));
        }

        public IEnumerable<Row> GetCatalogValuesByRows(Guid catalogId)
        {
            var valuesByRows = ValueServ.GetCatalogValuesByRows(catalogId);
            var rows = ConvertDTOValuesByRowsToValuesByRows(valuesByRows);

            return rows;
        }

        private IEnumerable<Row> ConvertDTOValuesByRowsToValuesByRows(IEnumerable<IOrderedEnumerable<Service_ValueDTO>> valuesByRows)
        {
            List<Row> rows = new List<Row>();

            foreach (var valuesByRow in valuesByRows)
            {
                var row = new Row
                {
                    Number = valuesByRow.First().Row
                };
                foreach (var valueDTO in valuesByRow)
                {
                    var value = Mapper.Map<Value>(valueDTO);
                    row.Values.Add(value);
                }
                rows.Add(row);
            }

            return rows;
        }

        public IEnumerable<Row> GetPagedByRowsCatalogValues(Guid catalogId, int? page, int? itemsPerPage)
        {
            var valuesByRows = ValueServ.GetPagedByRowsCatalogValues(catalogId, page ?? 1, itemsPerPage ?? 10);
            var rows = ConvertDTOValuesByRowsToValuesByRows(valuesByRows);
            return rows;
        }

        public void Dispose()
        {
            CatalogServ.Dispose();
            ValueServ.Dispose();
            Mapper.Reset();
        }
    }
}
