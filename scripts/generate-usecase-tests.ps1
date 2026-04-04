param()

$ErrorActionPreference = "Stop"

$repoRoot = (Resolve-Path (Join-Path $PSScriptRoot "..")).ProviderPath
$generatorProject = Join-Path $repoRoot "tools\UseCaseTestGenerator\UseCaseTestGenerator.csproj"

dotnet run --project $generatorProject --configuration Debug -- $repoRoot
