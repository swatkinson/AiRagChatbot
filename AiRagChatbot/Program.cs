using AiRagChatbot.OllamaClient;

// Create the request payload
var request = new ChatRequest
{
    //TODO: Move modelfile information into this as "options"
    //TODO: Set keepalive to 0, and unload manually at the end of the program
    Model = "llama3.2:3b",
    Messages =
    [
        // Parameters for the AI
        new Message { Content = "You are a helpful concierge, ready to professionally assist the user. Provide responses as briefly as possible", Role = "system" },
        //First message
        new Message { Content = "My name is Chatley, your AI Concierge. How can I help you out today?", Role = "assistant" }
    ], 
    Stream = false,
};

OllamaClient.PrintMessage(request.Messages.First(m => m.Role == "assistant"));
        
while (true)
{
    // Ask user for AI prompt
    Console.Write("> ");
    var userInput = Console.ReadLine() ?? "";
    
    // Add user prompt to messages
    request.Messages.Add(new Message { Content = userInput, Role = "user" });
            
    // Call AI with updated request
    var response = await OllamaClient.CallOllamaModel(request);
    
    // Add AI response to messages
    request.Messages.Add(response!.Message);
    
    OllamaClient.PrintMessage(response.Message);
}