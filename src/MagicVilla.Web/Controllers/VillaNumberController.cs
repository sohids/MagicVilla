using AutoMapper;
using MagicVilla.Utility;
using MagicVilla.Web.Models;
using MagicVilla.Web.Models.Dto;
using MagicVilla.Web.Models.ViewModels;
using MagicVilla.Web.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace MagicVilla.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;
        private readonly ILogger<VillaNumberController> _logger;


        public VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper, IVillaService villaService, ILogger<VillaNumberController> logger)
        {
            _villaNumberService = villaNumberService;
            _mapper = mapper;
            _villaService = villaService;
            _logger = logger;
        }
        public async Task<IActionResult> IndexVillaNumber()
        {
            CheckSession();
            var list = new List<VillaNumberDto>();
            var response = await _villaNumberService.GetVillaAsync<ApiResponse>(HttpContext.Session.GetString(StaticDetails.SessionToken));
            if (response != null)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDto>>(response.Result.ToString() ?? string.Empty);
            }
            return View(list);
        }

        [Authorize(Roles = "admin")]

        public async Task<IActionResult> CreateVillaNumber()
        {
            var response = await _villaService.GetVillaAsync<ApiResponse>(HttpContext.Session.GetString(StaticDetails.SessionToken));
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
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVm model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.CreateAsync<ApiResponse>(model.VillaNumber, HttpContext.Session.GetString(StaticDetails.SessionToken));
                if (response is { IsSuccess: true })
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]

        public async Task<IActionResult> UpdateVillaNumber(int id)
        {
            var villaNumberUpdateVm = new VillaNumberUpdateVm();
            var response = await _villaNumberService.GetAsync<ApiResponse>(id, HttpContext.Session.GetString(StaticDetails.SessionToken));
            if (response is not { IsSuccess: true }) return View();

            var model = JsonConvert.DeserializeObject<VillaNumberDto>(response.Result.ToString() ?? string.Empty);

            villaNumberUpdateVm.VillaNumber = _mapper.Map<VillaNumberUpdateDto>(model);

            response = await _villaService.GetVillaAsync<ApiResponse>(HttpContext.Session.GetString(StaticDetails.SessionToken));
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
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVm model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.UpdateAsync<ApiResponse>(model.VillaNumber, HttpContext.Session.GetString(StaticDetails.SessionToken));
                if (response is { IsSuccess: true })
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
            }

            return View(model);
        }

        [Authorize(Roles = "admin")]

        public async Task<IActionResult> DeleteVillaNumber(int id)
        {
            var villaNumberDeleteVm = new VillaNumberDeleteVm();

            var response = await _villaNumberService.GetAsync<ApiResponse>(id, HttpContext.Session.GetString(StaticDetails.SessionToken));
            if (response is not { IsSuccess: true }) return NotFound();


            villaNumberDeleteVm.VillaNumber = JsonConvert.DeserializeObject<VillaNumberDto>(response.Result.ToString() ?? string.Empty);

            response = await _villaService.GetVillaAsync<ApiResponse>(HttpContext.Session.GetString(StaticDetails.SessionToken));
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
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVm model)
        {
            var response = await _villaNumberService.DeleteAsync<ApiResponse>(model.VillaNumber.VillaNo, HttpContext.Session.GetString(StaticDetails.SessionToken));
            if (response is { IsSuccess: true })
            {
                return RedirectToAction(nameof(IndexVillaNumber));
            }

            return View(model);
        }

        private void CheckSession()
        {
            if (HttpContext.Session.TryGetValue(StaticDetails.SessionToken, out byte[] value))
            {
                // Session is set
                var token = Encoding.UTF8.GetString(value);
                _logger.LogInformation("Session is set: "+ token);
            }
            else
            {
                // Session is not set
            }
        }
    }
}
