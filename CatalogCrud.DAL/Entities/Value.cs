using System;

namespace CatalogCrud.DAL.Entities
{
    public class Value
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Row { get; set; }
        public Guid FieldId { get; set; }
        public Guid CatalogId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Field Field { get; set; }
        public virtual Catalog Catalog { get; set; }
    }
}
