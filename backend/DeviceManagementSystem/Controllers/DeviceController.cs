using DeviceManagementSystem.Domain;
using DeviceManagementSystem.Repository;
using DeviceManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceRepository _deviceRepository;

        public DeviceController(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var devices = await _deviceRepository.GetAllAsync();
            return Ok(devices);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var device = await _deviceRepository.GetByIdAsync(id);

            if (device == null)
                return NotFound();

            return Ok(device);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(Device device)
        {
            var id = await _deviceRepository.CreateAsync(device);
            device.Id = id;

            return CreatedAtAction(nameof(GetById), new { id }, device);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Device device)
        {
            if (id != device.Id)
                return BadRequest();

            var updated = await _deviceRepository.UpdateAsync(device);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _deviceRepository.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }

        // BONUS: Get devices by user
        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var devices = await _deviceRepository.GetByUserIdAsync(userId);
            return Ok(devices);
        }

        [Authorize]
        [HttpPost("generate-description")]
        public async Task<IActionResult> GenerateDescription([FromBody] Device device, [FromServices] AiService aiService)
        {
            if (device == null)
            {
                return BadRequest();
            }

            var description = await aiService.GenerateDeviceDescription(device);

            return Ok(new { description });
        }

        [Authorize]
        [HttpGet("search/{query}")]
        public async Task<IActionResult> RankDevicesByMatch(string query, [FromServices] DeviceSearchService deviceSearchService)
        {
            var devices = await _deviceRepository.GetAllAsync();

            var orderedDevices = devices
                .Select(d => new
                {
                    Device = d,
                    Score = deviceSearchService.CalculateScore(d, query)
                })
                .OrderByDescending(x => x.Score)
                // .Select(x => x.Device)
                .ToList();

            return Ok(orderedDevices);
        }
    }
}
