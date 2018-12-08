using System;

namespace CatalogCrud.BLL.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message = "Item with given id not found.") : base(message) { }
    }
}
