# Development environment

## Locations
Cloud checkout: `/workspace/Window-Door`.
Active Windows development checkout: `C:\Users\Parsapanahpoor\Documents\ChatGPT\Panjerehyab`.
Original Windows source (preserved): `C:\Users\Parsapanahpoor\Desktop\Drives\App\Projects\Panjerehyab\Source Code\Window14011014\Window`.
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

## SDK selection
`global.json` sets SDK 8.0.427 as the minimum and permits stable newer major SDKs (`latestMajor`). The existing cloud SDK remains supported; the Windows machine currently selects SDK 10.0.401. CI installs the baseline SDK from global.json. The application target remains `net8.0`; running it requires the .NET 8 and ASP.NET Core 8 runtimes. This is compatibility across existing environments, not a guarantee of identical compiler versions.

## Windows
Use PowerShell 7 (`pwsh`) and a compatible SDK, then:
```powershell
Set-Location 'C:\Users\Parsapanahpoor\Documents\ChatGPT\Panjerehyab'
pwsh -NoProfile -File scripts/dev.ps1 Doctor
pwsh -NoProfile -File scripts/dev.ps1 Setup
pwsh -NoProfile -File scripts/dev.ps1 Verify
```
Use an isolated development SQL Server database. The current configuration uses Windows integrated authentication; cloud/Linux requires a suitable development connection override. ASP.NET Core reads `ConnectionStrings__WindowConnection` from the process environment. Enter its value through a secure local configuration mechanism, not source control or chat. The setting is a database credential, not an HTTP proxy secret.

`Setup` restores NuGet packages and the repository-local EF Core 8.0.0 tool. `Verify` confirms that the three recovered Log view models are tracked, checks diff whitespace, and restores/builds Release. It propagates failure and does not claim test coverage. Existing warnings remain visible. `Build -Configuration Debug` is available for interactive development.

The Windows SQL Server instance `localhost\MSSQLSERVER2022` is running; a read-only catalog check on 2026-11-04 returned no user databases. Do not assume a usable application database exists. No migration or seed operation has been run as part of setup.

### Development secrets and startup
The Web project has a UserSecretsId. Configure a development database before starting; this example names a database that must first be created/restored and validated:

```powershell
dotnet user-secrets set 'ConnectionStrings:WindowConnection' 'Server=localhost\MSSQLSERVER2022;Database=Panjerehyab_Dev;Trusted_Connection=True;TrustServerCertificate=True' --project Window.Web
dotnet user-secrets set 'kavenegar:apikey' 'YOUR_TEST_API_KEY' --project Window.Web
pwsh -NoProfile -File scripts/dev.ps1 Run
```

User Secrets are stored outside the repository as plaintext; use them only for development, not production secret management. The Run wrapper requires a connection override (User Secrets or environment), sets Development for its process, and listens on `http://127.0.0.1:5075`. It does not apply migrations. Do not use live integration credentials for validation.

VS Code recommendations and tasks live under `.vscode/`. Open Window.sln with C# Dev Kit; use `Panjerehyab: build`, `Panjerehyab: verify`, and `Panjerehyab: run` from Tasks. Debugging can use C# Dev Kit's solution explorer after configuration. `.editorconfig` governs new edits; do not reformat the legacy codebase wholesale.

For migration review, generate SQL without applying it:

```powershell
New-Item -ItemType Directory -Force artifacts | Out-Null
dotnet ef migrations script --idempotent --project Window.Infra.Data --startup-project Window.Web --output artifacts/migrations.sql
```

The frontend assets already exist under wwwroot. Nested vendor package.json files do not imply a required root npm install.

### Continuous integration
`.github/workflows/build.yml` runs Verify on Windows for master pushes and pull requests, with read-only repository permission. It builds from a clean checkout, so locally ignored source cannot silently satisfy compilation. It does not deploy, migrate a database, or run business tests. Read the workflow run status separately from local verification.

References: [SDK selection](https://learn.microsoft.com/en-us/dotnet/core/tools/global-json), [development User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0).

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
