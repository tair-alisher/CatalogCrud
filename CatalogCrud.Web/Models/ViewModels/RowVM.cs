using System.Collections.Generic;

namespace CatalogCrud.Web.Models.ViewModels
{
    public class RowVM
    {
        public int Number { get; set; }
        public List<ValueVM> Values { get; set; }

        public RowVM()
        {
            Values = new List<ValueVM>();
        }
    }
}