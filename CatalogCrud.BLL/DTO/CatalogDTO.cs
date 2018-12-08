using System;
using System.Collections.Generic;

namespace CatalogCrud.BLL.DTO
{
    public class CatalogDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<FieldDTO> Fields { get; set; }
        public ICollection<ValueDTO> Values { get; set; }
    }
}
