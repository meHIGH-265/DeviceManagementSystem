namespace DeviceManagementSystem.Domain
{
    public class Device
    {
        public int? Id { get; set; }
        public string Name { get; set; } = null!;

        public string Manufacturer { get; set; } = null!;

        public string Type { get; set; } = null!;

        public string OS { get; set; } = null!;

        public string? OSVersion { get; set; }

        public string? Processor { get; set; }

        public string? RAM { get; set; }

        public string? Description { get; set; }

        public int? AssignedUserId { get; set; }

        public User? AssignedUser { get; set; }
    }
}
