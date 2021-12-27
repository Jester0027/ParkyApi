using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    // [Route("/api/[controller]")]
    [Route("api/v{version:apiVersion}/trails")]
    [ApiController]
    // [ApiExplorerSettings(GroupName = "v1-trail")]
    public class TrailController : ControllerBase
    {
        private readonly ITrailRepository _trailRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TrailController> _logger;

        public TrailController(
            ITrailRepository trailRepository,
            IMapper mapper,
            ILogger<TrailController> logger
        )
        {
            _trailRepository = trailRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TrailDto>))]
        public IActionResult GetTrails()
        {
            var trails = _trailRepository.GetTrails();
            var dtos = trails.Select(trail => _mapper.Map<TrailDto>(trail)).ToList();
            return Ok(dtos);
        }

        [HttpGet("{id:int}", Name = "GetTrail")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        public IActionResult GetTrail(int id)
        {
            var trail = _trailRepository.GetTrail(id);
            if (trail == null)
            {
                return NotFound();
            }

            var trailDto = _mapper.Map<TrailDto>(trail);
            return Ok(trailDto);
        }
        
        [HttpGet("[action]/{nationalParkId:int}", Name = "GetTrailsInNationalPark")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        public IActionResult GetTrailsInNationalPark(int nationalParkId)
        {
            var trails = _trailRepository.GetTrailsInNationalPark(nationalParkId);
            var trailsDto = trails.Select(trail => _mapper.Map<TrailDto>(trail));
            return Ok(trailsDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(NationalParkDto))]
        [ProducesResponseType(400)]
        public IActionResult CreateTrail([FromBody] TrailCreateDto trailDto)
        {
            if (trailDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_trailRepository.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("Error", "Trail already exists");
                return BadRequest(ModelState);
            }

            var trail = _mapper.Map<Trail>(trailDto);
            if (!_trailRepository.CreateTrail(trail))
            {
                ModelState.AddModelError("Error", $"Something went wrong when saving {trail.Name}");
                return StatusCode(500, ModelState);
            }

            var date = DateTime.Now;
            _logger.LogInformation("[{date}] Trail added: {trail.Name} ({trail.Id})", date, trail.Name, trail.Id);
            return CreatedAtRoute("GetNationalPark", new {id = trail.Id}, trail);
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdateTrail(int id, [FromBody] TrailUpdateDto trailDto)
        {
            if (trailDto == null || id != trailDto.Id)
            {
                return BadRequest(ModelState);
            }

            var trail = _mapper.Map<Trail>(trailDto);
            if (!_trailRepository.UpdateTrail(trail))
            {
                ModelState.AddModelError("Error", $"Something went wrong when updating {trail.Name}");
                return StatusCode(500, ModelState);
            }

            var date = DateTime.Now;
            _logger.LogInformation("[{date}] Trail updated: {trail.Name} ({trail.Id})", date, trail.Name, trail.Id);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteTrail(int id)
        {
            var trail = _trailRepository.GetTrail(id);
            if (trail == null)
            {
                return NotFound();
            }

            if (!_trailRepository.DeleteTrail(trail))
            {
                ModelState.AddModelError("Error", $"Something went wrong when deleting {trail.Name}");
                return StatusCode(500, ModelState);
            }

            var date = DateTime.Now;
            _logger.LogInformation("[{date}] Trail deleted: {trail.Name} ({trail.Id})", date, trail.Name, trail.Id);
            return NoContent();
        }
    }
}