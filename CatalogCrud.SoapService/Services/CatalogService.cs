using CatalogCrud.BLL.DTO;
using CatalogCrud.BLL.Services;
using CatalogCrud.SoapService.Interfaces;
using System.Collections.Generic;

namespace CatalogCrud.SoapService.Services
{
    public class CatalogService : ICatalogService
    {
        BLL.Interfaces.IServiceCreator ServiceCreator;
        BLL.Interfaces.ICatalogService CatalogSrvc;
        public CatalogService()
        {
            ServiceCreator = new ServiceCreator();
            CatalogSrvc = ServiceCreator.CreateCatalogService(@"Data Source=.\SQLExpress;Initial Catalog=Catalog;Integrated Security=True;");
        }

        public IEnumerable<CatalogDTO> GetAll()
        {
            return CatalogSrvc.GetAll();
        }
    }
}
