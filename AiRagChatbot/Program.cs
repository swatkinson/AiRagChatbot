using AiRagChatbot.OllamaClient;

namespace AiRagChatbot;

internal abstract class Program
{
    private static async Task Main(string[] args)
    {
        var ollamaClient = new OllamaClient.OllamaClient();
        
        // Create the request payload
        var request = new ChatRequest
        {
            Model = "assistant",
            Messages = [],
            Stream = false,
            Prompt = "Briefly, why is the sky blue?"
        };

        var response = await OllamaClient.OllamaClient.CallOllamaModel(request);

        Console.WriteLine(response);
    }
    
}