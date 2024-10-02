using System.Text.Json.Serialization;

namespace AiRagChatbot.OllamaClient;

public class EmbeddingResponse
{
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("embeddings")]
    public List<List<float>> Embeddings { get; set; }

    [JsonPropertyName("total_duration")]
    public long TotalDuration { get; set; }

    [JsonPropertyName("load_duration")]
    public long LoadDuration { get; set; }

    [JsonPropertyName("prompt_eval_count")]
    public int PromptEvalCount { get; set; }
}