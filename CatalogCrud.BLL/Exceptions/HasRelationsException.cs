using System;

namespace CatalogCrud.BLL.Exceptions
{
    public class HasRelationsException : Exception
    {
        public HasRelationsException(string message = "Item has relations.") : base(message) { }
    }
}
