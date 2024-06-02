using System.Net;
using AutoMapper;
using MagicVilla.Api.Models;
using MagicVilla.Api.Models.Dto;
using MagicVilla.Api.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.Api.Controllers
{
    [Route("api/Villas")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly IVillaRepository _villaRepository;
        private readonly IMapper _mapper;
        protected readonly ApiResponse _response;

        public VillaController(ILogger<VillaController> logger, IVillaRepository villaRepository, IMapper mapper)
        {
            _logger = logger;
            _villaRepository = villaRepository;
            _mapper = mapper;
            _response = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetVillas()
        {
            _logger.LogDebug("Getting a list of villa");
            var villaList = await _villaRepository.GetAllAsync();
            _response.Result = _mapper.Map<List<VillaDto>>(villaList);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{id}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetVilla(int id)
        {
            if (id == 0) return BadRequest();

            var villa = await _villaRepository.GetAsync(x=>x.Id == id);
            if (villa == null)
            {
                _logger.LogWarning($"Villa doesn't found against the id {id}");
                return NotFound();
            }

            _response.Result = _mapper.Map<VillaDto>(villa);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> CreateVilla([FromBody] VillaCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("One or more field are missing ");
                return BadRequest();
            }

            if (await _villaRepository.GetAsync(x => x.Name.ToLower() == createDto.Name.ToLower()) != null)
            {
                _logger.LogWarning("Duplicate Villa name");
                ModelState.AddModelError("CustomError", "Villa already exist");
                return BadRequest(ModelState);
            }

            var villa = _mapper.Map<Villa>(createDto);

            await _villaRepository.CreateAsync(villa);

            _response.Result = _mapper.Map<VillaDto>(villa);
            _response.StatusCode = HttpStatusCode.Created;

            _logger.LogInformation("New villa added");
            return CreatedAtRoute("GetVilla", new { id = villa.Id }, _response);
        }

        [HttpDelete("{id}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<ActionResult<ApiResponse>> DeleteVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogInformation($"Invalid Villa Id = {id}");
                return BadRequest();
            }

            var villa = await _villaRepository.GetAsync(x => x.Id == id);
            if (villa == null)
            {
                _logger.LogWarning($"Villa doesn't exist against this id {id}");
                return NotFound();
            }

            await _villaRepository.RemoveAsync(villa);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            _logger.LogInformation($"Villa with id = {id} has removed successfully");
            return Ok();
        }

        [HttpPut("{id}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<ApiResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {
            if (id == 0 || id != updateDto.Id)
            {
                _logger.LogWarning($"Request id = {id} and Dto Id = {updateDto.Id} don't match");
                return BadRequest();
            }
           
            var villa = _mapper.Map<Villa>(updateDto);
            await _villaRepository.UpdateAsync(villa);
            _logger.LogInformation("Villa updated successfully");

            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;

            return Ok(_response);
        }

        [HttpPatch("{id}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto>? patchDto)
        {
            if (patchDto == null || id == 0)
            {
                _logger.LogWarning($"input is null or request id {id} is invalid");
                return BadRequest();
            }

            var villa = await _villaRepository.GetAsync(x => x.Id == id);
            if (villa == null)
            {
                _logger.LogWarning($"Villa not found against the id = {id}");

                return BadRequest();
            }

            var villaUpdateDto = _mapper.Map<VillaUpdateDto>(villa);
            patchDto.ApplyTo(villaUpdateDto, ModelState);

            var model = _mapper.Map<Villa>(villaUpdateDto);
            await _villaRepository.UpdateAsync(model);

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
