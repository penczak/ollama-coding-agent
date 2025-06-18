namespace LLMClientTool
{
  public static class ToolDefinitions
  {
    public static List<OllamaToolDefinition> GetOllamaToolDefinitions()
    {
      return new List<OllamaToolDefinition>
      {
        ListDirectory,
        GetFileContents,
      };
      /*new[]
        {
          new {
            type = "function",
            function = new {
              name = "get_current_weather",
              description = "Get the current weather for a location",
              parameters = new {
                type = "object",
                properties = new Dictionary<string, object>
                {
                  {
                    "location",
                    new
                    {
                      type = "string",
                      description = "The location to get the weather for, e.g. San Francisco, CA"
                    }
                  },
                  {
                    "format",
                    new
                    {
                      type = "string",
                      description = "The format to return the weather in, e.g. 'celsius' or 'fahrenheit'",
                      _enum = new [] { "celsius", "fahrenheit" }
                    }
                  }
                },
                required = new [] { "location", "format" }
              }
            }
          },*/

      /*
        new {
          type = "function",
          function = new {
            name = nameof(Functions.ListDirectory),
            description = "Get the file system entries of a directory and all subdirectories",
            parameters = new {
              type = "object",
              properties = new Dictionary<string, object>
              {
                { "relativeDirectoryPath", new {
                  type = "string",
                  description = "The directory to enumerate, e.g. /src/controllers, relative to the root of the project."
                  }
                }
              },
              required = new [] { "relativeDirectoryPath" }
            }
          }
        },*/
    }

    private const string _string = "string";
    private const string _bool = "bool";

    public const string RelativeDirectoryPath = "relativeDirectoryPath";
    public const string Recurse = "recurse";

    public static OllamaToolDefinition ListDirectory { get => new OllamaToolDefinition
      {
        Function = new OllamaFunction
        {
          Name = nameof(Functions.ListDirectory),
          Description = "Get the file system entries of a directory and all subdirectories",
          Parameters = new OllamaFunctionParametersDefinition
          {
            Required = [RelativeDirectoryPath, Recurse],
            Properties = new Dictionary<string, OllamaParameterPropertyDefinition>
              {
                {
                  RelativeDirectoryPath,
                  new OllamaParameterPropertyDefinition
                  {
                    Type = _string,
                    Description = "The directory to enumerate, e.g. /src/controllers, relative to the root of the project.",
                  }
                },
                {
                  Recurse,
                  new OllamaParameterPropertyDefinition
                  {
                    Type = _bool,
                    Description = "Whether to recurse the directory and output files of all subdirectories or not.",
                  }
                },
              }
          }
        }
      };
    }

    public const string RelativeFilePath = "relativeFilePath";

    public static OllamaToolDefinition GetFileContents { get => new OllamaToolDefinition
      {
        Function = new OllamaFunction
        {
          Name = nameof(Functions.GetFileContents),
          Description = "Get the text content of a file",
          Parameters = new OllamaFunctionParametersDefinition
          {
            Required = [RelativeFilePath],
            Properties = new Dictionary<string, OllamaParameterPropertyDefinition>
              {
                {
                  RelativeFilePath,
                  new OllamaParameterPropertyDefinition
                  {
                    Type = _string,
                    Description = "The file to read relative to the root of the project.",
                  }
                },
              }
          }
        }
      };
    }
  }
}
