using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CatalogCrud.Web.Models.ViewModels
{
    public class FieldVM
    {
        public Guid Id { get; set; }
        [Display(Name = "Поле")]
        public string Name { get; set; }

        public ICollection<CatalogVM> Catalogs { get; set; }
        public ICollection<ValueVM> Values { get; set; }
    }
}