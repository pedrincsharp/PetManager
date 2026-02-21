# Setup e Execução - PetManager Frontend

## Pré-requisitos

- Node.js >= 18
- npm ou yarn
- Backend PetManager rodando em `http://localhost:5000`

## Instalação de Dependências

```bash
cd src/frontend/petmanager
npm install
```

## Configuração de Ambiente

1. Copie o arquivo `.env.example` para `.env.local`:

```bash
cp .env.example .env.local
```

2. Edite `.env.local` com suas configurações:

```env
# .env.local
VITE_API_URL=http://localhost:5000/api
VITE_API_KEY=your-api-key-here
```

## Executar em Desenvolvimento

```bash
npm run dev
```

A aplicação estará acessível em `http://localhost:5173`

## Build para Produção

```bash
npm run build
```

Os arquivos serão gerados em `dist/`

## Lint e Verificação de Código

```bash
npm run lint
```

## Fluxo de Autenticação

1. **Tela de Login** (`/login`)
   - Insira um username e password válidos cadastrados no backend
   - Sistema faz POST para `/api/auth/login`

2. **Token Management**
   - TokenManager armazena `accessToken`, `refreshToken` e `expiresAt`
   - Session dura **60 minutos**
   - Token é renovado automaticamente 10 minutos antes de expirar

3. **Acesso Protegido** (`/dashboard`)
   - Apenas usuários autenticados podem acessar
   - Se token expirou, será redirecionado para `/login`

## Estrutura de Pastas

Veja [ARCHITECTURE.md](ARCHITECTURE.md) para detalhes completos sobre a estrutura de Clean Architecture.

## Variáveis de Ambiente Disponíveis

| Variável       | Descrição            | Default                     |
| -------------- | -------------------- | --------------------------- |
| `VITE_API_URL` | URL base da API      | `http://localhost:5000/api` |
| `VITE_API_KEY` | Chave de API inicial | `your-api-key-here`         |

## Erros Comuns

### "Cannot GET /dashboard"

- Certifique-se de que `react-router-dom` está instalado
- Execute `npm install react-router-dom`

### "Failed to fetch from API"

- Verifique se o backend está rodando em `http://localhost:5000`
- Confirme que `VITE_API_URL` está correto em `.env.local`

### "401 Unauthorized"

- Username ou password incorretos
- API Key configurada incorretamente
- Token expirou - faça login novamente

## Desenvolvendo uma Nova Feature

1. Crie a estrutura seguindo Clean Architecture
2. Se for um serviço: `infrastructure/services/*` → `application/services/*`
3. Se for uma página: `presentation/pages/*` + rota em `App.tsx`
4. Use `useAlert()` hook para mensagens
5. Implemente erros seguindo `ErrorConstants.ts`

## Próximas Melhorias

- [ ] Interceptadores automáticos de authorization header
- [ ] Retry automático em falhas de rede
- [ ] Cache inteligente (React Query)
- [ ] Validação de formulários robusta
- [ ] Testes automatizados
- [ ] Progressive Web App (PWA)
