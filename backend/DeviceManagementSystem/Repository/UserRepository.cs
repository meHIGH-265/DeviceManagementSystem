using DeviceManagementSystem.Domain;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviceManagementSystem.Repository
{
    public interface IUserRepository : IRepository<User, int>
    {
        Task<User?> GetByEmailAsync(string email, string? passwordHash);
    }

    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            }
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = new List<User>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM Users";

            using var command = new SqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add(new User
                {
                    Id = (int)reader["Id"],
                    Name = reader["Name"].ToString()!,
                    Role = reader["Role"].ToString()!,
                    Location = reader["Location"].ToString()!,
                    Email = reader["Email"].ToString()!,
                    PasswordHash = null// reader["PasswordHash"].ToString()!
                });
            }

            return users;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM Users WHERE Id = @Id";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = (int)reader["Id"],
                    Name = reader["Name"].ToString()!,
                    Role = reader["Role"].ToString()!,
                    Location = reader["Location"].ToString()!,
                    Email = reader["Email"].ToString()!,
                    PasswordHash = null// reader["PasswordHash"].ToString()!
                };
            }

            return null;
        }

        public async Task<int> CreateAsync(User entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                INSERT INTO Users (Name, Role, Location, Email, PasswordHash)
                OUTPUT INSERTED.Id
                VALUES (@Name, @Role, @Location, @Email, @PasswordHash)";

            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Name", entity.Name);
            command.Parameters.AddWithValue("@Role", entity.Role);
            command.Parameters.AddWithValue("@Location", entity.Location);
            command.Parameters.AddWithValue("@Email", entity.Email);
            command.Parameters.AddWithValue("@PasswordHash", entity.PasswordHash);

            var result = await command.ExecuteScalarAsync();
            return (int)result!;
        }

        public async Task<bool> UpdateAsync(User entity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                UPDATE Users
                SET Name = @Name,
                    Role = @Role,
                    Location = @Location,
                    Email = @Email,
                    PasswordHash = @PasswordHash
                WHERE Id = @Id";

            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Id", entity.Id);
            command.Parameters.AddWithValue("@Name", entity.Name);
            command.Parameters.AddWithValue("@Role", entity.Role);
            command.Parameters.AddWithValue("@Location", entity.Location);
            command.Parameters.AddWithValue("@Email", entity.Email);
            command.Parameters.AddWithValue("@PasswordHash", entity.PasswordHash);

            var rows = await command.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var query = "DELETE FROM Users WHERE Id = @Id";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            var rows = await command.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<User?> GetByEmailAsync(string email, string? passwordHash)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            string passwordCheckClause = passwordHash != null ? " AND PasswordHash = @PasswordHash" : string.Empty;
            var query = $"SELECT * FROM Users WHERE Email = @Email{passwordCheckClause}";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", email);
            if (passwordHash != null )
            {
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);
            }

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = (int)reader["Id"],
                    Name = reader["Name"].ToString()!,
                    Role = reader["Role"].ToString()!,
                    Location = reader["Location"].ToString()!,
                    Email = reader["Email"].ToString()!,
                    PasswordHash = null// reader["PasswordHash"].ToString()!
                };
            }

            return null;
        }
    }
}
