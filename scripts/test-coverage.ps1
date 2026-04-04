param(
    [string]$Configuration = "Debug"
)

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot "..")
$testProject = Join-Path $repoRoot "EngConnect.Tests\EngConnect.Tests.csproj"
$testBinRoot = Join-Path $repoRoot "EngConnect.Tests\bin"
$coverageRoot = Join-Path $repoRoot "artifacts\test-results\coverage"
$exclude = "[EngConnect.Application]EngConnect.Application.UseCases.*Course*%2c[EngConnect.Application]EngConnect.Application.UseCases.*Module*%2c[EngConnect.Application]EngConnect.Application.UseCases.*Session*%2c[EngConnect.Application]EngConnect.Application.UseCases.*Resource*%2c[EngConnect.Application]EngConnect.Application.UseCases.*Common*%2c[EngConnect.Application]EngConnect.Application.UseCases.*Extensions*"
$excludeByAttribute = "GeneratedCodeAttribute%2cExcludeFromCodeCoverageAttribute"

Get-Process testhost -ErrorAction SilentlyContinue |
    Where-Object { $_.Path -like "$testBinRoot*" } |
    Stop-Process -Force -ErrorAction SilentlyContinue

Start-Sleep -Seconds 1

if (Test-Path $coverageRoot) {
    Remove-Item -LiteralPath $coverageRoot -Recurse -Force
}

dotnet build $testProject --configuration $Configuration --no-restore -m:1 /p:BuildInParallel=false
if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

dotnet test $testProject --configuration $Configuration --no-build --no-restore -m:1 /p:BuildInParallel=false `
    /p:CollectCoverage=true `
    /p:CoverletOutput="$coverageRoot\" `
    /p:Include='[EngConnect.Application]EngConnect.Application.UseCases.*' `
    /p:Exclude=$exclude `
    /p:ExcludeByAttribute=$excludeByAttribute
exit $LASTEXITCODE
