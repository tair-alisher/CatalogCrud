using CatalogCrud.BLL.DTO;
using CatalogCrud.SoapService.DTO;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace CatalogCrud.SoapService
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        IEnumerable<Catalog> GetAllCatalogList();

        [OperationContract]
        Catalog GetCatalogById(Guid id);

        [OperationContract]
        IEnumerable<Row> GetCatalogValuesByRows(Guid catalogId);

        [OperationContract]
        IEnumerable<Row> GetPagedByRowsCatalogValues(Guid CatalogId, int? page, int? itemsPerPage);
    }
}
