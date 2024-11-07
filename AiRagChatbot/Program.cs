using AiRagChatbot;
using AiRagChatbot.OllamaClient;

var ollamaEndpoint = "http://127.0.0.1:11434";
var ollamaClient = new HttpClient
{
    BaseAddress = new Uri(ollamaEndpoint)
};
var ollamaModelName = "llama3.2:3b";

var request = new ChatRequest
{
    //TODO: Set keepalive to 0, and unload manually at the end of the program
    Model = ollamaModelName,
    Messages =
    [
        // Parameters for the AI
        new Message 
        { 
            Content = "You are a helpful concierge, ready to professionally assist the user. " +
                      "Provide responses as briefly as possible. "
                      // "Each user message will have a Context section, answer the question they ask using the context provided" +
                      // "If the user asks about something that isn't related to the context provided, you are not allowed to answer." +
                      // "If someone asks something unrelated to the context, politely decline, and ask if they have any questions related to the context." +
                      // "If you do not know something as a fact, say that you are not sure. Do NOT make up information."
                      , 
            Role = "system"
        },
        //First message
        new Message { Content = "My name is Chatley, your AI Concierge. How can I help you out today?", Role = "assistant" }
    ], 
    Stream = false,
    Options = new Options { Temperature = 0.1 }
};

Console.Write("Loading context ...    ");

var kernelMemory = KernelMemory.GetMemoryKernel(ollamaClient, ollamaModelName);

await kernelMemory.ImportDocumentAsync("C:\\Users\\Spenc\\src\\AiRagChatbot\\AiRagChatbot\\src\\TestInformation\\Resqwest info.txt");

Console.WriteLine("Done!");

OllamaClient.PrintMessage(request.Messages.First(m => m.Role == "assistant").Content);

while (true)
{
    string llamaQuestion;
    
    // Ask user for AI prompt
    Console.Write("> ");
    var userQuestion = Console.ReadLine() ?? "";
    
    var ragAnswer = await kernelMemory.AskAsync(userQuestion);

    Console.WriteLine($"INFO FOUND: {!ragAnswer.NoResult}\n" +
                      $"CONTEXT: {ragAnswer}");

    if (ragAnswer.NoResult)
    {
        llamaQuestion = $"SYSTEM: There is no information available to answer any questions, do not answer any questions. " +
                        $"Instead, respond with \"I'm sorry, I do not have the relevant information to answer that question accurately. Is there anything else I can assist you with?\"." +
                        $"If the user prompt is not a question, you can respond to it" +
                        $"USER PROMPT: \"{userQuestion}\"";
    }
    else
    {
        llamaQuestion = $"Respond to the USER PROMPT using the provided CONTEXT." +
                        $"CONTEXT: \"{ragAnswer.Result}\"\n " +
                        $"USER PROMPT: \"{userQuestion}\"";
    }
    
    // Add user prompt to messages
    request.Messages.Add(new Message { Content = llamaQuestion, Role = "user" });
            
    // Call AI with updated request
    var response = await OllamaClient.CallOllamaModel(request);
    
    // Add AI response to messages
    request.Messages.Add(response!.Message);
    
    OllamaClient.PrintMessage(response.Message.Content);

    //If user says "bye", end the chat.
    if (userQuestion.Contains("bye")) break;
}