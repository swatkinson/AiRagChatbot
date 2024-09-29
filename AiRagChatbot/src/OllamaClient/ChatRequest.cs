using System.Text.Json.Serialization;

namespace AiRagChatbot.OllamaClient;

public class ChatRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; }
    
    [JsonPropertyName("messages")]
    public List<Message> Messages { get; set; }
    
    [JsonPropertyName("stream")]
    public bool Stream { get; set; }
    
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; }
    
}

