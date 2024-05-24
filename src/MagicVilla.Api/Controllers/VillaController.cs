using MagicVilla.Api.Data;
using MagicVilla.Api.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.Api.Controllers
{
    [Route("api/Villas")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;

        public VillaController(ILogger<VillaController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            _logger.LogDebug("Getting a list of villa");
            return Ok(VillaStore.villaList);
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            if (villa == null)
            {
                _logger.LogWarning($"Villa doesn't found against the id {id}");
                return NotFound();
            }

            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<VillaDto> CreateVilla([FromBody] VillaDto villaDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("One or more field are missing ", ModelState.Values);
                return BadRequest();
            }

            if (VillaStore.villaList.FirstOrDefault(x => x.Name.ToLower() == villaDto.Name.ToLower())!= null)
            {
                _logger.LogWarning("Duplicate Villa name");
                 ModelState.AddModelError("CustomError", "Villa already exist");
                 return BadRequest(ModelState);
            }
            if (villaDto.Id > 0)
            {
                _logger.LogWarning("User Id can't be zero");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            villaDto.Id = VillaStore.villaList.MaxBy(x => x.Id)!.Id + 1;
            VillaStore.villaList.Add(villaDto);
            _logger.LogInformation("New villa added");
            return CreatedAtRoute("GetVilla", new { id = villaDto.Id }, villaDto);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogInformation("Invalid Villa Id");
                return BadRequest(); 
            }

            var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            if (villa == null)
            {
                _logger.LogWarning($"Villa doesn't exist against this id {id}");
                return NotFound();
            }
            VillaStore.villaList.Remove(villa);
            _logger.LogInformation("Villa has removed successfully");

            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDto villaDto)
        {
            if (id == 0 || id!= villaDto.Id)
            {
                _logger.LogWarning("Invalid villa request");
                return BadRequest();
            }

            var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            if (villa == null)
            {
                _logger.LogWarning("Villa not found");
                return NotFound();
            }

            villa.Name = villaDto.Name;
            villa.Occupency = villaDto.Occupency;
            villa.Sqft = villaDto.Sqft;

            _logger.LogWarning("Villa updated successfully");

            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDto>? patchDto)
        {
            if (patchDto == null || id == 0)
            {
                _logger.LogWarning("Invalid input");
                return BadRequest();
            }

            var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            if (villa == null)
            {
                return BadRequest();
            }

            patchDto.ApplyTo(villa, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();
        }
    }
}
