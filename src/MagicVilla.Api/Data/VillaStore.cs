using MagicVilla.Api.Models.Dto;

namespace MagicVilla.Api.Data
{
    public class VillaStore
    {
        public static List<VillaDto> villaList = new List<VillaDto>
        {
            new VillaDto { Id = 1, Name = "Pool View", Occupancy = 20, SqFt = 50},
            new VillaDto { Id = 2, Name = "Beach View", Occupancy = 22, SqFt = 60},
        };
    }
}
