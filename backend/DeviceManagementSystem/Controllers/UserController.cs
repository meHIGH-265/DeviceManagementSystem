using DeviceManagementSystem.Domain;
using DeviceManagementSystem.Repository;
using DeviceManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(users);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            var id = await _userRepository.CreateAsync(user);
            user.Id = id;

            return CreatedAtAction(nameof(GetById), new { id }, user);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, User user)
        {
            if (id != user.Id)
                return BadRequest();

            var updated = await _userRepository.UpdateAsync(user);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _userRepository.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request, [FromServices] JwtService jwtService)
        {
            var user = await _userRepository.GetByEmailAsync(
                request.Email,
                request.PasswordHash
            );

            if (user == null)
                return Unauthorized();

            var token = jwtService.GenerateToken(user);

            return Ok(new
            {
                token = token,
                user = user
            });
        }

        [HttpGet("by_email/{email}")]
        public async Task<IActionResult> GetByEmail(string email, [FromServices] JwtService jwtService)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
                return Ok(false);

            return Ok(true);
        }
    }
}
