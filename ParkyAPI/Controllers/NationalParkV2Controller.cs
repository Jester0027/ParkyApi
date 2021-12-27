using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [Route("api/v{version:apiVersion}/national-parks")]
    [ApiVersion("2.0")]
    [ApiController]
    public class NationalParkV2Controller : ControllerBase
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;

        public NationalParkV2Controller(
            INationalParkRepository nationalParkRepository,
            IMapper mapper
        )
        {
            _nationalParkRepository = nationalParkRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<NationalParkDto>))]
        public IActionResult GetNationalParks()
        {
            var nationalPark = _nationalParkRepository.GetNationalParks().FirstOrDefault();
            return Ok(nationalPark);
        }
    }
}