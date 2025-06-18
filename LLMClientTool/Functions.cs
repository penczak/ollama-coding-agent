using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLMClientTool
{
  public class Functions
  {
    public static string ListDirectory(string relativePathToDirectory, bool recurse)
    {
      var root = Environment.CurrentDirectory;
      var path = Path.Join(root, relativePathToDirectory);

      if (!Directory.Exists(path))
      {
        return $"Directory not found: {relativePathToDirectory}";
      }

      var relativePathEntries = Directory.EnumerateFileSystemEntries(path, "*", new EnumerationOptions() { RecurseSubdirectories = recurse })
        .Select(fullPathEntry => fullPathEntry.Replace(root, string.Empty));
      return string.Join(Environment.NewLine, relativePathEntries);
    }

    public static string GetFileContents(string relativePathToFile)
    {
      var root = Environment.CurrentDirectory;
      var path = Path.Join(root, relativePathToFile);

      if (!File.Exists(path))
      {
        return $"File not found: {relativePathToFile}";
      }

      try
      {
        return File.ReadAllText(path);
      }
      catch (Exception ex)
      {
        return $"Error reading file: {ex.Message}";
      }
    }

  }
}
