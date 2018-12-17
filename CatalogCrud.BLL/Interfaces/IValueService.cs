using CatalogCrud.BLL.DTO;
using CatalogCrud.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CatalogCrud.BLL.Interfaces
{
    public interface IValueService : IService<ValueDTO>
    {
        IEnumerable<IOrderedEnumerable<ValueDTO>> GetCatalogValuesByRows(Guid? catalogId);
        IEnumerable<IOrderedEnumerable<Service_ValueDTO>> GetCatalogValuesByRows(Guid catalogId);
        IEnumerable<IOrderedEnumerable<Service_ValueDTO>> GetPagedByRowsCatalogValues(Guid catalogId, int page, int itemsPerPage);
        IOrderedEnumerable<ValueDTO> GetCatalogValuesByRow(Guid catalogId, int rowNumber);
        OperationDetails DeleteRowAndDecrementAllFollowing(Guid catalogId, int rowNumber);
    }
}
