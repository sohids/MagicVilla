using System.ComponentModel.DataAnnotations;

namespace MagicVilla.Api.Models.Dto
{
    public class VillaNumberDto
    {
        [Required]
        public int VillaNo { get; set; }
        public int VillaId { get; set; }

        public string SpecialDetails { get; set; }
    }
}
