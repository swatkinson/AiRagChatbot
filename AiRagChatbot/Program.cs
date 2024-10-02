using AiRagChatbot;
using AiRagChatbot.OllamaClient;



//Console.WriteLine(websites.Result.First().Value);
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

OllamaClient.PrintMessage(request.Messages.First(m => m.Role == "assistant").Content);

var websites = WebsiteCrawler.WebsiteCrawler.Crawl("https://www.resqwest.com", 1).Result;

// Load websites into memory
var kernelMemory = KernelMemory.GetMemoryKernel(ollamaClient, ollamaModelName);

foreach (var website in websites)
{
    await kernelMemory.ImportWebPageAsync(website.Key);
}



while (true)
{
    Console.Write("> ");
    var question = Console.ReadLine();

    var answer = await kernelMemory.AskAsync(question);

    OllamaClient.PrintMessage(answer.Result);
}


/*
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

    //If user says "bye", end the chat.
    if (userInput.Contains("bye")) break;
}
*/