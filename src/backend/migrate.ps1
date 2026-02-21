<#
  migrate.ps1
  Uso: .\migrate.ps1 <NomeMigration> [-Project <path>] [-StartupProject <path>] [-Context <DbContextName>]

  - Por padrão, usa:
    Project:  PetManager.Infrastructure/PetManager.Infrastructure.csproj
    Startup:  PetManager.Api/PetManager.Api.csproj

  Execute a partir da pasta `src/backend` ou use o caminho completo ao chamar o script.
#>

param(
    [string]$Name,
    [string]$Project,
    [string]$StartupProject,
    [string]$Context
)

if (-not $Name -or $Name -in @('-h','--help')) {
    Write-Host "Uso: .\migrate.ps1 <MigrationName> [-Project <path>] [-StartupProject <path>] [-Context <DbContextName>]" -ForegroundColor Yellow
    Write-Host "Exemplo: .\migrate.ps1 AddPetEntity" -ForegroundColor Yellow
    exit 1
}

if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    Write-Error "dotnet não está disponível no PATH. Instale o .NET SDK e o tool 'dotnet-ef' se necessário."
    exit 2
}

if (-not $Project) {
    $Project = Join-Path $PSScriptRoot 'PetManager.Infrastructure\PetManager.Infrastructure.csproj'
}
if (-not $StartupProject) {
    $StartupProject = Join-Path $PSScriptRoot 'PetManager.Api\PetManager.Api.csproj'
}

Write-Host "Executando: dotnet ef migrations add $Name`n  - Project: $Project`n  - Startup: $StartupProject" -ForegroundColor Cyan

$psArgs = @('ef','migrations','add',$Name,'-p',$Project,'-s',$StartupProject)
if ($Context) { $psArgs += @('-c',$Context) }

& dotnet @psArgs
$rc = $LASTEXITCODE

if ($rc -ne 0) {
    Write-Error "dotnet ef falhou com o código de saída $rc"
    exit $rc
}

Write-Host "Migration '$Name' criada com sucesso." -ForegroundColor Green

Write-Host "Aplicando migration ao banco de dados (dotnet ef database update)..." -ForegroundColor Cyan
$updateArgs = @('ef','database','update','-p',$Project,'-s',$StartupProject)
if ($Context) { $updateArgs += @('-c',$Context) }

& dotnet @updateArgs
$rc2 = $LASTEXITCODE

if ($rc2 -ne 0) {
    Write-Error "dotnet ef database update falhou com o código de saída $rc2"
    exit $rc2
}

Write-Host "Database atualizado com sucesso." -ForegroundColor Green
exit 0
