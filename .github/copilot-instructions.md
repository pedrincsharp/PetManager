<!-- Copilot instructions for PetManager veterinary clinic management system. Keep short and specific. -->

# Copilot Instructions for PetManager

## Purpose

Help AI agents understand this mono-repo: a React (Vite) + ASP.NET Core backend system for veterinary clinic management.

## Quick Architecture

**Backend** (`src/backend/PetManager.slnx` — 4 projects):

- `PetManager.Api` — HTTP endpoints, middleware, Swagger, JWT/ApiKey auth setup
- `PetManager.Application` — services (UserService, AuthService), DTOs, mappers, business logic
- `PetManager.Domain` — models (`ModelBase` → `Person` → `User`), enums (`Role`, `Status`)
- `PetManager.Infrastructure` — PostgreSQL via EF Core DbContext, Repository pattern, migrations

**Frontend** (`src/frontend/petmanager`):

- React 19 + Typescript, Vite bundler, Tailwind CSS v4, ESLint
- **Clean Architecture**: Presentation → Application → Infrastructure → Core
- **Authentication**: JWT tokens with 60-minute expiration
- **Styling**: Gradient blue-to-emerald theme

**Data flow**: HTTP request → Controller → IService → Domain model (mutation via methods) → IRepository → DB.

## What to Read First

1. **Scope**: [docs/documentation.md](docs/documentation.md) — functional requirements (7 modules: users, clients, pets, services, appointments, payments, dashboard)
2. **Backend Architecture**: [src/backend/PetManager.Api/Program.cs](src/backend/PetManager.Api/Program.cs) — startup, DI, JWT config, Npgsql
3. **Domain model hierarchy**: [src/backend/PetManager.Domain/Models/ModelBase.cs](src/backend/PetManager.Domain/Models/ModelBase.cs) (`Id` = Guid v7, `CreatedAt`, `UpdatedAt`, `Status`)
4. **Entities**: [Person.cs](src/backend/PetManager.Domain/Models/Person.cs), [User.cs](src/backend/PetManager.Domain/Models/User.cs) — mutation via `ChangeName()`, `ChangeRole()`, etc.
5. **Frontend Docs**: [ARCHITECTURE.md](src/frontend/petmanager/ARCHITECTURE.md) — Clean Architecture pattern for UI
6. **Implementation**: [IMPLEMENTATION_SUMMARY.md](IMPLEMENTATION_SUMMARY.md) — complete login system overview

## Build & Run

**Backend**:

```powershell
# Build
dotnet build "src/backend/PetManager.slnx"

# Run (dev)
$env:ASPNETCORE_ENVIRONMENT = 'Development'
dotnet run --project src/backend/PetManager.Api/PetManager.Api.csproj
# Swagger at http://localhost:5000/swagger
```

**Frontend**:

```bash
cd src/frontend/petmanager
npm install react-router-dom # if needed
npm run dev    # http://localhost:5173
npm run build
npm run lint
```

**Database**:

```bash
# Start PostgreSQL (Docker)
docker-compose up -d

# Apply migrations
dotnet ef database update -p src/backend/PetManager.Infrastructure -s src/backend/PetManager.Api
# Or use: src/backend/migrate.ps1 (PowerShell)
```

## Key Patterns

**Domain Models**: Derive from `ModelBase`, mutate state via methods (never property setters):

```csharp
user.ChangeName("New Name");  // Updates UpdatedAt automatically
user.ChangeStatus(Status.Inactive);
```

**Repository + Service pattern**:

- **IUserRepository** (infra): CRUD ops, e.g., `GetByUsernameAsync()`
- **IUserService** (app): business logic, e.g., password validation via BCrypt

**API Response wrapper**: All endpoints return `ApiResponse<T>` (see [Models/ApiResponse.cs](src/backend/PetManager.Api/Models/ApiResponse.cs))

```csharp
ApiResponse<object>.Success("200", "User created", user)
ApiResponse<object>.Error("400", "Username taken", null)
```

**Authentication**: JWT Bearer tokens with Refresh Token support

- Login endpoint: `POST /api/auth/login` (username + password)
- Token endpoint: `POST /api/auth/token` (API key)
- Refresh endpoint: `POST /api/auth/refresh` (refresh token)
- Session timeout: **60 minutes**
- See [AuthService.cs](src/backend/PetManager.Application/Services/AuthService.cs) + [TokenService.cs](src/backend/PetManager.Application/Services/TokenService.cs)

**Frontend Token Management** (Singleton):

- `TokenManager.getInstance()` handles token storage/refresh
- Auto-renewal 10 minutes before expiry
- Callbacks notify when tokens refresh
- See [TokenManager.ts](src/frontend/petmanager/src/core/TokenManager.ts)

