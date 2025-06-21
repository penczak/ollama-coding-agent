# tools/dotnet_tools.py

import subprocess
from server import mcp
@mcp.tool(description="Tool that builds a dotnet project by calling `dotnet build path_to_project`")
def build_dotnet_project(path_to_project: str) -> str:
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

    stdout_str = result.stdout
    stderr_str = result.stderr
    exit_code = result.returncode

    print("STDOUT:")
    print(stdout_str)

    print("STDERR:")
    print(stderr_str)

    output = f"EXIT_CODE: {exit_code}\nSTDOUT: {stdout_str}\nSTDERR: {stderr_str}"

    return output
