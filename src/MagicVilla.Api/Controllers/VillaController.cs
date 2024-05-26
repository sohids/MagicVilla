using MagicVilla.Api.Data;
using MagicVilla.Api.Models;
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
        private readonly ApplicationDbContext _dbContext;

        public VillaController(ILogger<VillaController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            _logger.LogDebug("Getting a list of villa");
            return Ok(_dbContext.Villas?.ToList());
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

            var villa = _dbContext.Villas?.FirstOrDefault(x => x.Id == id);
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

            if (_dbContext.Villas?.FirstOrDefault(x => x.Name.ToLower() == villaDto.Name.ToLower()) != null)
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

            var villa = new Villa
            {
                Name = villaDto.Name,
                Details = villaDto.Details,
                ImageUrl = villaDto.ImageUrl,
                Occupancy = villaDto.Occupancy,
                Rate = villaDto.Rate,
                SqFt = villaDto.SqFt,
                Amenity = villaDto.Amenity,
                CreatedDate = DateTime.Now
            };

            _dbContext.Villas?.Add(villa);
            _dbContext.SaveChanges();

            _logger.LogInformation("New villa added");
            return CreatedAtRoute("GetVilla", new { id = villaDto.Id }, villaDto);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogInformation($"Invalid Villa Id = {id}");
                return BadRequest();
            }

            var villa = _dbContext.Villas?.FirstOrDefault(x => x.Id == id);
            if (villa == null)
            {
                _logger.LogWarning($"Villa doesn't exist against this id {id}");
                return NotFound();
            }
            _dbContext.Villas?.Remove(villa);
            _dbContext.SaveChanges();

            _logger.LogInformation($"Villa with id = {id} has removed successfully");

            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDto villaDto)
        {
            if (id == 0 || id != villaDto.Id)
            {
                _logger.LogWarning($"Request id = {id} and Dto Id = {villaDto.Id} don't match");
                return BadRequest();
            }

            var villa = _dbContext.Villas?.FirstOrDefault(x => x.Id == id);
            if (villa == null)
            {
                _logger.LogWarning("Villa not found");
                return NotFound();
            }

            villa.Name = villaDto.Name;
            villa.Occupancy = villaDto.Occupancy;
            villa.SqFt = villaDto.SqFt;
            villa.Details = villaDto.Details;
            villa.Id = villaDto.Id;
            villa.Rate = villaDto.Rate;
            villa.ImageUrl = villaDto.ImageUrl;

            // _dbContext.Update(villa);
            _dbContext.SaveChanges();

            _logger.LogWarning("Villa updated successfully");

            return NoContent();
        }

        //There is an issue with patch request right now, it shouldn't work properly 
        //fix it using this: https://www.udemy.com/course/restful-api-with-asp-dot-net-core-web-api/learn/lecture/33346200#notes
        
        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDto>? patchDto)
        {
            if (patchDto == null || id == 0)
            {
                _logger.LogWarning($"input is null or request id {id} is invalid");
                return BadRequest();
            }

            var villa = _dbContext.Villas?.FirstOrDefault(x => x.Id == id);
            if (villa == null)
            {
                _logger.LogWarning($"Villa not found against the id = {id}");

                return BadRequest();
            }

            var villaDto = new VillaDto
            {
                Name = villa.Name,
                Occupancy = villa.Occupancy,
                SqFt = villa.SqFt,
                Details = villa.Details,
                Id = villa.Id,
                Rate = villa.Rate,
                ImageUrl = villa.ImageUrl,
            };

            patchDto.ApplyTo(villaDto, ModelState);


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
