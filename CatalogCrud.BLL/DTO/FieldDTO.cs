using System;
using System.Collections.Generic;

namespace CatalogCrud.BLL.DTO
{
    public class FieldDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<CatalogDTO> Catalogs { get; set; }
        public ICollection<ValueDTO> Values { get; set; }
    }
}
