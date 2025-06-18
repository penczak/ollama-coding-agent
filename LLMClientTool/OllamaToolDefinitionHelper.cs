namespace LLMClientTool
{
  public static class OllamaToolDefinitionHelper
  {
    public static List<OllamaToolDefinition> GetOllamaToolDefinitions()
    {
      return new List<OllamaToolDefinition>
      {
        new OllamaToolDefinition
        {
          Function = new OllamaFunction
          {
            Name = nameof(Functions.ListDirectory),
            Description = "Get the file system entries of a directory and all subdirectories",
            Parameters = new OllamaFunctionParametersDefinition
            {
              Required = ["relativeDirectoryPath", "recurse"],
              Properties = new Dictionary<string, OllamaParameterPropertyDefinition>
              {
                {
                  "relativeDirectoryPath",
                  new OllamaParameterPropertyDefinition
                  {
                    Type = "string",
                    Description = "The directory to enumerate, e.g. /src/controllers, relative to the root of the project.",
                  }
                },
                {
                  "recurse",
                  new OllamaParameterPropertyDefinition
                  {
                    Type = "bool",
                    Description = "Whether to recurse the directory and output files of all subdirectories or not.",
                  }
                },
              }
            }
          }
        }
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
  }
}
