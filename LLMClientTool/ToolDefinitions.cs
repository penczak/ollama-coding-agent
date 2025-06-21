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
    }

    private const string _string = "string";
    private const string _bool = "bool";

    public const string RelativeDirectoryPath = "relativeDirectoryPath";
    public const string Recurse = "recurse";

    public static OllamaToolDefinition ListDirectory
    { get => new OllamaToolDefinition
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

    public static OllamaToolDefinition GetFileContents
    { get => new OllamaToolDefinition
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

    public static OllamaToolDefinition ReplaceFileLines
    { get => new OllamaToolDefinition
      {
        Function = new OllamaFunction
        {
          Name = nameof(Functions.ReplaceFileLines),
          Description = "Replace the lines of a file with new content by specifying which line number to replace and how many lines of the original to remove",
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
