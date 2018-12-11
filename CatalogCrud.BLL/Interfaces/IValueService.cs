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
        IOrderedEnumerable<ValueDTO> GetCatalogValuesByRow(Guid catalogId, int rowNumber);
        OperationDetails DeleteRowAndDecrementAllFollowing(Guid catalogId, int rowNumber);
    }
}
