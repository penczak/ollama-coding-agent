# tools/dotnet_tools.py

import subprocess
from server import mcp

@mcp.tool(description="Tool that builds a dotnet project by calling `dotnet build path_to_project`")
def dotnet_build(path_to_project: str) -> str:
    """
    Build a dotnet project by calling `dotnet build {path_to_project}`
    Args:
        path_to_project: Filepath to .csproj file
    Returns:
        A string of the build output.
    """
    result = subprocess.run(
        ["dotnet", "build", path_to_project],
        stdout=subprocess.PIPE,
        stderr=subprocess.PIPE,
        text=True
    )

    output = f"EXIT_CODE: {result.returncode}\nSTDOUT: {result.stdout}\nSTDERR: {result.stderr}"

    return output


@mcp.tool(description="Tool that runs tests of a dotnet project by calling `dotnet test path_to_project`")
def dotnet_test(path_to_project: str) -> str:
    """
    Test a dotnet project by calling `dotnet build {path_to_project}`
    Args:
        path_to_project: Filepath to .csproj file
    Returns:
        A string of the build output.
    """
    result = subprocess.run(
        ["dotnet", "test", path_to_project],
        stdout=subprocess.PIPE,
        stderr=subprocess.PIPE,
        text=True
    )

    output = f"EXIT_CODE: {result.returncode}\nSTDOUT: {result.stdout}\nSTDERR: {result.stderr}"

    return output
