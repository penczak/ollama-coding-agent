using LLMClientTool;
using System.IO;

namespace LLMClientToolTests
{
  public class ReplaceFileLines_Tests
  {
    [Fact]
    public void CanReplaceFileLines()
    {
      var relPath = $"/test-{Guid.NewGuid()}.txt";

      var root = Environment.CurrentDirectory;
      var path = Path.Join(root, relPath);
      try
      {
        File.WriteAllText(path, @"Ollama now supports tool calling with popular models such as Llama 3.1. This enables a model to answer a given prompt using tool(s) it knows about, making it possible for models to perform more complex tasks or interact with the outside world.
  Example tools include:
  Functions and APIs
  Web browsing
  Code interpreter
  much more!

  Tool calling: 
  To enable tool calling, provide a list of available tools via the tools field in Ollama’s API.
  import ollama

  response = ollama.chat(
      model='llama3.1',
      messages=[{'role': 'user', 'content':
          'What is the weather in Toronto?'}],

		  # provide a weather checking tool to the model
      tools=[{
        'type': 'function',
        'function': {
          'name': 'get_current_weather',
          'description': 'Get the current weather for a city',
          'parameters': {
            'type': 'object',
            'properties': {
              'city': {
                'type': 'string',
                'description': 'The name of the city',
              },
            },
            'required': ['city'],
          },
        },
      },
    ],
  )

  print(response['message']['tool_calls'])
  ");

        var newContent = @"  My other feature
  Some cool things!";
        var res = Functions.ReplaceFileLines(relPath, newContent, 3, 4);

        Assert.DoesNotContain("Functions and APIs\r\nWeb browsing\r\nCode interpreter\r\nmuch more!", res);
        Assert.DoesNotContain("Functions and APIs", res);
        Assert.DoesNotContain("Web browsing", res);
        Assert.DoesNotContain("Code interpreter", res);
        Assert.DoesNotContain("much more!", res);
        Assert.Contains("My other feature", res);
        Assert.Contains("Some cool things!", res);

        var writtenContent = File.ReadAllText(path);

        Assert.DoesNotContain("Functions and APIs\r\nWeb browsing\r\nCode interpreter\r\nmuch more!", writtenContent);
        Assert.DoesNotContain("Functions and APIs", writtenContent);
        Assert.DoesNotContain("Web browsing", writtenContent);
        Assert.DoesNotContain("Code interpreter", writtenContent);
        Assert.DoesNotContain("much more!", writtenContent);
        Assert.Contains("My other feature", writtenContent);
        Assert.Contains("Some cool things!", writtenContent);

        Assert.Contains(@"  Example tools include:
  My other feature
  Some cool things!

  Tool calling: ", writtenContent);
      }
      finally
      {
        File.Delete(path);
      }
    }

    [Fact]
    public void CanReplaceFileLines_0LinesWillInsert()
    {
      var relPath = $"/test-{Guid.NewGuid()}.txt";

      var root = Environment.CurrentDirectory;
      var path = Path.Join(root, relPath);
      try
      {
        File.WriteAllText(path, @"Ollama now supports tools
Example tools include:
Functions and APIs
Web browsing
Code interpreter
much more!
Tool calling
");

        var newContent = @"My other feature
Some cool things!";
        var res = Functions.ReplaceFileLines(relPath, newContent, 3, 0);

        var writtenContent = File.ReadAllText(path);

        Assert.Contains(@"Ollama now supports tools
Example tools include:
Functions and APIs
Web browsing
My other feature
Some cool things!
Code interpreter
much more!
Tool calling", writtenContent);
      }
      finally
      {
        File.Delete(path);
      }
    }
  }
}