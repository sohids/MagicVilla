using System.Net;
using Asp.Versioning;
using AutoMapper;
using MagicVilla.Api.Models;
using MagicVilla.Api.Models.Dto;
using MagicVilla.Api.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/VillaNumbers")]
    [ApiController]
    public class VillaNumberV1Controller : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly IVillaNumberRepository _villaNumberRepository;
        private readonly IVillaRepository _villaRepository;
        private readonly IMapper _mapper;
        private readonly ApiResponse _response;

        public VillaNumberV1Controller(ILogger<VillaController> logger, IVillaNumberRepository villaNumberRepository,
            IMapper mapper, IVillaRepository villaRepository)
        {
            _logger = logger;
            _villaNumberRepository = villaNumberRepository;
            _mapper = mapper;
            _response = new ApiResponse();
            _villaRepository = villaRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetVillaNumbers()
        {
            try
            {
                _logger.LogDebug("Getting a list of villa numbers");
                var villaNumbers = await _villaNumberRepository.GetAllAsync(includeProperties:"Villa");
                _response.Result = _mapper.Map<List<VillaNumberDto>>(villaNumbers);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [MapToApiVersion("1.0")]

        [HttpGet("{id}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetVillaNumber(int id)
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var villaNumber = await _villaNumberRepository.GetAsync(x=>x.VillaNo == id);
            if (villaNumber == null)
            {
                _logger.LogWarning($"Villa doesn't found against the id {id}");
                _response.StatusCode = HttpStatusCode.NoContent;
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
            try
            {
                if (await _villaNumberRepository.GetAsync(x => x.VillaNo == createDto.VillaNo) != null)
                {
                    _logger.LogWarning("Duplicate Villa name");
                    ModelState.AddModelError("ErrorMessage", "Villa Number already exist");
                    return BadRequest(ModelState);
                }
                if (await _villaRepository.GetAsync(x => x.Id == createDto.VillaId) == null)
                {
                    ModelState.AddModelError("ErrorMessage", "Villa Id is invalid");
                    return BadRequest(ModelState);
                }

                var villaNumber = _mapper.Map<VillaNumber>(createDto);

                await _villaNumberRepository.CreateAsync(villaNumber);

                _response.Result = _mapper.Map<VillaNumberCreateDto>(villaNumber);
                _response.StatusCode = HttpStatusCode.Created;

                _logger.LogInformation("New villa number added");
                return CreatedAtRoute("GetVillaNumber", new { id = villaNumber.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string> { ex.ToString() };
            }
            return _response;
            
        }

        [HttpDelete("{id}", Name = "DeleteVillaNumber")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<ActionResult<ApiResponse>> DeleteVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogInformation($"Invalid Villa Id = {id}");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var villaNumber = await _villaNumberRepository.GetAsync(x => x.VillaNo == id);
                if (villaNumber == null)
                {
                    _logger.LogWarning($"Villa doesn't exist against this id {id}");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return NotFound(_response);
                }

                await _villaNumberRepository.RemoveAsync(villaNumber);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                _logger.LogInformation($"Villa with id = {id} has removed successfully");
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string> { ex.ToString() };
            }

            return _response;

        }

        [HttpPut("{id}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<ApiResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDto updateDto)
        {
            try
            {
                if (id == 0 || id != updateDto.VillaNo)
                {
                    _logger.LogWarning($"Request id = {id} and Dto Id = {updateDto.VillaNo} don't match");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (await _villaRepository.GetAsync(x => x.Id == updateDto.VillaId) == null)
                {
                    ModelState.AddModelError("CustomError", "Villa Id is invalid");
                    return BadRequest(ModelState);
                }

                var villaNumber = _mapper.Map<VillaNumber>(updateDto);
                await _villaNumberRepository.UpdateAsync(villaNumber);
                _logger.LogInformation("Villa updated successfully");

                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string> { ex.ToString() };
            }

            return _response;

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
