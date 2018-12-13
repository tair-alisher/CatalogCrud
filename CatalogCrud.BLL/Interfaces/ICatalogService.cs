using CatalogCrud.BLL.DTO;
using CatalogCrud.BLL.Infrastructure;
using System;
using System.Collections.Generic;

namespace CatalogCrud.BLL.Interfaces
{
    public interface ICatalogService : IService<CatalogDTO>
    {
        IEnumerable<FieldDTO> GetCatalogFields(Guid catalogId);
        OperationDetails AttachField(Guid catalogId, Guid fieldId);
        OperationDetails DetachField(Guid catalogId, Guid fieldId);
        IEnumerable<FieldDTO> GetOrderedCatalogFieldList(Guid? catalogId);
    }
}
