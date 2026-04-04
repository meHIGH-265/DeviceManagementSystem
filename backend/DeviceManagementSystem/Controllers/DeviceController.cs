using DeviceManagementSystem.Domain;
using DeviceManagementSystem.Repository;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var devices = await _deviceRepository.GetAllAsync();
            return Ok(devices);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var device = await _deviceRepository.GetByIdAsync(id);

            if (device == null)
                return NotFound();

            return Ok(device);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Device device)
        {
            var id = await _deviceRepository.CreateAsync(device);
            device.Id = id;

            return CreatedAtAction(nameof(GetById), new { id }, device);
        }

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _deviceRepository.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }

        // BONUS: Get devices by user
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var devices = await _deviceRepository.GetByUserIdAsync(userId);
            return Ok(devices);
        }
    }
}
