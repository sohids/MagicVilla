using MagicVilla.Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla.Web.Models.ViewModels
{
    public class VillaNumberUpdateVm
    {
        public VillaNumberUpdateDto VillaNumber { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? VillaList { get; set; }

        public VillaNumberUpdateVm()
        {
            VillaNumber = new VillaNumberUpdateDto();
        }
    }
}
