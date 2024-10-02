using System.Text;
using Microsoft.KernelMemory.AI;
using Microsoft.KernelMemory.AI.OpenAI;

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

    public static void PrintMessage(string message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(string.IsNullOrEmpty(message) ? "" : message);
        Console.ForegroundColor = ConsoleColor.White;
    }
    
    internal class OllamaTextGeneration : ITextGenerator
    {
        chatHelper chatHelper;
        private string _modelName;

        public OllamaTextGeneration(HttpClient ollamaClient, string modelName)
        {
            chatHelper = new chatHelper(ollamaClient);
            _modelName = modelName;
        }

        public int MaxTokenTotal => 4096;

        public int CountTokens(string text)
        {
            return DefaultGPTTokenizer.StaticCountTokens(text);
        }

        public async IAsyncEnumerable<string> GenerateTextAsync(string prompt, TextGenerationOptions options, CancellationToken cancellationToken = default)
        {
            await foreach (var response in chatHelper.ChatCompletion(prompt, _modelName, cancellationToken))
            {
                yield return response;
            }
        }

        public IReadOnlyList<string> GetTokens(string text)
        {
            throw new NotImplementedException();
        }
    }
}