**Frontend API Client**: Automatic JWT injection and 401 handling

- `apiClient.get/post/put/delete()` — auto-adds Authorization header
- Detects 401, refreshes token, retries original request
- See [apiClient.ts](src/frontend/petmanager/src/infrastructure/api/apiClient.ts)

**Configuration**: `appsettings.json` (Logging, ConnectionStrings); `appsettings.Development.json` overrides (local Postgres: `docker:docker`)

Frontend: `.env.local` stores `VITE_API_URL` and `VITE_API_KEY`

## When Adding Features

### Backend

1. **New entity**: Create in Domain/Models, derive from `ModelBase` or `Person`
2. **Database**: Add DbSet to [AppDbContext.cs](src/backend/PetManager.Infrastructure/AppDbContext.cs), configure in OnModelCreating
3. **Repository**: Add IXxxRepository, implement in Repositories/, register in Program.cs
4. **Service**: Create IXxxService in Application/Interfaces, implement with business logic
5. **Controller**: Use attribute routing `[Route("api/[controller]")]`, inject IXxxService, return `IActionResult`
6. **Migrations**: `dotnet ef migrations add {Name} -p PetManager.Infrastructure -s PetManager.Api`

### Frontend

1. **New page/feature**: Create in `presentation/pages/` or `presentation/components/`
2. **Infrastructure service**: If calling API, add to `infrastructure/services/`
3. **Application logic**: Add to `application/services/`
4. **DTOs**: Add types in `application/dtos/`
5. **Shared components**: Reusable UI in `presentation/shared/` (e.g., Alert, buttons)
6. **Routing**: Add route in [App.tsx](src/frontend/petmanager/src/App.tsx)
7. **Protected routes**: Wrap with `<ProtectedRoute element={<YourPage />} />`

## Frontend File Structure

```
src/frontend/petmanager/
├── config/              # Environment & constants
├── core/                # Singletons (TokenManager, ErrorConstants)
├── application/         # Business logic (services, DTOs)
├── infrastructure/      # API client, HTTP services
├── presentation/        # React components (pages, shared, components)
└── styles/             # Global CSS
```

## Where Not to Guess

- No tests or CI pipelines configured — ask before adding test frameworks
- Frontend integration points — confirm API contract before implementing UI
- Role-based access control (RBAC) — User has `Role` enum; see [documentation.md](docs/documentation.md) for permission rules per module
- Token refresh behavior — TokenManager handles 60-minute sessions with auto-refresh

## Conventions

- **Naming**: IFooRepository (interface), FooRepository (impl), IFooService, FooService
- **IDs**: Always Guid v7 (auto-generated in ModelBase ctor)
- **Timestamps**: CreatedAt, UpdatedAt (UTC), managed automatically
- **Password**: Never store plaintext — use BCrypt.Net-Next (see UserService.CreateUserAsync)
- **DTOs for API**: See Application/DTO — map via AutoMapper (MappingProfile registered in Program.cs)
- **Frontend Components**: Named exports, PascalCase for React components
- **Frontend Services**: Singleton instances (not classes)

## Important Files by Feature

**Authentication**:

- Backend: [AuthController.cs](src/backend/PetManager.Api/Controllers/AuthController.cs), [AuthService.cs](src/backend/PetManager.Application/Services/AuthService.cs)
- Frontend: [LoginPage.tsx](src/frontend/petmanager/src/presentation/pages/LoginPage.tsx), [TokenManager.ts](src/frontend/petmanager/src/core/TokenManager.ts)

**User Management**:

- Backend: [User.cs](src/backend/PetManager.Domain/Models/User.cs), [UserService.cs](src/backend/PetManager.Application/Services/UserService.cs), [UserController.cs](src/backend/PetManager.Api/Controllers/UserController.cs)

## Quick Reference Docs

- **Full Implementation**: [IMPLEMENTATION_SUMMARY.md](IMPLEMENTATION_SUMMARY.md)
- **Frontend Architecture**: [src/frontend/petmanager/ARCHITECTURE.md](src/frontend/petmanager/ARCHITECTURE.md)
- **Frontend Setup**: [src/frontend/petmanager/SETUP.md](src/frontend/petmanager/SETUP.md)
- **Code Examples**: [src/frontend/petmanager/EXAMPLES.md](src/frontend/petmanager/EXAMPLES.md)
- **Testing Guide**: [LOGIN_TEST_GUIDE.md](LOGIN_TEST_GUIDE.md)
- **Checklist**: [CHECKLIST.md](CHECKLIST.md)
