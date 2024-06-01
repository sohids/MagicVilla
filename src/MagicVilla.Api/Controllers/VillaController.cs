using MagicVilla.Api.Data;
using MagicVilla.Api.Models;
using MagicVilla.Api.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            _logger.LogDebug("Getting a list of villa");
            var result = await _dbContext.Villas.ToListAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task< ActionResult<VillaDto>> GetVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var villa = await _dbContext.Villas.FirstOrDefaultAsync(x => x.Id == id);
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
        public ActionResult<VillaCreateDto> CreateVilla([FromBody] VillaCreateDto villaDto)
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

            var model = new Villa
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

            _dbContext.Villas?.Add(model);
            _dbContext.SaveChanges();

            _logger.LogInformation("New villa added");
            return CreatedAtRoute("GetVilla", new { id = model.Id }, model);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogInformation($"Invalid Villa Id = {id}");
                return BadRequest();
            }

            var villa = await _dbContext.Villas.FirstOrDefaultAsync(x => x.Id == id);
            if (villa == null)
            {
                _logger.LogWarning($"Villa doesn't exist against this id {id}");
                return NotFound();
            }
            _dbContext.Villas?.Remove(villa);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Villa with id = {id} has removed successfully");

            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto villaDto)
        {
            if (id == 0 || id != villaDto.Id)
            {
                _logger.LogWarning($"Request id = {id} and Dto Id = {villaDto.Id} don't match");
                return BadRequest();
            }

            var villa = await _dbContext.Villas.FirstOrDefaultAsync(x => x.Id == id);
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

             await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Villa updated successfully");

            return NoContent();
        }

        //There is an issue with patch request right now, it shouldn't work properly 
        //fix it using this: https://www.udemy.com/course/restful-api-with-asp-dot-net-core-web-api/learn/lecture/33346200#notes
         
        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task< IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto>? patchDto)
        {
            if (patchDto == null || id == 0)
            {
                _logger.LogWarning($"input is null or request id {id} is invalid");
                return BadRequest();
            }

            var villa = await _dbContext.Villas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (villa == null)
            {
                _logger.LogWarning($"Villa not found against the id = {id}");

                return BadRequest();
            }

            var villaDto = new VillaUpdateDto
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

            var model = new Villa
            {
                Name = villaDto.Name,
                Occupancy = villaDto.Occupancy,
                SqFt = villaDto.SqFt,
                Details = villaDto.Details,
                Id = villaDto.Id,
                Rate = villaDto.Rate,
                ImageUrl = villaDto.ImageUrl,
                Amenity = villaDto.Amenity
            };

            _dbContext.Villas.Update(model);
            await _dbContext.SaveChangesAsync();

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
