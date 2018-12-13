using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CatalogCrud.Web.Models.ViewModels
{
    public class CatalogVM
    {
        public Guid Id { get; set; }
        [Display(Name = "Справочник")]
        public string Name { get; set; }

        public ICollection<FieldVM> Fields { get; set; }
        public ICollection<ValueVM> Values { get; set; }
    }
}