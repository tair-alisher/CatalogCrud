using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CatalogCrud.SoapService.DTO
{
    [DataContract]
    public class Row
    {
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public List<Value> Values { get; set; }

        public Row()
        {
            Values = new List<Value>();
        }
    }
}
