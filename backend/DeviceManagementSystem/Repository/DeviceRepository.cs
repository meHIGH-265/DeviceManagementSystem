using DeviceManagementSystem.Domain;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviceManagementSystem.Repository
{
    public interface IDeviceRepository : IRepository<Device, int>
    {
        Task<IEnumerable<Device>> GetByUserIdAsync(int userId);
    }

    public class DeviceRepository : IDeviceRepository
    {
        private readonly string _connectionString;

        public DeviceRepository(IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            }
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Device>> GetAllAsync()
        {
            var devices = new List<Device>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM Devices";

            using var command = new SqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                devices.Add(new Device
                {
                    Id = (int)reader["Id"],
                    Name = reader["Name"].ToString()!,
                    Manufacturer = reader["Manufacturer"].ToString()!,
                    Type = reader["Type"].ToString()!,
                    OS = reader["OS"].ToString()!,
                    OSVersion = reader["OSVersion"]?.ToString(),
                    Processor = reader["Processor"]?.ToString(),
                    RAM = reader["RAM"]?.ToString(),
                    Description = reader["Description"]?.ToString(),
                    AssignedUserId = reader["AssignedUserId"] as int?
                });
            }

            return devices;
        }

        public async Task<Device?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM Devices WHERE Id = @Id";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Device
                {
                    Id = (int)reader["Id"],
                    Name = reader["Name"].ToString()!,
                    Manufacturer = reader["Manufacturer"].ToString()!,
                    Type = reader["Type"].ToString()!,
                    OS = reader["OS"].ToString()!,
                    OSVersion = reader["OSVersion"]?.ToString(),
                    Processor = reader["Processor"]?.ToString(),
                    RAM = reader["RAM"]?.ToString(),
                    Description = reader["Description"]?.ToString(),
                    AssignedUserId = reader["AssignedUserId"] as int?
                };
            }

            return null;
        }

        public async Task<int> CreateAsync(Device entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                INSERT INTO Devices 
                (Name, Manufacturer, Type, OS, OSVersion, Processor, RAM, Description, AssignedUserId)
                OUTPUT INSERTED.Id
                VALUES 
                (@Name, @Manufacturer, @Type, @OS, @OSVersion, @Processor, @RAM, @Description, @AssignedUserId)";

            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Name", entity.Name);
            command.Parameters.AddWithValue("@Manufacturer", entity.Manufacturer);
            command.Parameters.AddWithValue("@Type", entity.Type);
            command.Parameters.AddWithValue("@OS", entity.OS);
            command.Parameters.AddWithValue("@OSVersion", (object?)entity.OSVersion ?? DBNull.Value);
            command.Parameters.AddWithValue("@Processor", (object?)entity.Processor ?? DBNull.Value);
            command.Parameters.AddWithValue("@RAM", (object?)entity.RAM ?? DBNull.Value);
            command.Parameters.AddWithValue("@Description", (object?)entity.Description ?? DBNull.Value);
            command.Parameters.AddWithValue("@AssignedUserId", (object?)entity.AssignedUserId ?? DBNull.Value);

            var result = await command.ExecuteScalarAsync();
            return (int)result!;
        }

        public async Task<bool> UpdateAsync(Device entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                UPDATE Devices
                SET Name = @Name,
                    Manufacturer = @Manufacturer,
                    Type = @Type,
                    OS = @OS,
                    OSVersion = @OSVersion,
                    Processor = @Processor,
                    RAM = @RAM,
                    Description = @Description,
                    AssignedUserId = @AssignedUserId
                WHERE Id = @Id";

            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@Name", entity.Name);
            command.Parameters.AddWithValue("@Manufacturer", entity.Manufacturer);
            command.Parameters.AddWithValue("@Type", entity.Type);
            command.Parameters.AddWithValue("@OS", entity.OS);
            command.Parameters.AddWithValue("@OSVersion", (object?)entity.OSVersion ?? DBNull.Value);
            command.Parameters.AddWithValue("@Processor", (object?)entity.Processor ?? DBNull.Value);
            command.Parameters.AddWithValue("@RAM", (object?)entity.RAM ?? DBNull.Value);
            command.Parameters.AddWithValue("@Description", (object?)entity.Description ?? DBNull.Value);
            command.Parameters.AddWithValue("@AssignedUserId", (object?)entity.AssignedUserId ?? DBNull.Value);

            var rows = await command.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "DELETE FROM Devices WHERE Id = @Id";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            var rows = await command.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<IEnumerable<Device>> GetByUserIdAsync(int userId)
        {
            var devices = new List<Device>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM Devices WHERE AssignedUserId = @UserId";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserId", userId);

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                devices.Add(new Device
                {
                    Id = (int)reader["Id"],
                    Name = reader["Name"].ToString()!,
                    Manufacturer = reader["Manufacturer"].ToString()!,
                    Type = reader["Type"].ToString()!,
                    OS = reader["OS"].ToString()!,
                    OSVersion = reader["OSVersion"]?.ToString(),
                    Processor = reader["Processor"]?.ToString(),
                    RAM = reader["RAM"]?.ToString(),
                    Description = reader["Description"]?.ToString(),
                    AssignedUserId = reader["AssignedUserId"] as int?
                });
            }

            return devices;
        }
    }
}
