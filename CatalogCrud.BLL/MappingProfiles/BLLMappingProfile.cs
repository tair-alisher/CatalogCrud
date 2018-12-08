using AutoMapper;
using CatalogCrud.BLL.DTO;
using CatalogCrud.DAL.Entities;

namespace CatalogCrud.BLL.MappingProfiles
{
    public class BLLMappingProfile : Profile
    {
        public BLLMappingProfile()
        {
            CreateMap<Catalog, CatalogDTO>(MemberList.None).ReverseMap();
            CreateMap<Field, FieldDTO>(MemberList.None).ReverseMap();
            CreateMap<Value, ValueDTO>(MemberList.None).ReverseMap();
        }
    }
}
