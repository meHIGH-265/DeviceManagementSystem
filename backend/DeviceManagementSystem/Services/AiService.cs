using DeviceManagementSystem.Domain;
using OpenAI.Chat;

namespace DeviceManagementSystem.Services
{
    public class AiService
    {
        private readonly IConfiguration _config;

        public AiService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> GenerateDeviceDescription(Device device)
        {
            var apiKey = _config["OpenAI:ApiKey"];

            var client = new ChatClient(model: "gpt-5.4-nano", apiKey: apiKey);

            var prompt = $@"
Create a human-readable, concise, and informative description of a device based on its technical specifications.

Example:
Input: Name – iPhone 17 Pro, Manufacturer – Apple, OS – iOS, Type – phone, RAM – 12GB, Processor – A19 Pro
Output: 'A high-performance Apple smartphone running iOS, suitable for daily business use.'

Focus on generating clear, relevant, and user-friendly descriptions that enhance the device information.

Here are the details of the device that I need you to generate a description for:
Name: {device.Name}
Manufacturer: {device.Manufacturer}
Type: {device.Type}
OS: {device.OS}
OS Version: {device.OSVersion}
Processor: {device.Processor}
RAM: {device.RAM}

Do NOT begin the descriotion with 'The {device.Name} is ...'!
";

            var response = await client.CompleteChatAsync(prompt);

            return response.Value.Content[0].Text;
        }
    }
}
