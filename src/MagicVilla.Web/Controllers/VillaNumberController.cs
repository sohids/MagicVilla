using AutoMapper;
using MagicVilla.Web.Models;
using MagicVilla.Web.Models.Dto;
using MagicVilla.Web.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

namespace MagicVilla.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _villaNumberService;
        private readonly IMapper _mapper;

        public VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper)
        {
            _villaNumberService = villaNumberService;
            _mapper = mapper;
        }
        public async Task<IActionResult> IndexVillaNumber()
        {
            var list = new List<VillaNumberDto>();
            var response = await _villaNumberService.GetVillaAsync<ApiResponse>();
            if (response != null)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDto>>(response.Result.ToString() ?? string.Empty);
            }
            return View(list);
        }

        //public async Task<IActionResult> CreateVilla()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> CreateVilla(VillaCreateDto model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var response = await _villaService.CreateAsync<ApiResponse>(model);
        //        if (response is { IsSuccess: true })
        //        {
        //            return RedirectToAction(nameof(IndexVilla));
        //        }
        //    }

        //    return View(model);
        //}

        //public async Task<IActionResult> UpdateVilla(int id)
        //{
        //    var response = await _villaService.GetAsync<ApiResponse>(id);
        //    if (response is not { IsSuccess: true }) return View();

        //    var model = JsonConvert.DeserializeObject<VillaDto>(response.Result.ToString() ?? string.Empty);

        //    return View(_mapper.Map<VillaUpdateDto>(model));
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> UpdateVilla(VillaUpdateDto model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var response = await _villaService.UpdateAsync<ApiResponse>(model);
        //        if (response is { IsSuccess: true })
        //        {
        //            return RedirectToAction(nameof(IndexVilla));
        //        }
        //    }

        //    return View(model);
        //}

        //public async Task<IActionResult> DeleteVilla(int id)
        //{
        //    var response = await _villaService.GetAsync<ApiResponse>(id);
        //    if (response is not { IsSuccess: true }) return NotFound();

        //    var model = JsonConvert.DeserializeObject<VillaDto>(response.Result.ToString() ?? string.Empty);

        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteVilla(VillaDto model)
        //{
        //    var response = await _villaService.DeleteAsync<ApiResponse>(model.Id);
        //    if (response is { IsSuccess: true })
        //    {
        //        return RedirectToAction(nameof(IndexVilla));
        //    }

        //    return View(model);
        //}
    }
}
