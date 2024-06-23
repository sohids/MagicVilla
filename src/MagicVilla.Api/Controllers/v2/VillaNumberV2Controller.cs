using Asp.Versioning;
using AutoMapper;
using MagicVilla.Api.Controllers.v1;
using MagicVilla.Api.Models;
using MagicVilla.Api.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.Api.Controllers.v2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/VillaNumbers")]
    [ApiController]
    public class VillaNumberV2Controller : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly IVillaNumberRepository _villaNumberRepository;
        private readonly IVillaRepository _villaRepository;
        private readonly IMapper _mapper;
        private readonly ApiResponse _response;

        public VillaNumberV2Controller(ILogger<VillaController> logger, IVillaNumberRepository villaNumberRepository,
            IMapper mapper, IVillaRepository villaRepository)
        {
            _logger = logger;
            _villaNumberRepository = villaNumberRepository;
            _mapper = mapper;
            _response = new ApiResponse();
            _villaRepository = villaRepository;
        }

        // [MapToApiVersion("2.0")] is not necessary when the version is separate 
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
