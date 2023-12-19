using AutoMapper;
using MagicVilla_API.Logging;
using MagicVilla_API.Models;
using MagicVilla_API.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_API.Controllers.v2
{
    [Route("api/v{version:apiVersion}/VillaNumberAPI")]
    [ApiController]
    [ApiVersion("2.0")]
    public class VillaNumberAPIController : ControllerBase
    {

        protected APIResponse _response;
        private readonly ILogging _logger;
        private readonly IVillaRepository _dbvilla;
        private readonly IVillaNumberRepository _dbVillaNumber;
        private readonly IMapper _mapper;
        public VillaNumberAPIController(ILogging logger, IVillaNumberRepository dbVillaNumber, IMapper mapper, IVillaRepository dbVilla)
        {
            _logger = logger;
            _dbVillaNumber = dbVillaNumber;
            _mapper = mapper;
            _response = new();
            _dbvilla = dbVilla;
        }

        //[MapToApiVersion("2.0")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

    }
}
