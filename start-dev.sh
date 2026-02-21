#!/bin/bash

# Script para iniciar Backend e Frontend simultaneamente (Linux/Mac)
# Uso: chmod +x start-dev.sh && ./start-dev.sh

set -e  # Exit on error

# Cores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
CYAN='\033[0;36m'
GRAY='\033[0;37m'
NC='\033[0m' # No Color

# Print header
echo -e "${CYAN}========================================"
echo "  PetManager - Dev Environment Starter"
echo "========================================${NC}"
echo ""

# Verificar se está no diretório correto
if [ ! -f "docker-compose.yml" ] && [ ! -f "src/backend/PetManager.slnx" ]; then
    echo -e "${RED}Erro: Execute este script da raiz do projeto!${NC}"
    echo -e "${YELLOW}Exemplo: cd /caminho/para/Gestão\ Pet${NC}"
    exit 1
fi

# Função para cleanup ao sair
cleanup() {
    echo ""
    echo -e "${YELLOW}Encerrando processos...${NC}"
    kill $BACKEND_PID 2>/dev/null || true
    kill $FRONTEND_PID 2>/dev/null || true
    exit 0
}

# Trap Ctrl+C
trap cleanup INT TERM

# Verificar dependências
echo -e "${YELLOW}► Verificando dependências...${NC}"

# Verificar .NET
if command -v dotnet &> /dev/null; then
    DOTNET_VERSION=$(dotnet --version)
    echo -e "${GREEN}  ✓ .NET instalado: $DOTNET_VERSION${NC}"
else
    echo -e "${RED}  ✗ .NET não encontrado! Instale .NET SDK.${NC}"
    exit 1
fi

# Verificar Node.js/npm
if command -v node &> /dev/null; then
    NODE_VERSION=$(node --version)
    echo -e "${GREEN}  ✓ Node.js instalado: $NODE_VERSION${NC}"
else
    echo -e "${RED}  ✗ Node.js não encontrado! Instale Node.js.${NC}"
    exit 1
fi

if command -v npm &> /dev/null; then
    NPM_VERSION=$(npm --version)
    echo -e "${GREEN}  ✓ npm instalado: $NPM_VERSION${NC}"
else
    echo -e "${RED}  ✗ npm não encontrado! Instale npm.${NC}"
    exit 1
fi

echo ""
echo -e "${CYAN}► Iniciando componentes...${NC}"
echo ""

# Iniciar Backend
echo -e "${YELLOW}  [1/2] Backend...${NC}"
(
    cd src/backend
    export ASPNETCORE_ENVIRONMENT=Development
    echo -e "${GREEN}Iniciando Backend...${NC}"
    dotnet run --project "PetManager.Api/PetManager.Api.csproj"
) &
BACKEND_PID=$!

# Aguardar um pouco para o Backend iniciar
sleep 3

# Iniciar Frontend
echo -e "${YELLOW}  [2/2] Frontend...${NC}"
(
    cd src/frontend/petmanager
    
    # Verificar se node_modules existe
    if [ ! -d "node_modules" ]; then
        echo -e "${YELLOW}► Instalando dependências do Frontend...${NC}"
        npm install
    fi
    
    echo -e "${GREEN}Iniciando Frontend...${NC}"
    npm run dev
) &
FRONTEND_PID=$!

echo ""
echo -e "${CYAN}========================================"
echo "  ✓ Componentes iniciados com sucesso!"
echo "========================================${NC}"
echo ""
echo -e "${CYAN}URLs de acesso:${NC}"
echo -e "${GRAY}  • Backend:  http://localhost:5000"
echo "  • Swagger:  http://localhost:5000/swagger"
echo "  • Frontend: http://localhost:5173${NC}"
echo ""
echo -e "${YELLOW}Para parar, pressione Ctrl+C${NC}"
echo ""

# Aguardar processos
wait $BACKEND_PID
wait $FRONTEND_PID
