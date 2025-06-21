using Mcp.Net.Server.ServerBuilder;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mcp.Dotnet
{
  public static class StdioTransport
  {
    /// <summary>
    /// Run the server with stdio transport
    /// </summary>
    public static async Task RunWithStdioTransport(CommandLineOptions options, LogLevel logLevel)
    {
      Console.WriteLine("Starting MCP server with stdio transport...");

      // Create a cancellation token source for graceful shutdown
      var cancellationSource = new CancellationTokenSource();
      Console.CancelKeyPress += (sender, e) =>
      {
        if (cancellationSource.IsCancellationRequested == true)
        {
          // workaround for cancellation hanging (second press will exit)
          Environment.Exit(0);
        }
        Console.WriteLine("Shutdown signal received, beginning graceful shutdown");
        e.Cancel = true; // Prevent immediate termination
        cancellationSource.Cancel();
      };

      // Use the new explicit transport selection with builder pattern
      var serverBuilder = McpServerBuilder
          .ForStdio()
          .WithName(options.ServerName ?? "Dotnet Tools MCP Server")
          .WithVersion("1.0.0")
          .WithInstructions("MCP Server with tools for building and testing dotnet projects")
          .WithLogLevel(logLevel);

      // Add the entry assembly to scan for tools
      Assembly? entryAssembly = Assembly.GetEntryAssembly();
      if (entryAssembly != null)
      {
        Console.WriteLine($"Scanning entry assembly for tools: {entryAssembly.FullName}");
      }

      //// Add external tools assembly
      //serverBuilder.WithAdditionalAssembly(
      //    typeof(Mcp.Net.Examples.ExternalTools.UtilityTools).Assembly
      //);

      // Add custom tool assemblies if specified
      if (options.ToolAssemblies != null)
      {
        foreach (var assemblyPath in options.ToolAssemblies)
        {
          try
          {
            var assembly = Assembly.LoadFrom(assemblyPath);
            serverBuilder.WithAdditionalAssembly(assembly);
            Console.WriteLine($"Added tool assembly: {assembly.GetName().Name}");
          }
          catch (Exception ex)
          {
            Console.WriteLine($"Failed to load assembly {assemblyPath}: {ex.Message}");
          }
        }
      }

      // Configure common log levels using the specified log level
      serverBuilder.ConfigureCommonLogLevels(
          toolsLevel: logLevel,
          transportLevel: logLevel,
          jsonRpcLevel: logLevel
      );

      // Configure authentication based on command line options
      if (options.NoAuth)
      {
        // Disable authentication completely
        serverBuilder.WithAuthentication(auth => auth.WithNoAuth());
        Console.WriteLine("Authentication disabled via --no-auth flag");
      }
      else
      {
        // Configure standard authentication with API keys
        serverBuilder.WithAuthentication(auth =>
        {
          // Configure API key options
          auth.WithApiKeyOptions(options =>
          {
            options.HeaderName = "X-API-Key";
            options.QueryParamName = "api_key";
            options.DevelopmentApiKey = "dev-only-api-key"; // Only for dev/testing
          });

          // Add API keys with user IDs and claims (use realistic GUIDs for real keys)
          auth.WithApiKey(
              "api-f85d077e-4f8a-48c8-b9ff-ec1bb9e1772c", // Real API key example
              "user1",
              new Dictionary<string, string> { ["role"] = "admin" }
          );
          auth.WithApiKey(
              "api-2e37dc50-b7a9-4c3d-8a88-99953c99e64b", // Real API key example
              "user2",
              new Dictionary<string, string> { ["role"] = "user" }
          );

          // Configure secured paths if needed
          auth.WithSecuredPaths("/sse", "/messages");
        });
      }

      // Start the server
      var server = await serverBuilder.StartAsync();

      Console.WriteLine("Server started with stdio transport");
      Console.WriteLine("Press Ctrl+C to stop the server.");

      // Wait for cancellation
      try
      {
        await Task.Delay(Timeout.Infinite, cancellationSource.Token);
      }
      catch (OperationCanceledException)
      {
        Console.WriteLine("Server shutdown complete");
      }
    }
  }
}
