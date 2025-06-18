using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace LLMClientTool
{
  class Program
  {
    private const string MODEL = "llama3.2";
    //private const string MODEL = "qwen2.5:0.5b";
    private const string CHAT_URL = "http://localhost:11434/api/chat";
    static readonly HttpClient client = new();

    private static readonly List<OllamaMessage> ChatHistory = [];
    private static readonly JsonSerializerSettings serializerSettings = new()
    {
      ContractResolver = new CamelCasePropertyNamesContractResolver(),
      Converters =
      [
        new StringEnumConverter()
      ],
    };

    static async Task Main(string[] args)
    {
      while (true)
      {
        Console.WriteLine("User:");
        string? userPrompt = Console.ReadLine();

        if (string.IsNullOrEmpty(userPrompt))
        {
          Console.WriteLine("No input provided. Exiting...");
          return;
        }

        var userMessage = CreateUserMessage(userPrompt);
        ChatHistory.Add(userMessage);
        try
        {
          await SendRequestAsync(CreatePayload(ChatHistory));
        }
        catch (Exception ex)
        {
          Console.WriteLine($"An error occurred: {ex.Message}");
        }
      }
    }

    static OllamaMessage CreateUserMessage(string prompt)
    {
      return new OllamaMessage
      {
        Role = OllamaRole.User,
        Content = prompt,
      };
    }

    static OllamaPayload CreatePayload(List<OllamaMessage> messages)
    {
      return new OllamaPayload
      {
        Model = MODEL,
        Messages = messages,
        Tools = ToolDefinitions.GetOllamaToolDefinitions(),
      };
    }

    static async Task SendRequestAsync(object payload)
    {
      var json = JsonConvert.SerializeObject(payload, serializerSettings);

      ConsoleExt.WriteLineYellow("json payload:");
      ConsoleExt.WriteLineYellow(json);

      var content = new StringContent(json, Encoding.UTF8, "application/json");

      HttpResponseMessage response = await client.PostAsync(CHAT_URL, content);

      if (response.IsSuccessStatusCode)
      {
        await StreamResponseAsync(response.Content);
      }
      else
      {
        Console.WriteLine($"Request failed with status code {response.StatusCode} and message {await response.Content.ReadAsStringAsync()}");
      }
    }

    static async Task StreamResponseAsync(HttpContent content)
    {
      using var reader = new System.IO.StreamReader(await content.ReadAsStreamAsync());
      string? line;
      OllamaMessage fullMessage = new()
      {
        Content = string.Empty,
        Role = OllamaRole.Assistant,
      };

      while ((line = await reader.ReadLineAsync()) != null)
      {
        var parsedResponse = JsonConvert.DeserializeObject<OllamaResponse>(line) ?? throw new FormatException($"Failed to parse OllamaResponse: {line}");

        Console.Write(parsedResponse.Message.Content);

        if (!parsedResponse.Done)
        {
          fullMessage.Content += parsedResponse.Message.Content;
        }

        foreach (var toolCall in parsedResponse.Message.ToolCalls)
        {
          if (toolCall.Function is null) continue;

          string toolResult = toolCall.Function.Name switch
          {
            nameof(Functions.ListDirectory) => Functions.ListDirectory(
              toolCall.Function.Arguments[ToolDefinitions.RelativeDirectoryPath],
              bool.Parse(toolCall.Function.Arguments[ToolDefinitions.Recurse])
            ),
            nameof(Functions.GetFileContents) => Functions.GetFileContents(
              toolCall.Function.Arguments[ToolDefinitions.RelativeFilePath]
            ),
            _ => throw new InvalidOperationException($"Call made to unrecognized function: {toolCall.Function.Name}"),
          };

          ChatHistory.Add(new OllamaMessage
          {
            Role = OllamaRole.Tool,
            Content = toolResult,
          });
        }

        ConsoleExt.WriteLineYellow(line + Environment.NewLine);
      }

      ChatHistory.Add(fullMessage);

      foreach (var omsg in ChatHistory)
      {
        Console.WriteLine($"{omsg.Role}: {omsg.Content}");
      }
    }
  }

  public static class ConsoleExt
  {
    public static void WriteLineYellow(string message)
    {
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine(message);
      Console.ForegroundColor = ConsoleColor.White;
    }
  }
}