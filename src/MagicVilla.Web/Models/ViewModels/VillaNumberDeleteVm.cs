using MagicVilla.Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla.Web.Models.ViewModels
{
    public class VillaNumberDeleteVm
    {
        public VillaNumberDto VillaNumber { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? VillaList { get; set; }

        public VillaNumberDeleteVm()
        {
            VillaNumber = new VillaNumberDto();
        }
    }
}
