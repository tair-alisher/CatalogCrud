﻿using System;
using System.Collections.Generic;

namespace CatalogCrud.DAL.Entities
{
    public class Catalog
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Field> Fields { get; set; }
        public virtual ICollection<Value> Values { get; set; }
    }
}
