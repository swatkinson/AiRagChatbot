using System.Text;

namespace AiRagChatbot.OllamaClient;

public static class OllamaClient
{
    private static readonly HttpClient Client = new HttpClient();

    public static async Task<ChatResponse?> CallOllamaModel(ChatRequest request)
    {
        // Ollama API endpoint
        const string url = "http://localhost:11434/api/chat";
        
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
            
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error calling Ollama model: {ex.Message}");
            return null;
        }
    }

    public static void PrintMessage(Message message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(string.IsNullOrEmpty(message.Content) ? "" : message.Content);
        Console.ForegroundColor = ConsoleColor.White;
    }
}