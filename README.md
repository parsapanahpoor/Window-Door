# Window-Door
Window and Door Project

## Panjerehyab development

ASP.NET Core MVC / .NET 8, EF Core, and SQL Server.

- [Documentation index](docs/README.md)
- [Local and cloud development](docs/development.md)
- [Spec-driven workflow](specs/README.md)
- [Specification template](specs/templates/spec.md)

```powershell
pwsh -NoProfile -File ./scripts/dev.ps1 Setup
pwsh -NoProfile -File ./scripts/dev.ps1 Verify
```

Configure an isolated development database as described in the guide before running `scripts/dev.ps1 Run`. Build verification and full application readiness are tracked separately in the baseline spec.
