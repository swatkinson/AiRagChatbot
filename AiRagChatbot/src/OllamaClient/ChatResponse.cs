using System.Text.Json.Serialization;

namespace AiRagChatbot.OllamaClient;

public class ChatResponse
{
    [JsonPropertyName("model")]
    public string? Model { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("response")]
    public string? Response { get; set; }

    [JsonPropertyName("done")]
    public bool Done { get; set; }
    
    [JsonPropertyName("done_reason")]
    public string? DoneReason { get; set; }
    
    [JsonPropertyName("context")]
    public long[]? Context { get; set; }

    [JsonPropertyName("total_duration")]
    public double TotalDuration { get; set; }

    public double TotalDurationSeconds => TotalDuration / 100000000;

    [JsonPropertyName("load_duration")]
    public double LoadDuration { get; set; }
    
    public double LoadDurationSeconds => LoadDuration / 100000000;

    [JsonPropertyName("prompt_eval_count")]
    public int PromptEvalCount { get; set; }

    [JsonPropertyName("prompt_eval_duration")]
    public double PromptEvalDuration { get; set; }
    
    public double PromptEvalDurationSeconds => PromptEvalDuration / 100000000;

    [JsonPropertyName("eval_count")]
    public int EvalCount { get; set; }

    [JsonPropertyName("eval_duration")]
    public double EvalDuration { get; set; }
    
    public double EvalDurationSeconds => EvalDuration / 100000000;
}