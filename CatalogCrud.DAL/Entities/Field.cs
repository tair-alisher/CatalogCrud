using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogCrud.DAL.Entities
{
    [Table("Field")]
    public class Field
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Catalog> Catalogs { get; set; }
        public virtual ICollection<Value> Values { get; set; }
    }
}
