﻿using MagicVilla.Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla.Web.Models.ViewModels
{
    public class VillaNumberCreateVm
    {
        public VillaNumberCreateDto VillaNumber { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? VillaList { get; set; }

        public VillaNumberCreateVm()
        {
            VillaNumber = new VillaNumberCreateDto();
        }
    }
}
