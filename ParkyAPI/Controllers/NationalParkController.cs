using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParkController : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<NationalParkController> _logger;

        public NationalParkController(
            INationalParkRepository nationalParkRepository,
            IMapper mapper,
            ILogger<NationalParkController> logger
        )
        {
            _nationalParkRepository = nationalParkRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetNationalParks()
        {
            var nationalParks = _nationalParkRepository.GetNationalParks();
            var dtos = nationalParks.Select(park => _mapper.Map<NationalParkDto>(park)).ToList();
            return Ok(dtos);
        }
    }
}