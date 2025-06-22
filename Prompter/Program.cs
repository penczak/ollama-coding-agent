// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, World!");

const string CMD_CONFIG = "-m ollama:qwen3:8b --config \"C:\\Users\\Daniel\\source\\repos\\ollama-coding-agent\\mcp.json\"";

const string SYSTEM_PROMPT = @"
You are a coding assistant with access to several tools for file reading & manipulation as well as dotnet commands for building and running tests. 
";
const string CHECKER_SYSTEM_PROMPT = @"
You are an assistant whose job is to verify that a task has been completed by reading files, including source code, and running tests.
";
string prompt = "Create a file called \"test.txt\" with content \"abcd\"";

while (true) {

  var output = await RunProcess("mcphost", $"{CMD_CONFIG} --system-prompt \"{SYSTEM_PROMPT}\" -p \"{prompt}\" --quiet");
  Console.WriteLine(output);

  var checkerOutput = await RunProcess("mcphost", $"{CMD_CONFIG} --system-prompt \"{SYSTEM_PROMPT}\" -p \"{prompt}\" --quiet");
}

async Task<string> RunProcess(string filename, string args)
{
  var startInfo = new ProcessStartInfo
  {
    FileName = filename,
    Arguments = args,
    CreateNoWindow = true,
    RedirectStandardOutput = true,
  };
  string output = "";
  using var p = Process.Start(startInfo);
  p.Start();
  p.OutputDataReceived += (_, data) => output += data.Data;
  p.BeginOutputReadLine();
  await p.WaitForExitAsync();
  return output;
}