# 🚀 Quick Start - PetManager

**Comece aqui em 2 minutos**

---

## 1️⃣ Inicie o Projeto

### Windows

```powershell
.\start-dev.ps1
```

### Linux / macOS

```bash
chmod +x start-dev.sh
./start-dev.sh
```

✅ Backend em http://localhost:5000  
✅ Frontend em http://localhost:5173  
✅ API Docs em http://localhost:5000/swagger

---

## 2️⃣ Setup Database (IMPORTANTE!)

```powershell
cd src/backend
dotnet ef migrations add AddUserIdToRefreshToken -p PetManager.Infrastructure -s PetManager.Api
dotnet ef database update -p PetManager.Infrastructure -s PetManager.Api
```

---

## 3️⃣ Crie um Usuário de Teste

Abra http://localhost:5000/swagger e execute:

```
POST /api/user/create

{
  "username": "joao_silva",
  "email": "joao@example.com",
  "password": "Senha123!",
  "firstName": "João",
  "lastName": "Silva",
  "role": "Veterinarian"
}
```

---

## 4️⃣ Teste o Login

1. Acesse http://localhost:5173/login
2. Digite: `joao_silva` / `Senha123!`
3. Clique em "Entrar"
4. Você será redirecionado para /dashboard

---

## ✨ Recursos Implementados

- ✅ Login com username + password
- ✅ Sessão de 60 minutos
- ✅ Auto-refresh de token (a cada 10 min antes de expirar)
- ✅ Proteção de rotas
- ✅ Sistema de alertas (4 tipos)
- ✅ Design responsivo
- ✅ TypeScript strict

---

## 📚 Documentação Completa

Se precisar mais detalhes veja:

- `IMPLEMENTATION_STATUS.md` - Status e checklist
- `START_DEV_SCRIPTS.md` - Como usar os scripts
- `IMPLEMENTATION_SUMMARY.md` - Visão técnica
- `ARCHITECTURE.md` - Arquitetura Clean
- `.github/copilot-instructions.md` - Referência técnica

---

## 🆘 Problemas?

**Erro ao fazer login?**

```
❌ Verifique se rodou a migration
❌ Verifique se criou o usuário de teste
❌ Veja se backend está rodando em localhost:5000
```

**Porta já em uso?**

```
Windows: netstat -ano | findstr :5000
Linux:   lsof -i :5000
```

**Scripts não executam?**

```
PowerShell: Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
Bash:       chmod +x start-dev.sh
```

---

🎉 **Pronto! Sistema de login está funcionando!**
