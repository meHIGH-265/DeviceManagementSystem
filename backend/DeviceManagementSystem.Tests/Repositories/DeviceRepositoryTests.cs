using DeviceManagementSystem.Domain;
using DeviceManagementSystem.Repository;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace DeviceManagementSystem.Tests.Repositories
{
    public class DeviceRepositoryTests
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IUserRepository _userRepository;

        public DeviceRepositoryTests()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Test.json")
                .Build();

            _deviceRepository = new DeviceRepository(config);
            _userRepository = new UserRepository(config);
        }

        [Fact]
        public async Task Create_Get_Delete_Device_Should_Work()
        {
            // Arrange
            var device = new Device
            {
                Name = "Test Device",
                Manufacturer = "Test",
                Type = "phone",
                OS = "TestOS"
            };

            // Act
            var id = await _deviceRepository.CreateAsync(device);
            var fetched = await _deviceRepository.GetByIdAsync(id);

            // Assert
            Assert.NotNull(fetched);
            Assert.Equal("Test Device", fetched!.Name);

            // Cleanup
            await _deviceRepository.DeleteAsync(id);
        }

        [Fact]
        public async Task Update_Device_Should_Work()
        {
            // Arrange
            var device = new Device
            {
                Name = "Initial",
                Manufacturer = "Test",
                Type = "phone",
                OS = "OS"
            };

            var id = await _deviceRepository.CreateAsync(device);

            // Act
            device.Id = id;
            device.Name = "Updated";

            var updated = await _deviceRepository.UpdateAsync(device);
            var fetched = await _deviceRepository.GetByIdAsync(id);

            // Assert
            Assert.True(updated);
            Assert.Equal("Updated", fetched!.Name);

            // Cleanup
            await _deviceRepository.DeleteAsync(id);
        }

        [Fact]
        public async Task GetByUserId_Should_Return_Only_User_Devices()
        {
            // Arrange
            User user1 = new User
            {
                Name = "Test User",
                Role = "Admin",
                Location = "Test",
                Email = Guid.NewGuid() + "@test.com", // avoid unique conflict
                PasswordHash = "hash"
            };

            User user2 = new User
            {
                Name = "Test User",
                Role = "Admin",
                Location = "Test",
                Email = Guid.NewGuid() + "@test.com", // avoid unique conflict
                PasswordHash = "hash"
            };

            int uid1 = await _userRepository.CreateAsync(user1);
            int uid2 = await _userRepository.CreateAsync(user2);

            var device1 = new Device
            {
                Name = "UserDevice",
                Manufacturer = "Test",
                Type = "phone",
                OS = "OS",
                AssignedUserId = uid1
            };

            var device2 = new Device
            {
                Name = "OtherDevice",
                Manufacturer = "Test",
                Type = "phone",
                OS = "OS",
                AssignedUserId = uid2
            };

            var id1 = await _deviceRepository.CreateAsync(device1);
            var id2 = await _deviceRepository.CreateAsync(device2);

            // Act
            var result = await _deviceRepository.GetByUserIdAsync(uid1);

            // Assert
            Assert.Contains(result, d => d.Id == id1);
            Assert.DoesNotContain(result, d => d.Id == id2);

            // Cleanup
            await _deviceRepository.DeleteAsync(id1);
            await _deviceRepository.DeleteAsync(id2);

            await _userRepository.DeleteAsync(uid1);
            await _userRepository.DeleteAsync(uid2);
        }
    }
}