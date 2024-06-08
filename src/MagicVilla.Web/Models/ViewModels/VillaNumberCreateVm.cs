using MagicVilla.Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla.Web.Models.ViewModels
{
    public class VillaNumberCreateVm
    {
        public VillaNumberCreateDto VillaNumberCreateDto { get; set; }
        public IEnumerable<SelectListItem>? VillaList { get; set; }

        public VillaNumberCreateVm()
        {
            VillaNumberCreateDto = new VillaNumberCreateDto();
        }
    }
}
