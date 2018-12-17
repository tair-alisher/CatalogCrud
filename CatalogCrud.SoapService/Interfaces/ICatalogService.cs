using CatalogCrud.BLL.DTO;
using System.Collections.Generic;

namespace CatalogCrud.SoapService.Interfaces
{
    public interface ICatalogService
    {
        IEnumerable<CatalogDTO> GetAll();
    }
}
