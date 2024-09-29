using System.Text.Json.Serialization;

namespace AiRagChatbot.OllamaClient;

public class Message
{
    [JsonPropertyName("role")]
    public required string Role { get; set; }
    
    [JsonPropertyName("content")]
    public required string Content { get; set; }
}
