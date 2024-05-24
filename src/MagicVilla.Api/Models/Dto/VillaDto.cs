using System.ComponentModel.DataAnnotations;

namespace MagicVilla.Api.Models.Dto
{
    public class VillaDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public int Occupency { get; set; }
        public int Sqft { get; set; }
    }
}
