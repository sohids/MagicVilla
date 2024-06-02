using System.ComponentModel.DataAnnotations;

namespace MagicVilla.Api.Models.Dto
{
    public class VillaNumberCreateDto
    {
        [Required]
        public int VillaNumber { get; set; }
        public string SpecialDetails { get; set; }
    }
}
