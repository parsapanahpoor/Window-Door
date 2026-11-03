# Development environment

## Locations
Cloud checkout: `/workspace/Window-Door`.
Owner's Windows checkout: `C:\Users\Parsapanahpoor\Desktop\Drives\App\Projects\Panjerehyab`.
The cloud agent cannot directly access that Windows filesystem. Synchronize source through Git; never copy credentials into documentation.

## Cloud
The published snapshot contains SDK 8.0.427 under /workspace/.setup/dotnet. In each shell:
```bash
export DOTNET_ROOT=/workspace/.setup/dotnet
export PATH="$DOTNET_ROOT:$PATH"
cd /workspace/Window-Door
dotnet restore Window.sln --disable-parallel
dotnet build Window.sln --no-restore -m:2
```
Use `bash scripts/check-development.sh` for version, restore and build checks; it preserves failures. The environment install script installs the SDK if absent and restores dependencies. Do not assume running services survive publication.

## Windows (instructions, not executed by the cloud agent)
Install .NET 8 SDK, then in PowerShell:
```powershell
Set-Location 'C:\Users\Parsapanahpoor\Desktop\Drives\App\Projects\Panjerehyab'
dotnet --info
dotnet restore Window.sln --disable-parallel
dotnet build Window.sln --no-restore -m:2
```
Use an isolated development SQL Server database. The current configuration uses Windows integrated authentication; cloud/Linux requires a suitable development connection override. ASP.NET Core reads `ConnectionStrings__WindowConnection` from the process environment. Enter its value through a secure local configuration mechanism, not source control or chat. The setting is a database credential, not an HTTP proxy secret.

## Startup gate
The original cloud baseline (96a4bca) lacked three view-model types. Upstream commit 2e842cb now tracks those files and has been incorporated into this checkout. The owner also reports a successful Windows-local build. See project-memory.md for the current cloud build result; database and HTTP behavior still require validation. After a successful build and development database configuration:
```bash
cd Window.Web
ASPNETCORE_ENVIRONMENT=Development dotnet run --no-build --no-launch-profile --urls http://127.0.0.1:5075
```
PowerShell: set `$env:ASPNETCORE_ENVIRONMENT='Development'` before the same dotnet command (without the Bash prefix).
Validate a representative database-backed page and expected HTML, not merely an open port. Check both authorized and unauthorized behavior. Use local HTTP requests; do not publish loopback preview links. Do not invoke live SMS/payment operations.

Review migrations before applying them to an isolated database; no database migration or seeding has been validated in this environment. No dedicated automated test project currently exists.

## Publication
Saving an environment draft does not publish it. Review/save and Publish are user-interface operations. Git publication and environment publication are separate actions; report each independently. Keep docs and scripts in Git so they can be shared with the Windows checkout.
