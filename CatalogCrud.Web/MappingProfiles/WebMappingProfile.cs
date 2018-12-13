using AutoMapper;
using CatalogCrud.BLL.DTO;
using CatalogCrud.Web.Models.ViewModels;

namespace CatalogCrud.Web.MappingProfiles
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            CreateMap<CatalogDTO, CatalogVM>(MemberList.None).ReverseMap();
            CreateMap<FieldDTO, FieldVM>(MemberList.None).ReverseMap();
            CreateMap<ValueDTO, ValueVM>(MemberList.None).ReverseMap();
        }
    }
}