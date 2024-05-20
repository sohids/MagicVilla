using MagicVilla.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.Api.Controllers
{
    [Route("api/Villas")]
    [ApiController]
    public class VillaController: ControllerBase
    {
        private readonly ILogger<VillaController> _logger;

        public VillaController(ILogger<VillaController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IEnumerable<Villa> GetVillas()
        {
            _logger.LogDebug("Getting a list of villa");
            return new List<Villa>
            {
                new Villa { Id = 1, Name = "Pool View" },
                new Villa { Id = 1, Name = "Beach View" },
            };
        }
    }
}
