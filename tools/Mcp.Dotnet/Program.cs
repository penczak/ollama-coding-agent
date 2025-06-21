using Mcp.Dotnet;
using Microsoft.Extensions.Logging;
using CommandLineOptions = Mcp.Dotnet.CommandLineOptions;

internal partial class Program
{
  private static async Task Main(string[] args)
  {
    var options = CommandLineOptions.Parse(args);
    options.NoAuth = true;
    options.UseStdio = true;

    // Display all registered tools at startup for easier debugging
    Environment.SetEnvironmentVariable("MCP_DEBUG_TOOLS", "true");

    await StdioTransport.RunWithStdioTransport(options, LogLevel.Information);
  }
}