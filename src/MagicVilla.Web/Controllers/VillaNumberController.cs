﻿using AutoMapper;
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
                var response = await _villaNumberService.CreateAsync<ApiResponse>(model.VillaNumber);
                if (response is { IsSuccess: true })
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
            }
            return View(model);
        }

        public async Task<IActionResult> UpdateVillaNumber(int id)
        {
            var villaNumberUpdateVm = new VillaNumberUpdateVm();
            var response = await _villaNumberService.GetAsync<ApiResponse>(id);
            if (response is not { IsSuccess: true }) return View();

            var model = JsonConvert.DeserializeObject<VillaNumberDto>(response.Result.ToString() ?? string.Empty);

            villaNumberUpdateVm.VillaNumber = _mapper.Map<VillaNumberUpdateDto>(model);

            response = await _villaService.GetVillaAsync<ApiResponse>();
            if (response is { IsSuccess: true })
            {
                //we should throw an exception if serialization property doesn't match 
                villaNumberUpdateVm.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
            return View(villaNumberUpdateVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVm model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.UpdateAsync<ApiResponse>(model.VillaNumber);
                if (response is { IsSuccess: true })
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
            }

            return View(model);
        }

        public async Task<IActionResult> DeleteVillaNumber(int id)
        {
            var villaNumberDeleteVm = new VillaNumberDeleteVm();

            var response = await _villaNumberService.GetAsync<ApiResponse>(id);
            if (response is not { IsSuccess: true }) return NotFound();


            villaNumberDeleteVm.VillaNumber = JsonConvert.DeserializeObject<VillaNumberDto>(response.Result.ToString() ?? string.Empty);

            response = await _villaService.GetVillaAsync<ApiResponse>();
            if (response is { IsSuccess: true })
            {
                //we should throw an exception if serialization property doesn't match 
                villaNumberDeleteVm.VillaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
            return View(villaNumberDeleteVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVm model)
        {
            var response = await _villaNumberService.DeleteAsync<ApiResponse>(model.VillaNumber.VillaNo);
            if (response is { IsSuccess: true })
            {
                return RedirectToAction(nameof(IndexVillaNumber));
            }

            return View(model);
        }
    }
}
