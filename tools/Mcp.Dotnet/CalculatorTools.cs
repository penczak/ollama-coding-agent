using Mcp.Net.Core.Attributes;

internal partial class Program
{
  // 2. Define tools using simple attributes and POCOs
  [McpTool("Calculator", "Math operations")]
  public class CalculatorTools
  {
    // Simple synchronous tool that returns a plain string
    [McpTool("add", "Add two numbers")]
    public string Add(
        [McpParameter(required: true, description: "First number")] double a,
        [McpParameter(required: true, description: "Second number")] double b)
    {
      return $"The sum of {a} and {b} is {a + b}";
    }

    // Async tool with a POCO return type - easiest approach!
    [McpTool("getWeather", "Get weather for a location")]
    public async Task<object> GetWeatherAsync(
        [McpParameter(required: true, description: "Location")] string location)
    {
      // Simulate API call
      await Task.Delay(100);

      // Just return a POCO - no need to deal with ToolCallResult!
      return new
      {
        Location = location,
        Temperature = "72°F",
        Conditions = "Sunny",
        Forecast = new[] { "Clear", "Partly cloudy", "Clear" }
      };
    }
  }
}