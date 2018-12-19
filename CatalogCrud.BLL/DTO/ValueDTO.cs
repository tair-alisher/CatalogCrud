using System;

namespace CatalogCrud.BLL.DTO
{
    public class ValueDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Row { get; set; }
        public Guid FieldId { get; set; }
        public Guid CatalogId { get; set; }

        public FieldDTO Field { get; set; }
        public CatalogDTO Catalog { get; set; }
    }

    public class Service_ValueDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Row { get; set; }
        public Guid FieldId { get; set; }
        public Guid CatalogId { get; set; }

        public string Field { get; set; }
        public string Catalog { get; set; }
    }
}
