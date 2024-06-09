using AutoMapper;
using MagicVilla.Web.Models.Dto;
using MagicVilla.Web.Models.ViewModels;

namespace MagicVilla.Web
{
    public class MappingConfig: Profile
    {
        public MappingConfig()
        {
            CreateMap<VillaDto, VillaCreateDto>().ReverseMap();
            CreateMap<VillaDto, VillaUpdateDto>().ReverseMap();

            CreateMap<VillaNumberDto, VillaNumberCreateDto>().ReverseMap();
            CreateMap<VillaNumberDto, VillaNumberUpdateDto>().ReverseMap();
        }
    }
}
