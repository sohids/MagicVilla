using AutoMapper;
using MagicVilla.Web.Models;
using MagicVilla.Web.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using MagicVilla.Web.Models.Dto;

namespace MagicVilla.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public HomeController(IVillaService villaService, IMapper mapper)
        {
            _villaService = villaService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var list = new List<VillaDto>();
            var response = await _villaService.GetVillaAsync<ApiResponse>();
            if (response != null)
            {
                list = JsonConvert.DeserializeObject<List<VillaDto>>(response.Result.ToString() ?? string.Empty);
            }
            return View(list);
        }
    }
}
