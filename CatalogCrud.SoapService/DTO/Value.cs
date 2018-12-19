using System;
using System.Runtime.Serialization;

namespace CatalogCrud.SoapService.DTO
{
    [DataContract]
    public class Value
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public int Row { get; set; }
        [DataMember]
        public Guid FieldId { get; set; }
        [DataMember]
        public Guid CatalogId { get; set; }
        [DataMember]
        public string Field { get; set; }
        [DataMember]
        public string Catalog { get; set; }
    }
}
