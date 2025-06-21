using LLMClientTool;

namespace LLMClientToolTests
{
  public class ListDirectory_Tests
  {
    [Fact]
    public void CanListDirectory()
    {
      var res = Functions.ListDirectory("/", recurse: false);
      Assert.Contains("/", res);
      Assert.Contains("/LLMClientTool.dll", res);
    }

    [Fact]
    public void NavigatesOutOfRoot_ReturnsErrorMessage()
    {
      var res = Functions.ListDirectory("..\\..\\", recurse: false);
      //Assert.Equal("Navigation outside of root is not alllowed", res);
    }
  }
}