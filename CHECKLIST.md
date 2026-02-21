# Checklist de Implementação - Sistema de Login PetManager

## ✅ Backend - Pronto para Usar

### Verificação

- [x] Endpoint `/api/auth/login` criado em AuthController
- [x] Método `LoginAsync` implementado em AuthService
- [x] TokenService com suporte a User tokens
- [x] RefreshToken modelo atualizado com UserId
- [x] AppDbContext configurado com relação User/RefreshToken
- [x] IUserRepository já registrado em Program.cs

### Próximos Passos Backend

1. **Criar Migration** (IMPORTANTE!)

```powershell
# No diretório raiz
cd src/backend
dotnet ef migrations add AddUserIdToRefreshToken -p PetManager.Infrastructure -s PetManager.Api
```

2. **Aplicar Migration**

```powershell
dotnet ef database update -p PetManager.Infrastructure -s PetManager.Api
# OU
.\migrate.ps1  # Se existir script
```

3. **Criar Usuário de Teste**

- Abra Swagger: `http://localhost:5000/swagger`
- Use endpoint `POST /api/user/create` (se existir)
- Ou insira diretamente no banco via SQL:

```sql
INSERT INTO users (id, username, password_hash, name, email, cellphone, document, role, created_at, updated_at, status)
VALUES (
  gen_random_uuid(),
  'joao_silva',
  '$2a$11$...',  -- Hash BCrypt de 'SenhaForte123!'
  'João Silva',
  'joao@example.com',
  '11999999999',
  '12345678900',
  1,  -- Role Attendant
  NOW(),
  NOW(),
  'Active'
);
```

4. **Verificar Endpoints**

- POST `/api/auth/login` → Deve retornar accessToken
- POST `/api/auth/token` → Continua funcionando
- POST `/api/auth/refresh` → Com novo refreshToken

---

## ✅ Frontend - Quase Pronto

### Verificação

- [x] Clean Architecture implementada
- [x] Página de login criada
- [x] TokenManager singleton funcionando
- [x] ApiClient com auto-refresh
- [x] Componentes reutilizáveis (Alert, ProtectedRoute)
- [x] App com routing

### Próximo Passo: Instalar Dependências

```bash
cd src/frontend/petmanager

# 1. Instalar react-router-dom (essencial!)
npm install react-router-dom

# 2. Verificar instalação
npm list react-router-dom

# 3. Opcional: reinstalar tudo se houver erro
rm -r node_modules package-lock.json
npm install
```

### Configurar Variáveis de Ambiente

```bash
# 1. Criar .env.local
cp .env.example .env.local

# 2. Editar com sua configuração
# VITE_API_URL=http://localhost:5000/api
# VITE_API_KEY=your-api-key-here
```

---

## 🚀 Teste Completo

### Terminal 1: Backend

```powershell
cd src/backend
$env:ASPNETCORE_ENVIRONMENT = 'Development'
dotnet run --project PetManager.Api/PetManager.Api.csproj
# Aguarde: "Now listening on: http://localhost:5000"
```

### Terminal 2: Frontend

```bash
cd src/frontend/petmanager
npm run dev
# Aguarde: "VITE v... ready in ... ms"
# "local:   http://localhost:5173"
```

### Teste no Navegador

1. Abra `http://localhost:5173`
2. Vá para `/login`
3. Insira credenciais do usuário criado
4. Clique em "Entrar"
5. Deve redirecionar para `/dashboard`

---

## 🔍 Verificações de Sanidade

### Backend

- [ ] Migration criada e aplicada sem erros
- [ ] Swagger mostra endpoint `/auth/login`
- [ ] Usuário cadastrado no banco
- [ ] Database sem erros de foreign key

### Frontend

- [x] Todos os arquivos no lugar
- [ ] `npm install` executado
- [ ] `.env.local` criado e preenchido
- [ ] `npm run dev` iniciando sem erros

### Integração

- [ ] Frontend conecta ao backend sem CORS
- [ ] Login retorna 200 e tokens válidos
- [ ] Dashboard acessível após login
- [ ] Logout limpa tokens

---

## 📝 Commands Rápidos de Referência

```bash
# Frontend Setup
cd src/frontend/petmanager && npm install && npm run dev

# Backend Setup
cd src/backend && dotnet ef database update -p PetManager.Infrastructure -s PetManager.Api

# Build Frontend
npm run build

# Lint
npm run lint

# Test Login (com curl)
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"joao_silva","password":"SenhaForte123!"}'
```

---

## 🐛 Troubleshooting Rápido

| Erro                                    | Solução                              |
| --------------------------------------- | ------------------------------------ |
| "Cannot find module 'react-router-dom'" | `npm install react-router-dom`       |
| "CORS policy"                           | Backend precisa de CORS configurado  |
| "Invalid username/password"             | Usuario não existe no banco          |
| "401 Unauthorized"                      | Token expirou, fazer login novamente |
| "Network Error"                         | Backend offline, verificar terminal  |
| Migration error                         | Eliminar migrations testes, refazer  |

---

## 📋 Documentação Relacionada

Para mais informações, veja:

- **IMPLEMENTATION_SUMMARY.md** - Resumo completo
- **ARCHITECTURE.md** - Explicação da arquitetura
- **SETUP.md** - Guia detalhado de setup
- **EXAMPLES.md** - Exemplos práticos
- **LOGIN_TEST_GUIDE.md** - Guia de testes

---

## ✨ Pronto para Começar?

1. Crie a migration
2. Crie um usuário de teste
3. Instale dependências do frontend
4. Configure `.env.local`
5. Rode `npm run dev`
6. Teste o login

**Sucesso! 🎉**
