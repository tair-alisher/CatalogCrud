using System.Data.SqlClient;

namespace CatalogCrud.SoapService.Config
{
    public static class Config
    {
        public static string ConnectionString {
            get
            {
                return new SqlConnectionStringBuilder {
                    DataSource = @".\SqlExpress",
                    InitialCatalog = "Catalog",
                    //UserID = "",
                    //Password = "",
                    //Encrypt = true,
                    //TrustServerCertificate = true,
                    //ConnectTimeout = 30,
                    AsynchronousProcessing = true,
                    //MultipleActiveResultSets = true,
                    IntegratedSecurity = true
                }.ToString();
            }
        }
    }
}
