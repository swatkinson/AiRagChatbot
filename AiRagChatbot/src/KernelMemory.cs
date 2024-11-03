using AiRagChatbot.OllamaClient;
using Microsoft.KernelMemory;
using Microsoft.KernelMemory.FileSystem.DevTools;
using Microsoft.KernelMemory.MemoryStorage.DevTools;

namespace AiRagChatbot;

internal static class KernelMemory
{
    internal static MemoryServerless GetMemoryKernel(HttpClient ollamaClient, string modelName)
    {
        var memoryBuilder = new KernelMemoryBuilder();

        memoryBuilder.WithCustomPromptProvider(new OllamaPromptProvider());
        memoryBuilder.WithCustomEmbeddingGenerator(new OllamaTextEmbedding())
            .WithCustomTextGenerator(new OllamaClient.OllamaClient.OllamaTextGeneration(ollamaClient, modelName))
            .WithSimpleVectorDb(new SimpleVectorDbConfig { Directory = "VectorDirectory", StorageType = FileSystemTypes.Disk })
            .Build<MemoryServerless>();

        var memory = memoryBuilder.Build<MemoryServerless>();

        return memory;
    }
}