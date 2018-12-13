using CatalogCrud.BLL.DTO;
using System;
using System.Collections.Generic;

namespace CatalogCrud.BLL.Interfaces
{
    public interface IFieldService : IService<FieldDTO>
    {
        IEnumerable<FieldDTO> FindFields(string value);
    }
}
