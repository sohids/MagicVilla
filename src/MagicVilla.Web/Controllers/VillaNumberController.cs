using AutoMapper;
using MagicVilla.Web.Models;
using MagicVilla.Web.Models.Dto;
using MagicVilla.Web.Models.ViewModels;
using MagicVilla.Web.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace MagicVilla.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper, IVillaService villaService)
        {
            _villaNumberService = villaNumberService;
            _mapper = mapper;
            _villaService = villaService;
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

        public async Task<IActionResult> CreateVillaNumber()
        {
            var response = await _villaService.GetVillaAsync<ApiResponse>();
            var model = new VillaNumberCreateVm();
            if (response is { IsSuccess: true })
            {
                //we should throw an exception if serialization property doesn't match 
                model.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVm model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.CreateAsync<ApiResponse>(model.VillaNumberCreateDto);
                if (response is { IsSuccess: true })
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
            }
            return View(model);
        }

        //public async Task<IActionResult> UpdateVillaNumber(int id)
        //{
        //    var response = await _villaNumberService.GetAsync<ApiResponse>(id);
        //    if (response is not { IsSuccess: true }) return View();

        //    var model = JsonConvert.DeserializeObject<VillaDto>(response.Result.ToString() ?? string.Empty);

        //    return View(_mapper.Map<VillaUpdateDto>(model));
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> UpdateVillaNumber(VillaUpdateDto model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var response = await _villaNumberService.UpdateAsync<ApiResponse>(model);
        //        if (response is { IsSuccess: true })
        //        {
        //            return RedirectToAction(nameof(IndexVillaNumber));
        //        }
        //    }

        //    return View(model);
        //}

        //public async Task<IActionResult> DeleteVillaNumber(int id)
        //{
        //    var response = await _villaNumberService.GetAsync<ApiResponse>(id);
        //    if (response is not { IsSuccess: true }) return NotFound();

        //    var model = JsonConvert.DeserializeObject<VillaDto>(response.Result.ToString() ?? string.Empty);

        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteVillaNumber(VillaDto model)
        //{
        //    var response = await _villaNumberService.DeleteAsync<ApiResponse>(model.Id);
        //    if (response is { IsSuccess: true })
        //    {
        //        return RedirectToAction(nameof(IndexVillaNumber));
        //    }

        //    return View(model);
        //}
    }
}
