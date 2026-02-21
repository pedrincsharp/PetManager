# Script para iniciar Backend e Frontend simultaneamente (Windows PowerShell)
# Uso: .\start-dev.ps1

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  PetManager - Dev Environment Starter" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Verificar se está no diretório correto
if (-not (Test-Path ".\src\backend") -and -not (Test-Path ".\docker-compose.yml")) {
    Write-Host "Erro: Execute este script da raiz do projeto!" -ForegroundColor Red
    Write-Host "Exemplo: cd C:\caminho\para\Gestão Pet" -ForegroundColor Yellow
    exit 1
}

# Função para iniciar o Backend
function Start-Backend {
    Write-Host ""
    Write-Host "► Iniciando Backend (ASP.NET Core)..." -ForegroundColor Green
    Write-Host "  URL: http://localhost:5225" -ForegroundColor Gray
    Write-Host "  Swagger: http://localhost:5225/swagger" -ForegroundColor Gray
    
    try {
        $env:ASPNETCORE_ENVIRONMENT = 'Development'
        Set-Location "src/backend"
        dotnet run --project "PetManager.Api/PetManager.Api.csproj"
    }
    catch {
        Write-Host "Erro ao iniciar Backend: $_" -ForegroundColor Red
    }
}

# Função para iniciar o Frontend
function Start-Frontend {
    Write-Host ""
    Write-Host "► Iniciando Frontend (React)..." -ForegroundColor Green
    Write-Host "  URL: http://localhost:5173" -ForegroundColor Gray
    
    try {
        Set-Location "src/frontend/petmanager"
        npm run dev
    }
    catch {
        Write-Host "Erro ao iniciar Frontend: $_" -ForegroundColor Red
    }
}

# Verificar dependências
Write-Host "► Verificando dependências..." -ForegroundColor Yellow

# Verificar .NET
try {
    $dotnetVersion = dotnet --version
    Write-Host "  ✓ .NET instalado: $dotnetVersion" -ForegroundColor Green
}
catch {
    Write-Host "  ✗ .NET não encontrado! Instale .NET SDK." -ForegroundColor Red
    exit 1
}

# Verificar Node.js/npm
try {
    $nodeVersion = node --version
    $npmVersion = npm --version
    Write-Host "  ✓ Node.js instalado: $nodeVersion" -ForegroundColor Green
    Write-Host "  ✓ npm instalado: $npmVersion" -ForegroundColor Green
}
catch {
    Write-Host "  ✗ Node.js não encontrado! Instale Node.js." -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "► Iniciando componentes..." -ForegroundColor Cyan
Write-Host ""

# Iniciar Backend em uma nova janela PowerShell
Write-Host "  [1/2] Backend..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", {
    $env:ASPNETCORE_ENVIRONMENT = 'Development'
    Set-Location $PSScriptRoot
    cd "src/backend"
    Write-Host "Iniciando Backend..." -ForegroundColor Green
    dotnet run --project "PetManager.Api/PetManager.Api.csproj"
} -WindowStyle Normal

# Aguardar um pouco para o Backend iniciar
Start-Sleep -Seconds 3

# Iniciar Frontend em uma nova janela PowerShell
Write-Host "  [2/2] Frontend..." -ForegroundColor Yellow
Start-Process powershell -ArgumentList "-NoExit", "-Command", {
    Set-Location $PSScriptRoot
    cd "src/frontend/petmanager"
    
    # Verificar se node_modules existe
    if (-not (Test-Path "node_modules")) {
        Write-Host "► Instalando dependências do Frontend..." -ForegroundColor Yellow
        npm install
    }
    
    Write-Host "Iniciando Frontend..." -ForegroundColor Green
    npm run dev
} -WindowStyle Normal

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  ✓ Componentes iniciados com sucesso!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "URLs de acesso:" -ForegroundColor Cyan
Write-Host "  • Backend:  http://localhost:5225" -ForegroundColor Gray
Write-Host "  • Swagger:  http://localhost:5225/swagger" -ForegroundColor Gray
Write-Host "  • Frontend: http://localhost:5173" -ForegroundColor Gray
Write-Host ""
Write-Host "Para parar, feche as janelas abertas." -ForegroundColor Yellow
Write-Host ""
