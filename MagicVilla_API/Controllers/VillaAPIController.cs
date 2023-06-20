using AutoMapper;
using MagicVilla_API.Logging;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_API.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        //private readonly ILogger<VillaAPIController> _logger;
        //private readonly ApplicationDbContext _db;

        //public VillaAPIController(ILogger<VillaAPIController> logger)
        //{
        //    _logger = logger;
        //}

        protected APIResponse _response;
        private readonly ILogging _logger;
        private readonly IVillaRepository _dbVilla;
        private readonly IMapper _mapper;
        public VillaAPIController(ILogging logger, IVillaRepository dbVilla, IMapper mapper)
        {
            _logger = logger;
            _dbVilla = dbVilla;
            _mapper = mapper;
            this._response = new(); 
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
				_logger.Log("Getting all villas", "");
				//IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();
				IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync();
				_response.Result = _mapper.Map<List<VillaDTO>>(villaList);
				_response.StatusCode = HttpStatusCode.OK;
				return Ok(_response);
			}
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;

        }

        //If you do not define HTTP Verb,it defaults to "HTTPGET"
        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
				if (id == 0)
				{
					_logger.Log("Get Villa Error with Id" + id, "error");
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}
				//var villa = await _db.Villas.FirstOrDefaultAsync(u => u.Id == id);

				var villa = await _dbVilla.GetAsync(u => u.Id == id);
				if (villa == null)
				{
					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_response);
				}
				_response.Result = _mapper.Map<VillaDTO>(villa);
				_response.StatusCode = HttpStatusCode.OK;
				return Ok(_response);
			}
            catch (Exception ex)
            {
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string>() { ex.ToString() };
			}
			return _response;
		}

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO createDTO)
        {
            //if (ModelState.IsValid) {
            //    return BadRequest(ModelState);
            //}
            try
            {
				if (await _dbVilla.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
				{
					ModelState.AddModelError("CustomError", "Villa already Exists");
					return BadRequest(ModelState);
				}
				if (createDTO == null)
				{
					return BadRequest(createDTO);
				}
				Villa model = _mapper.Map<Villa>(createDTO);

				//Villa model = new()
				//{
				//    Amenity = createDTO.Amenity,
				//    Details = createDTO.Details,
				//    ImageUrl = createDTO.ImageUrl,
				//    Name = createDTO.Name,
				//    Occupancy = createDTO.Occupancy,
				//    Rate = createDTO.Rate,
				//    Sqft = createDTO.Sqft
				//};

				//await _db.Villas.AddAsync(model);
				//await _db.SaveChangesAsync();

				await _dbVilla.CreateAsync(model);
				_response.Result = _mapper.Map<VillaDTO>(model);
				_response.StatusCode = HttpStatusCode.Created;

				return CreatedAtRoute("GetVilla", new { id = model.Id }, _response);
			}
            catch (Exception ex)
            {
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string>() { ex.ToString() };
			}
			return _response;


		}

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
				if (id == 0)
				{
					return BadRequest();
				}

				var villa = await _dbVilla.GetAsync(u => u.Id == id);
				if (villa == null)
				{
					return NotFound();
				}
				//_db.Villas.Remove(villa);
				//await _db.SaveChangesAsync(true);

				await _dbVilla.RemoveAsync(villa);
				_response.StatusCode = HttpStatusCode.NoContent;
				_response.IsSuccess = true;
				return Ok(_response);
			}
            catch (Exception ex) { 
                _response.IsSuccess = false; 
                _response.ErrorMessages = new List<string>() { ex.ToString() }; 
            }
            return _response;
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO)
        {
            try
            {
				if (updateDTO == null || id != updateDTO.Id)
				{
					return BadRequest();
				}

				Villa model = _mapper.Map<Villa>(updateDTO);
				//Villa model = new()
				//{
				//    Amenity = updateDTO.Amenity,
				//    Details = updateDTO.Details,
				//    Id = updateDTO.Id,
				//    ImageUrl = updateDTO.ImageUrl,
				//    Name = updateDTO.Name,
				//    Occupancy = updateDTO.Occupancy,
				//    Rate = updateDTO.Rate,
				//    Sqft = updateDTO.Sqft
				//};
				//_db.Villas.Update(model);
				//await _db.SaveChangesAsync();

				await _dbVilla.UpdateAsync(model);
				_response.StatusCode = HttpStatusCode.NoContent;
				_response.IsSuccess = true;
				return Ok(_response);
			}
            catch (Exception ex) { 
                _response.IsSuccess = false; 
                _response.ErrorMessages = new List<string>() { ex.ToString() }; 
            }
			return _response;

		}

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDto)
        {
            try
            {
				if (patchDto == null || id == 0)
				{
					return BadRequest();
				}
				//var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
				var villa = await _dbVilla.GetAsync(u => u.Id == id, tracked: false);

				VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);


				if (villa == null)
				{
					return BadRequest();
				}
				patchDto.ApplyTo(villaDTO, ModelState);
				Villa model = _mapper.Map<Villa>(villaDTO);

				//_db.Villas.Update(model);
				//await _db.SaveChangesAsync();

				await _dbVilla.UpdateAsync(model);

				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}
				return NoContent();
			}
            catch (Exception ex)
            {
				_response.IsSuccess = false;
				_response.ErrorMessages = new List<string>() { ex.ToString() };
			}
			return _response;
			
        }

    }
}
