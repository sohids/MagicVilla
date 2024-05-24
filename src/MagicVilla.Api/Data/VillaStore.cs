using MagicVilla.Api.Models.Dto;

namespace MagicVilla.Api.Data
{
    public class VillaStore
    {
        public static List<VillaDto> villaList = new List<VillaDto>
        {
            new VillaDto { Id = 1, Name = "Pool View", Occupency = 20, Sqft = 50},
            new VillaDto { Id = 2, Name = "Beach View", Occupency = 22, Sqft = 60},
        };
    }
}
