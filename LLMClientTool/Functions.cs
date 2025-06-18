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
      if (false == Directory.Exists(path))
      {
        return $"Directory not found: {path}";
      }

      var relativePathEntries = Directory.EnumerateFileSystemEntries(path, "*", new EnumerationOptions() { RecurseSubdirectories = recurse })
        .Select(fullPathEntry => fullPathEntry.Replace(root, string.Empty));
      return string.Join(Environment.NewLine, relativePathEntries);
    }
  }
}
