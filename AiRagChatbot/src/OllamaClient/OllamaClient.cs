using System.Text;

namespace AiRagChatbot.OllamaClient;

public sealed class OllamaClient
{
    private static readonly HttpClient Client = new HttpClient();

    public static async Task<string?> CallOllamaModel(ChatRequest request)
    {
        // Ollama API endpoint
        const string url = "http://localhost:11434/api/generate";
        
        // Serialize the request body to JSON
        var json = System.Text.Json.JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            // Send the POST request
            var response = await Client.PostAsync(url, content);

            // Ensure the response is successful
            response.EnsureSuccessStatusCode();

            // Read the response content
            var resultJson = await response.Content.ReadAsStringAsync();
            
            var result = System.Text.Json.JsonSerializer.Deserialize<ChatResponse>(resultJson);
            
            return result?.Response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error calling Ollama model: {ex.Message}");
            return null;
        }
    }
}