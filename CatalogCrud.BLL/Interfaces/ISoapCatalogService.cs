using CatalogCrud.BLL.DTO;
using System.Collections.Generic;

namespace CatalogCrud.BLL.Interfaces
{
    public interface ISoapCatalogService
    {
        IEnumerable<CatalogDTO> GetAll();
    }
}
