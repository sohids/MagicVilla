using System.Net;
using AutoMapper;
using MagicVilla.Api.Models;
using MagicVilla.Api.Models.Dto;
using MagicVilla.Api.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.Api.Controllers
{
    [Route("api/VillaNumbers")]
    [ApiController]
    public class VillaNumberController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly IVillaNumberRepository _villaNumberRepository;
        private readonly IMapper _mapper;
        protected readonly ApiResponse _response;

        public VillaNumberController(ILogger<VillaController> logger, IVillaNumberRepository villaNumberRepository, IMapper mapper)
        {
            _logger = logger;
            _villaNumberRepository = villaNumberRepository;
            _mapper = mapper;
            _response = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetVillas()
        {
            _logger.LogDebug("Getting a list of villa numbers");
            var villaNumbers = await _villaNumberRepository.GetAllAsync();
            _response.Result = _mapper.Map<List<VillaNumberDto>>(villaNumbers);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetVilla(int id)
        {
            if (id == 0) return BadRequest();

            var villaNumber = await _villaNumberRepository.GetAsync(x=>x.VillaNo == id);
            if (villaNumber == null)
            {
                _logger.LogWarning($"Villa doesn't found against the id {id}");
                return NotFound();
            }

            _response.Result = _mapper.Map<VillaNumberDto>(villaNumber);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("One or more field are missing ");
                return BadRequest();
            }

            if (await _villaNumberRepository.GetAsync(x => x.VillaNo == createDto.VillaNumber) != null)
            {
                _logger.LogWarning("Duplicate Villa name");
                ModelState.AddModelError("CustomError", "Villa already exist");
                return BadRequest(ModelState);
            }

            var villaNumber = _mapper.Map<VillaNumber>(createDto);

            await _villaNumberRepository.CreateAsync(villaNumber);

            _response.Result = _mapper.Map<VillaNumberCreateDto>(villaNumber);
            _response.StatusCode = HttpStatusCode.Created;

            _logger.LogInformation("New villa number added");
            return CreatedAtRoute("GetVillaNumber", new { id = villaNumber.VillaNo }, _response);
        }

        [HttpDelete("{id}", Name = "DeleteVillaNumber")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<ActionResult<ApiResponse>> DeleteVillaNumber(int id)
        {
            if (id == 0)
            {
                _logger.LogInformation($"Invalid Villa Id = {id}");
                return BadRequest();
            }

            var villaNumber = await _villaNumberRepository.GetAsync(x => x.VillaNo == id);
            if (villaNumber == null)
            {
                _logger.LogWarning($"Villa doesn't exist against this id {id}");
                return NotFound();
            }

            await _villaNumberRepository.RemoveAsync(villaNumber);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            _logger.LogInformation($"Villa with id = {id} has removed successfully");
            return Ok();
        }

        [HttpPut("{id}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<ApiResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDto updateDto)
        {
            if (id == 0 || id != updateDto.VillaNumber)
            {
                _logger.LogWarning($"Request id = {id} and Dto Id = {updateDto.Id} don't match");
                return BadRequest();
            }
           
            var villaNumber = _mapper.Map<VillaNumber>(updateDto);
            await _villaNumberRepository.UpdateAsync(villaNumber);
            _logger.LogInformation("Villa updated successfully");

            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;

            return Ok(_response);
        }

        [HttpPatch("{id}", Name = "UpdatePartialVillaNumber")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaNumberUpdateDto>? patchDto)
        {
            if (patchDto == null || id == 0)
            {
                _logger.LogWarning($"input is null or request id {id} is invalid");
                return BadRequest();
            }

            var villa = await _villaNumberRepository.GetAsync(x => x.VillaNo == id);
            if (villa == null)
            {
                _logger.LogWarning($"Villa not found against the id = {id}");

                return BadRequest();
            }

            var villaNumberUpdate = _mapper.Map<VillaNumberUpdateDto>(villa);
            patchDto.ApplyTo(villaNumberUpdate, ModelState);

            var model = _mapper.Map<VillaNumber>(villaNumberUpdate);
            await _villaNumberRepository.UpdateAsync(model);

            if (ModelState.IsValid)
            {
                _logger.LogWarning("Field updated successfully");
                return NoContent();
            }

            _logger.LogWarning("ModelState seems to be invalid");
            return BadRequest(ModelState);
        }
    }
}
