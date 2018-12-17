using CatalogCrud.BLL.DTO;
using CatalogCrud.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogCrud.BLL.Services
{
    public class SoapCatalogService : ISoapCatalogService
    {
        SqlConnection connection;
        SqlConnectionStringBuilder connBuilder;

        public SoapCatalogService(string dataSource, string initialCatalog)
        {
            connBuilder = new SqlConnectionStringBuilder
            {
                DataSource = dataSource,
                InitialCatalog = initialCatalog,
                Encrypt = true,
                TrustServerCertificate = true,
                ConnectTimeout = 30,
                AsynchronousProcessing = true,
                MultipleActiveResultSets = true,
                IntegratedSecurity = true
            };
            connection = new SqlConnection(connBuilder.ToString());
        }
        public IEnumerable<CatalogDTO> GetAll()
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = "select Id, Name from Catalog";
        }
    }
}
