using AutoMapper;
using MagicVilla.Web.Models;
using MagicVilla.Web.Models.Dto;
using MagicVilla.Web.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public VillaController(IVillaService villaService, IMapper mapper)
        {
            _villaService = villaService;
            _mapper = mapper;
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

        public async Task<IActionResult> CreateVilla()
        {
            return View();
        }
        [HttpPost]
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
    }
}
