using System.ComponentModel.DataAnnotations;

namespace MagicVilla.Web.Models.Dto
{
    public class VillaNumberDto
    {
        public int VillaNo { get; set; }
        public int VillaId { get; set; }

        public string SpecialDetails { get; set; }

        public VillaDto Villa { get; set; }
    }
}
