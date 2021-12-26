using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkyAPI.Models;
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

        [HttpGet("{id:int}", Name = "GetNationalPark")]
        public IActionResult GetNationalPark(int id)
        {
            var np = _nationalParkRepository.GetNationalPark(id);
            if (np == null)
            {
                return NotFound();
            }
            var npDto = _mapper.Map<NationalParkDto>(np);
            return Ok(npDto);
        }

        [HttpPost]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_nationalParkRepository.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("Error", "National park already exists");
                return BadRequest(ModelState);
            }

            var nationalPark = _mapper.Map<NationalPark>(nationalParkDto);
            if (!_nationalParkRepository.CreateNationalPark(nationalPark))
            {
                ModelState.AddModelError("Error", $"Something went wrong when saving {nationalPark.Name}");
                return StatusCode(500, ModelState);
            }

            var date = DateTime.Now;
            _logger.LogInformation("[{date}] National park added: {nationalPark.Name} ({nationalPark.Id})", date, nationalPark.Name, nationalPark.Id);
            return CreatedAtRoute("GetNationalPark", new {id = nationalPark.Id}, nationalPark);
        }

        [HttpPatch("{id:int}")]
        public IActionResult UpdateNationalPark(int id, [FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null || id != nationalParkDto.Id)
            {
                return BadRequest(ModelState);
            }

            var nationalPark = _mapper.Map<NationalPark>(nationalParkDto);
            if (!_nationalParkRepository.UpdateNationalPark(nationalPark))
            {
                ModelState.AddModelError("Error", $"Something went wrong when updating {nationalPark.Name}");
                return StatusCode(500, ModelState);
            }

            var date = DateTime.Now;
            _logger.LogInformation("[{date}] National park updated: {nationalPark.Name} ({nationalPark.Id})", date, nationalPark.Name, nationalPark.Id);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteNationalPark(int id)
        {
            var nationalPark = _nationalParkRepository.GetNationalPark(id);
            if (nationalPark == null)
            {
                return NotFound();
            }

            if (!_nationalParkRepository.DeleteNationalPark(nationalPark))
            {
                ModelState.AddModelError("Error", $"Something went wrong when deleting {nationalPark.Name}");
                return StatusCode(500, ModelState);
            }
            var date = DateTime.Now;
            _logger.LogInformation("[{date}] National park deleted: {nationalPark.Name} ({nationalPark.Id})", date, nationalPark.Name, nationalPark.Id);
            return NoContent();
        }
    }
}