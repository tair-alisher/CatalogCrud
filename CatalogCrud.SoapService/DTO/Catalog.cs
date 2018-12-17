using System;
using System.Runtime.Serialization;

namespace CatalogCrud.SoapService.DTO
{
    [DataContract]
    public class Catalog
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}
