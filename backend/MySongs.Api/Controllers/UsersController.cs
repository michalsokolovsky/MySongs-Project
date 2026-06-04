using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MySongs.Common.DTOs;
using MySongs.Services.Interfaces;

namespace MySongs.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(_mapper.Map<List<UserResponseDto>>(users));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetById(id);
            if (user == null) return NotFound();
            return Ok(_mapper.Map<UserResponseDto>(user));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UserDto user)
        {
            user.UserId = id;
            await _userService.Update(user);
            return Ok("המשתמש עודכן");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.Delete(id);
            return Ok("המשתמש נמחק");
        }
    }
}