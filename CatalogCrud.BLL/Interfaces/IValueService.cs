using CatalogCrud.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CatalogCrud.BLL.Interfaces
{
    public interface IValueService : IService<ValueDTO>
    {
        IEnumerable<IOrderedEnumerable<ValueDTO>> GetCatalogValuesByRows(Guid? catalogId);
        IOrderedEnumerable<ValueDTO> GetCatalogValuesByRow(Guid catalogId, int rowNumber);
    }
}
