[CmdletBinding()]
param(
    [ValidateSet('Doctor', 'Setup', 'Build', 'Verify', 'Run')]
    [string]$Action = 'Doctor',
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release'
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$repoRoot = Split-Path $PSScriptRoot -Parent
$webProject = 'Window.Web/Window.Web.csproj'

function Invoke-DotNet {
    param([string[]]$Arguments)
    & dotnet @Arguments
    if ($LASTEXITCODE -ne 0) { throw "dotnet $($Arguments[0]) failed (exit $LASTEXITCODE)." }
}

Push-Location $repoRoot
try {
    switch ($Action) {
        'Doctor' {
            Invoke-DotNet -Arguments @('--version')
            Invoke-DotNet -Arguments @('--list-runtimes')
            & git status --short --branch
            if ($LASTEXITCODE -ne 0) { throw 'Cannot read repository status.' }
            Write-Host 'Runtime target: net8.0. SDK selection: global.json.'
            Write-Host 'See docs/development.md for local database and secret configuration.'
        }
        'Setup' {
            Invoke-DotNet -Arguments @('restore', 'Window.sln')
            Invoke-DotNet -Arguments @('tool', 'restore')
        }
        'Build' {
            Invoke-DotNet -Arguments @('build', 'Window.sln', '-c', $Configuration, '--nologo')
        }
        'Verify' {
            $requiredModels = @(
                'FilterLogForInquiryViewModel.cs',
                'FilterLogVisitSellerProfileViewModel.cs',
                'LogForBrandsViewModel.cs'
            )
            foreach ($model in $requiredModels) {
                $path = "Window.Domain/ViewModels/Admin/Log/$model"
                if (-not (Test-Path -LiteralPath $path)) { throw "Required source missing: $path" }
                & git ls-files --error-unmatch -- $path | Out-Null
                if ($LASTEXITCODE -ne 0) { throw "Source exists locally but is not tracked: $path" }
            }
            & git diff --check
            if ($LASTEXITCODE -ne 0) { throw 'Whitespace errors in working tree.' }
            & git diff --cached --check
            if ($LASTEXITCODE -ne 0) { throw 'Whitespace errors in staged changes.' }
            Invoke-DotNet -Arguments @('restore', 'Window.sln')
            Invoke-DotNet -Arguments @('build', 'Window.sln', '-c', $Configuration, '--no-restore', '--nologo')
            Write-Host 'Build verification passed. No automated test project currently exists; this is not an application acceptance test.'
        }
        'Run' {
            # Require an explicit development database override instead of the checked-in default.
            $secretOutput = & dotnet user-secrets list --project $webProject 2>&1
            if ($LASTEXITCODE -ne 0) { throw 'Cannot read development secrets. Run Setup first.' }
            $hasConnectionSecret = @($secretOutput | Where-Object { "$_" -match '^ConnectionStrings:WindowConnection\s*=\s*\S' }).Count -gt 0
            if (-not $env:ConnectionStrings__WindowConnection -and -not $hasConnectionSecret) {
                throw 'Set ConnectionStrings:WindowConnection with dotnet user-secrets or ConnectionStrings__WindowConnection in the environment. See docs/development.md.'
            }
            $previousAspnetEnvironment = $env:ASPNETCORE_ENVIRONMENT
            $previousDotnetEnvironment = $env:DOTNET_ENVIRONMENT
            try {
                $env:ASPNETCORE_ENVIRONMENT = 'Development'
                $env:DOTNET_ENVIRONMENT = 'Development'
                Invoke-DotNet -Arguments @('run', '--project', $webProject, '-c', 'Debug', '--no-launch-profile', '--', '--urls', 'http://127.0.0.1:5075')
            }
            finally {
                $env:ASPNETCORE_ENVIRONMENT = $previousAspnetEnvironment
                $env:DOTNET_ENVIRONMENT = $previousDotnetEnvironment
            }
        }
    }
}
finally { Pop-Location }
