using System.Runtime.InteropServices.Marshalling;

namespace DeviceManagementSystem.Domain
{
    public class User
    {
        public int? Id { get; set; }
        public string Name { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string Location { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? PasswordHash { get; set; }
    }
}
