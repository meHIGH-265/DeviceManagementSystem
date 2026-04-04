namespace DeviceManagementSystem.Domain
{
    public class User : Entity
    {
        public string Name { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string Location { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;
    }
}
