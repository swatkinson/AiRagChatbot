using System.Text;
using Microsoft.KernelMemory;
using Microsoft.KernelMemory.AI;
using Microsoft.KernelMemory.AI.OpenAI;

namespace AiRagChatbot.OllamaClient;

internal class OllamaTextEmbedding : ITextEmbeddingGenerator
{
    private const string Url = "http://localhost:11434/api/embed";

    private const string Payload = @"{
                ""model"": ""mxbai-embed-large:335m-v1-fp16"",
                ""input"": ""{input}""
            }";

    private static readonly HttpClient HttpClient = new();
    
    public int MaxTokens => 4096;

    public int CountTokens(string text)
    {
        return DefaultGPTTokenizer.StaticCountTokens(text);
    }

    public async Task<Embedding> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default)
    {
        // Set up the request content
        StringContent content = new StringContent(Payload, Encoding.UTF8, "application/json");

        // Send the POST request
        HttpResponseMessage response = await HttpClient.PostAsync(Url, content, cancellationToken);

        // Check if the response is successful
        if (response.IsSuccessStatusCode)
        {
            // Read the response content
            string responseData = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON into the ApiResponse object
            EmbeddingResponse apiResponse = System.Text.Json.JsonSerializer.Deserialize<EmbeddingResponse>(responseData)!;

            var embedding = new Embedding
            {
                Data = new ReadOnlyMemory<float>(Array.ConvertAll(apiResponse.Embeddings[0].ToArray(), x => (float)x))
            };

            return embedding;
        }

        Console.WriteLine("Error: " + response.StatusCode);
        return null;
    }

    public IReadOnlyList<string> GetTokens(string text)
    {
        throw new NotImplementedException();
    }
}