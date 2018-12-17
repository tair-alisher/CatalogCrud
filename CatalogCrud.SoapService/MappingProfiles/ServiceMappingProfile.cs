using AutoMapper;
using CatalogCrud.BLL.DTO;
using CatalogCrud.SoapService.DTO;

namespace CatalogCrud.SoapService.MappingProfiles
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            CreateMap<CatalogDTO, Catalog>(MemberList.None).ReverseMap();
            CreateMap<Service_ValueDTO, Value>(MemberList.None).ReverseMap();
        }
    }
}
