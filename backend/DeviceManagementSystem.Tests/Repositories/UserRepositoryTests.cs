using DeviceManagementSystem.Domain;
using DeviceManagementSystem.Repository;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace DeviceManagementSystem.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private readonly IUserRepository _repository;

        public UserRepositoryTests()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Test.json")
                .Build();

            _repository = new UserRepository(config);
        }

        [Fact]
        public async Task Create_Get_Delete_User_Should_Work()
        {
            // Arrange
            var user = new User
            {
                Name = "Test User",
                Role = "Admin",
                Location = "Test",
                Email = Guid.NewGuid() + "@test.com", // avoid unique conflict
                PasswordHash = "hash"
            };

            // Act
            var id = await _repository.CreateAsync(user);
            var fetched = await _repository.GetByIdAsync(id);

            // Assert
            Assert.NotNull(fetched);
            Assert.Equal("Test User", fetched!.Name);

            // Cleanup
            await _repository.DeleteAsync(id);
        }

        [Fact]
        public async Task Update_User_Should_Work()
        {
            // Arrange
            var user = new User
            {
                Name = "Initial",
                Role = "User",
                Location = "Test",
                Email = Guid.NewGuid() + "@test.com",
                PasswordHash = "hash"
            };

            var id = await _repository.CreateAsync(user);

            // Act
            user.Id = id;
            user.Name = "Updated";

            var updated = await _repository.UpdateAsync(user);
            var fetched = await _repository.GetByIdAsync(id);

            // Assert
            Assert.True(updated);
            Assert.Equal("Updated", fetched!.Name);

            // Cleanup
            await _repository.DeleteAsync(id);
        }
    }
}