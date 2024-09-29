using System.Text.Json.Serialization;

namespace AiRagChatbot.OllamaClient;

public sealed class ChatRequest
{
    [JsonPropertyName("model")]
    public required string Model { get; set; }
    
    [JsonPropertyName("messages")]
    public required List<Message> Messages { get; set; }
    
    [JsonPropertyName("stream")]
    public bool Stream { get; set; }
}

