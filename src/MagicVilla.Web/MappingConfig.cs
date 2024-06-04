using AutoMapper;
using MagicVilla.Web.Models.Dto;

namespace MagicVilla.Web
{
    public class MappingConfig: Profile
    {
        public MappingConfig()
        {
            CreateMap<VillaDto, VillaCreateDto>().ReverseMap();
            CreateMap<VillaDto, VillaUpdateDto>().ReverseMap();

            CreateMap<VillaNumberDto, VillaNumberCreateDto>().ReverseMap();
            CreateMap<VillaNumberDto, VillaUpdateDto>().ReverseMap();
        }
    }
}
