# Arquitetura do Frontend - PetManager

Esta aplicação segue os princípios de **Clean Architecture** para manter o código organizado, testável e escalável.

## Estrutura de Pastas

```
src/
├── config/              # Configurações da aplicação
│   └── environment.ts   # Variáveis de ambiente e constantes
├── core/                # Camada de utilidades e singletons
│   ├── TokenManager.ts  # Gerenciador de tokens (singleton)
│   └── ErrorConstants.ts # Constantes de erro HTTP
├── application/         # Camada de Aplicação (lógica de negócio)
│   ├── services/        # Serviços de domínio
│   │   └── authApplicationService.ts
│   └── dtos/            # Data Transfer Objects
│       └── auth.dto.ts
├── infrastructure/      # Camada de Infraestrutura
│   ├── api/
│   │   └── apiClient.ts # Cliente HTTP
│   └── services/
│       └── authService.ts # Integração com API
├── presentation/        # Camada de Apresentação (UI)
│   ├── pages/          # Páginas (rotas)
│   │   ├── LoginPage.tsx
│   │   └── DashboardPage.tsx
│   ├── components/     # Componentes inteligentes
│   │   └── ProtectedRoute.tsx
│   └── shared/         # Componentes reutilizáveis
│       ├── Alert.tsx
│       ├── Alert.css
│       └── useAlert.ts # Hook para gerenciar alertas
└── styles/            # Estilos globais
```

## Camadas Explicadas

### 1. **Config** (Configuração)

Armazena variáveis de ambiente e constantes da aplicação. Centraliza URLs, timeouts e chaves de API.

### 2. **Core** (Núcleo)

Implementa padrões como Singleton (TokenManager) e constantes críticas. Sem dependências de outras camadas.

### 3. **Application** (Aplicação)

Contém a lógica de negócio. Combina infraestrutura com regras de domínio. Exemplo: `authApplicationService` utiliza `authService` da infraestrutura para fazer login e armazena tokens no `TokenManager`.

### 4. **Infrastructure** (Infraestrutura)

Implementa detalhes técnicos: chamadas HTTP, armazenamento, APIs externas. `apiClient` gerencia requisições; `authService` faz chamadas ao backend.

### 5. **Presentation** (Apresentação)

Componentes React puros focados em UI/UX. Integram serviços da `Application` para comportamento. Componentes reutilizáveis em `shared/` (Alert, hooks).

## Fluxo de Dados - Login

```
LoginPage.tsx (UI)
    ↓
authApplicationService.login()
    ↓
authService.login() (HTTP POST /auth/login)
    ↓
TokenManager.setTokens() (Armazena tokens)
    ↓
Redireciona para Dashboard
```

## TokenManager (Singleton)

Gerencia tokens com:

- **Renovação Automática**: Renova 10 minutos antes de expirar
- **Callbacks**: Notifica quando tokens são renovados
- **Persistência**: Salva em localStorage
- **Validação**: Verifica expiração com buffer de 5 minutos

## Tratamento de Erros

O `ApiClient` captura respostas HTTP e:

- **401**: Invalid username/password (erro de alerta)
- **500**: Internal Server Error (erro vermelho)
- **4xx**: Validation errors (alerta amarelo)
- **Network**: Mostra mensagem de conexão

O hook `useAlert` gerencia múltiplas mensagens na tela.

## Como Adicionar Novas Features

### 1. Nova Página de Autenticação (ex: Registro)

```typescript
// 1. Criar RegisterPage.tsx em presentation/pages/
// 2. Adicionar rota em App.tsx
// 3. Chamar authApplicationService.register()
```

### 2. Novo Serviço (ex: Clientes)

```typescript
// infrastructure/services/clientService.ts
// application/services/clientApplicationService.ts
// presentation/pages/ClientsPage.tsx
// application/dtos/client.dto.ts
```

### 3. Novo Componente Reutilizável

```typescript
// presentation/shared/Button.tsx
// export function Button({ ... }) { ... }
```

## Dependências Necessárias

```bash
npm install react-router-dom
```

As demais já estão configuradas en `package.json`.

## Variáveis de Ambiente

Copie `.env.example` para `.env.local` e configure:

```
VITE_API_URL=http://localhost:5000/api
VITE_API_KEY=seu-api-key
```

## Convenções de Código

- **Nomeação de Arquivos**: camelCase para componentes/serviços, PascalCase para exports
- **Tipos**: Use interfaces para DTOs, types para utilitários
- **Imports**: Importe do caminho relativo (compatível com alias no vite.config.ts)
- **Componentes**: Sempre nomeie exports (não use default invisível)
- **Serviços**: Exportam singleton/instância único

## Próximas Implementações

- [ ] Interceptor de requisições para adicionar Authorization header automaticamente
- [ ] Retry automático em caso de falha de rede
- [ ] Cache de requisições GET
- [ ] Formulários com validação usando biblioteca (react-hook-form)
- [ ] Testes unitários (Vitest)
- [ ] Testes de componentes (React Testing Library)
