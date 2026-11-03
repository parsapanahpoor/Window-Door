#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "${BASH_SOURCE[0]}")/.."
if ! command -v dotnet >/dev/null 2>&1 && [ -x /workspace/.setup/dotnet/dotnet ]; then
  export DOTNET_ROOT=/workspace/.setup/dotnet
  export PATH="$DOTNET_ROOT:$PATH"
fi
dotnet --version
dotnet restore Window.sln --disable-parallel
dotnet build Window.sln --no-restore -m:2
