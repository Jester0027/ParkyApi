using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserLoginDto model)
        {
            var user = _userRepository.Authenticate(model.Username, model.Password);
            if (user == null)
            {
                return BadRequest(new {message = "Username or password is incorrect"});
            }

            return Ok(_mapper.Map<UserDto>(user));
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterDto model)
        {
            if (!_userRepository.IsUserUnique(model.Username))
            {
                return BadRequest(new {message = "Username already exists"});
            }
            var user = _userRepository.Register(model.Username, model.Password);
            if (user == null)
            {
                return StatusCode(500, new {message = "Error while registering"});
            }
            return Ok(_mapper.Map<UserDto>(user));
        }
    }
}