using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LLMClientTool
{
  public class OllamaResponse
  {
    public required string Model { get; set; }
    public required OllamaMessage Message { get; set; }
    public bool Done { get; set; }
  }

  [JsonConverter(typeof(StringEnumConverter))]
  public enum OllamaRole
  {
    System,
    Assistant,
    User,
    Tool,
  }

  public class OllamaMessage
  {
    public required OllamaRole Role { get; set; }
    public required string Content { get; set; }
    [JsonProperty(PropertyName = "tool_calls")]
    public List<OllamaToolCalls> ToolCalls { get; set; } = new List<OllamaToolCalls>();
  }

  public class OllamaToolCalls
  {
    public required OllamaFunctionCall Function { get; set; }
  }

  public class OllamaFunctionCall
  {
    public required string Name { get; set; }
    public required Dictionary<string, string> Arguments { get; set; }
  }


  public class OllamaPayload
  {
    public required string Model { get; set; }
    public required List<OllamaMessage> Messages { get; set; }
    public required List<OllamaToolDefinition> Tools { get; set; }
  }

  public class OllamaToolDefinition
  {
    /// <summary>
    /// Defaults to "function" (only allowed value by ollama)
    /// </summary>
    public string Type { get; set; } = "function";
    public required OllamaFunction Function { get; set; }
  }

  public class OllamaFunction
  {
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required OllamaFunctionParametersDefinition Parameters { get; set; }
  }

  public class OllamaFunctionParametersDefinition
  {
    public string Type { get; set; } = "object";
    public required Dictionary<string, OllamaParameterPropertyDefinition> Properties { get; set; }
    public required List<string> Required { get; set; }
  }

  public class OllamaParameterPropertyDefinition
  {
    public required string Type { get; set; }
    public required string Description { get; set; }

    [JsonProperty(PropertyName = "_enum")]
    public List<string>? Enum { get; set; }
  }

}
