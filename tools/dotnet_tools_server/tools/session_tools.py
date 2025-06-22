# tools/session_tools.py

import subprocess
from server import mcp

@mcp.tool(description="Tool that gives context for an extended session")
def session_build(path_to_project: str) -> str:

    output = f"EXIT_CODE: {result.returncode}\nSTDOUT: {result.stdout}\nSTDERR: {result.stderr}"

    return output