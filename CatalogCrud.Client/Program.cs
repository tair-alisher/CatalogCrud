using CatalogCrud.SoapService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogCrud.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new Service();
            var catalogs = client.GetAll();

            foreach (var catalog in catalogs)
                Console.WriteLine(catalog.Name);

            Console.ReadLine();
        }
    }
}
