# Scripts de Inicialização do Projeto

Scripts para iniciar Backend e Frontend simultaneamente.

## Windows (PowerShell)

### Setup Inicial

1. **Primeira execução**: Permita execução de scripts PowerShell

```powershell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

2. **Execute o script**

```powershell
.\start-dev.ps1
```

### O que faz

- ✅ Verifica dependências (.NET, Node.js, npm)
- ✅ Inicia Backend em nova janela PowerShell (porta 5000)
- ✅ Aguarda 3 segundos
- ✅ Inicia Frontend em nova janela PowerShell (porta 5173)
- ✅ Mostra URLs de acesso

### Parar

- Feche as duas janelas que abriram ou pressione Ctrl+C

---

## Linux / macOS (Bash)

### Setup Inicial

1. **Primeira execução**: Dê permissão de execução

```bash
chmod +x start-dev.sh
```

2. **Execute o script**

```bash
./start-dev.sh
```

### O que faz

- ✅ Verifica dependências (.NET, Node.js, npm)
- ✅ Inicia Backend em background (porta 5000)
- ✅ Aguarda 3 segundos
- ✅ Instala dependências Frontend se necessário
- ✅ Inicia Frontend em foreground (porta 5173)
- ✅ Mostra URLs de acesso

### Parar

```bash
Ctrl+C
```

---

## URLs de Acesso

| Serviço      | URL                           |
| ------------ | ----------------------------- |
| **Backend**  | http://localhost:5000         |
| **Swagger**  | http://localhost:5000/swagger |
| **Frontend** | http://localhost:5173         |

---

## Requisitos

- **.NET SDK** >= 8.0
  - Windows: https://dotnet.microsoft.com/download
  - Linux/Mac: `brew install dotnet` (Mac) ou veja https://dotnet.microsoft.com/download

- **Node.js** >= 18
  - Windows: https://nodejs.org/
  - Linux/Mac: `brew install node` ou veja https://nodejs.org/

- **PostgreSQL** rodando (Docker)
  ```bash
  docker-compose up -d
  ```

---

## Troubleshooting

### PowerShell: "não é possível carregar arquivo"

```powershell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

### PowerShell: ".NET não encontrado"

- Instale .NET SDK: https://dotnet.microsoft.com/download
- Reinicie o terminal

### Bash: "Permission denied"

```bash
chmod +x start-dev.sh
```

### Bash: ".NET/Node não encontrado"

- Certifique-se de que estão instalados e no PATH
- Teste: `dotnet --version` e `node --version`

### Porta 5000/5173 já em uso

- Mude em `environment.ts` (Frontend) ou `appsettings.json` (Backend)
- Ou encerre o processo usando a porta:
  - Windows: `netstat -ano | findstr :5000`
  - Linux/Mac: `lsof -i :5000`

---

## Uso Alternativo (Manual)

Se preferir iniciar manualmente:

### Terminal 1 - Backend

```powershell
# Windows
$env:ASPNETCORE_ENVIRONMENT = 'Development'
cd src/backend
dotnet run --project PetManager.Api/PetManager.Api.csproj

# Linux/Mac
cd src/backend
export ASPNETCORE_ENVIRONMENT=Development
dotnet run --project PetManager.Api/PetManager.Api.csproj
```

### Terminal 2 - Frontend

```bash
cd src/frontend/petmanager
npm run dev
```

---

## Notas

- Backend usa **CORS** configurado para `http://localhost:5173`
- Frontend precisa de dependências instaladas (`npm install`)
- Ambos os scripts verificam automaticamente dependências
- Os scripts criam variáveis de ambiente necessárias

---

## Próximos Passos

Após iniciar:

1. Acesse http://localhost:5173
2. Teste o login com credenciais do banco
3. Veja logs em ambas as janelas/terminais
4. Para debug, abra DevTools no navegador (F12)

---

**Desenvolvido para: PetManager System**
