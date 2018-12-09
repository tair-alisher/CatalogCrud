using System;

namespace CatalogCrud.Web.Models.ViewModels
{
    public class ValueVM
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Row { get; set; }
        public Guid FieldId { get; set; }
        public Guid CatalogId { get; set; }

        public FieldVM Field { get; set; }
        public CatalogVM Catalog { get; set; }
    }
}