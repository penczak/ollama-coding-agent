using static System.Net.Mime.MediaTypeNames;

namespace LLMClientTool
{
  public class Functions
  {
    public static string ListDirectory(string relativePathToDirectory, bool recurse)
    {
      var err = VerifyRelativeDirectoryPath(relativePathToDirectory, out string root, out string path);
      if (err != null) return err;

      var relativePathEntries = Directory.EnumerateFileSystemEntries(path, "*", new EnumerationOptions() { RecurseSubdirectories = recurse, AttributesToSkip = FileAttributes.Directory })
        .Select(fullPathEntry => fullPathEntry.Replace(root, string.Empty));
      return string.Join(Environment.NewLine, relativePathEntries);
    }

    private static string? VerifyRelativeDirectoryPath(string relativePathToDirectory, out string root, out string path)
    {
      root = Environment.CurrentDirectory;
      path = Path.Join(root, relativePathToDirectory);

      if (!path.StartsWith(root))
      {
        return "Navigation outside of root is not alllowed";
      }

      if (!Directory.Exists(path))
      {
        return $"Directory not found: {relativePathToDirectory}";
      }
      return null;
    }

    private static string? VerifyRelativeFilePath(string relativePathToFile, out string root, out string path)
    {
      root = Environment.CurrentDirectory;
      path = Path.Join(root, relativePathToFile);

      if (!path.StartsWith(root))
      {
        return "Navigation outside of root is not alllowed";
      }

      if (!File.Exists(path))
      {
        return $"File not found: {relativePathToFile}";
      }
      return null;
    }

    public static string GetFileContents(string relativePathToFile)
    {
      var err = VerifyRelativeFilePath(relativePathToFile, out string root, out string path);
      if (err != null) return err;

      string text;
      try
      {
        text = File.ReadAllText(path);
      }
      catch (Exception ex)
      {
        return $"Error reading file: {ex.Message}";
      }

      return FormatTextWithLineNumbers(text);
    }

    private static string FormatTextWithLineNumbers(string input)
    {
      return string.Join(Environment.NewLine, input.Split(Environment.NewLine).Select((s, i) => $"{i + 1}:\t{s}"));
    }

    public static string ReplaceFileLines(string relativePathToFile, string newContent, int lineNumberStart, int linesCountToReplace)
    {
      var err = VerifyRelativeFilePath(relativePathToFile, out string root, out string path);
      if (err != null) return err;

      string text;
      try
      {
        text = File.ReadAllText(path);
      }
      catch (Exception ex)
      {
        return $"Error reading file: {ex.Message}";
      }

      var newLines = text.Split(Environment.NewLine).Select((s, i) => (s, i: i + 1)).Where(x => x.i < lineNumberStart || x.i >= lineNumberStart + linesCountToReplace).ToList();
      newLines.InsertRange(lineNumberStart - 1, newContent.Split(Environment.NewLine).Select((s, i) => (s, i)));

      var newLinesJoined = string.Join(Environment.NewLine, newLines.Select(x => x.s));

      File.WriteAllText(path, newLinesJoined);

      return FormatTextWithLineNumbers(newLinesJoined);
      //var newLines = originalLines.TakeWhile(x => x.i != lineNumberStart).ToList();
      //newLines.AddRange(newContent.Split(Environment.NewLine).Select((s, i) => (s, i + lineNumberStart)));
      //newLines.AddRange(originalLines.Skip(linesCountToReplace));
    }

  }
}
