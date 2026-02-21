# Guia de Teste - Sistema de Autenticação

## Preparação do Backend

### 1. Criar uma Migration para RefreshToken com UserId

```powershell
# No diretório raiz do projeto
dotnet ef migrations add AddUserIdToRefreshToken -p src/backend/PetManager.Infrastructure -s src/backend/PetManager.Api

# Aplicar
dotnet ef database update -p src/backend/PetManager.Infrastructure -s src/backend/PetManager.Api
```

### 2. Criar um Usuário de Teste

Use o Swagger da API para criar um usuário:

```bash
# 1. Inicie o Backend
$env:ASPNETCORE_ENVIRONMENT = 'Development'
dotnet run --project src/backend/PetManager.Api/PetManager.Api.csproj
```

Acesse `http://localhost:5000/swagger` e use o endpoint `POST /api/user/create`:

```json
{
  "name": "João Silva",
  "email": "joao@example.com",
  "cellphone": "11999999999",
  "document": "12345678900",
  "role": 1,
  "username": "joao_silva",
  "password": "SenhaForte123!"
}
```

### 3. Testar Endpoints de Autenticação

#### Login com Usuário

```bash
POST http://localhost:5000/api/auth/login
Content-Type: application/json

{
  "username": "joao_silva",
  "password": "SenhaForte123!"
}
```

Resposta esperada:

```json
{
  "code": "200",
  "message": "Login realizado com sucesso",
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIs...",
    "refreshToken": "ABCD1234567890...",
    "expiresAt": "2025-02-21T16:30:00Z"
  }
}
```

#### Refresh Token

```bash
POST http://localhost:5000/api/auth/refresh
Content-Type: application/json

{
  "refreshToken": "ABCD1234567890..."
}
```

## Teste do Frontend

### 1. Instalar Dependências

```bash
cd src/frontend/petmanager
npm install
npm install react-router-dom  # Se ainda não estiver instalado
```

### 2. Configurar .env.local

```bash
cp .env.example .env.local
```

Edite `.env.local`:

```
VITE_API_URL=http://localhost:5000/api
VITE_API_KEY=your-api-key-here
```

### 3. Executar Frontend

```bash
npm run dev
```

Acesse `http://localhost:5173`

### 4. Fazer Login

1. Na tela de login, insira:
   - Username: `joao_silva`
   - Password: `SenhaForte123!`

2. Clique em "Entrar"

3. Se bem-sucedido, você será redirecionado para `/dashboard`

### 5. Testes Adicionais

#### Teste de Token Expirado

1. Guarde o valor do `accessToken` do login
2. Aguarde alguns segundos (ou configure em `environment.ts` um tempo menor)
3. Tente chamar um endpoint protegido
4. O sistema deve renovar automaticamente usando `refreshToken`

#### Teste de Sessão de 60 minutos

1. Faça login
2. Observe o tokenManager renovando tokens a cada 50 minutos
3. Após 60 minutos, será solicitado novo login

#### Teste de Erro de Credenciais

1. Insira um senha incorreta
2. Deverá aparecer: "Usuário ou senha incorretos" (alerta amarelo)

#### Teste de Erro do Servidor

1. Desative temporariamente o backend
2. Tente fazer login
3. Deverá aparecer: "Erro de conexão..." (alerta vermelho)

## Debugging

### Ver Tokens no Console do Navegador

```javascript
// No console do navegador (F12 > Console)
import TokenManager from "./src/core/TokenManager";
TokenManager.getAccessToken();
TokenManager.getRefreshToken();
TokenManager.getTokens();
```

### Ver Requisições HTTP

1. Abra DevTools (F12)
2. Vá para a aba "Network"
3. Faça login
4. Veja as requisições:
   - `POST /api/auth/login`
   - Respostas com status 200 (sucesso) ou 401 (erro)

### Logs no Backend

Configure o nível de log em `appsettings.Development.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Information"
    }
  }
}
```

## Troubleshooting

| Erro                      | Causa                         | Solução                        |
| ------------------------- | ----------------------------- | ------------------------------ |
| 401 Unauthorized          | Username ou password inválido | Verifique credenciais no banco |
| 500 Internal Server Error | Erro no backend               | Veja logs do backend           |
| Network Error             | Backend offline               | Inicie o backend               |
| Token não salva           | localStorage desativado       | Ative javascript no navegador  |
| Sessão não expira         | Timer não configurado         | Verifique TokenManager         |

## Próximos Passos

- [ ] Adicionar endpoint para registrar novos usuários via frontend
- [ ] Implementar "Esqueci minha senha"
- [ ] Adicionar verificação 2FA
- [ ] Implementar logout automático em 60 minutos sem atividade
- [ ] Adicionar captcha no login
