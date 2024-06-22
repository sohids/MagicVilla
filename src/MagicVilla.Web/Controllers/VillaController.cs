using AutoMapper;
using MagicVilla.Web.Models;
using MagicVilla.Web.Models.Dto;
using MagicVilla.Web.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace MagicVilla.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;
        private readonly ILogger<VillaController> _logger;

        public VillaController(IVillaService villaService, IMapper mapper, ILogger<VillaController> logger)
        {
            _villaService = villaService;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IActionResult> IndexVilla()
        {
            var list = new List<VillaDto>();
            var response = await _villaService.GetVillaAsync<ApiResponse>();
            if (response != null)
            {
                list = JsonConvert.DeserializeObject<List<VillaDto>>(response.Result.ToString() ?? string.Empty);
            }
            return View(list);
        }
        [Authorize(Roles = "admin")]
        public IActionResult CreateVilla()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateVilla(VillaCreateDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaService.CreateAsync<ApiResponse>(model);
                if (response is { IsSuccess: true })
                {
                    return RedirectToAction(nameof(IndexVilla));
                }
            }

            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVilla(int id)
        {
            var response = await _villaService.GetAsync<ApiResponse>(id);
            if (response is not { IsSuccess: true }) return View();

            var model = JsonConvert.DeserializeObject<VillaDto>(response.Result.ToString() ?? string.Empty);

            return View(_mapper.Map<VillaUpdateDto>(model));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> UpdateVilla(VillaUpdateDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaService.UpdateAsync<ApiResponse>(model);
                if (response is { IsSuccess: true })
                {
                    return RedirectToAction(nameof(IndexVilla));
                }
            }

            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            var response = await _villaService.GetAsync<ApiResponse>(id);
            if (response is not { IsSuccess: true }) return NotFound();

            var model = JsonConvert.DeserializeObject<VillaDto>(response.Result.ToString() ?? string.Empty);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVilla(VillaDto model)
        {
            var response = await _villaService.DeleteAsync<ApiResponse>(model.Id);
            if (response is { IsSuccess: true })
            {
                return RedirectToAction(nameof(IndexVilla));
            }

            return View(model);
        }
    }
}
