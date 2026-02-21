<!-- Copied to repo to help AI coding agents. Keep short and specific to this project. -->

# Copilot instructions for PetManager

Purpose

- Help AI agents be productive in this mono-repo containing a small ASP.NET backend solution and docs.

Quick architecture

- Solution: `src/backend/PetManager.slnx` contains 4 projects:
  - `PetManager.Api` — minimal Web API (controllers + Program.cs)
  - `PetManager.Application` — application/service layer
  - `PetManager.Domain` — domain models and enums (see `Domain/Models`)
  - `PetManager.Infrastructure` — EF Core `AppDbContext` and infra
- Data flow: Controllers -> Application services -> Domain models -> Infrastructure (DbContext).

What to look at first

- `src/backend/PetManager.Api/Program.cs` — app startup, middleware, Swagger (dev only).
- `src/backend/PetManager.Infrastructure/AppDbContext.cs` — EF Core DbContext; register it in Program.cs when adding DB.
- `src/backend/PetManager.Domain/Models` — `ModelBase`, `Person`, `User`: conventions for IDs, timestamps and `Status`.
- `readme.md` and `docs/documentation.md` — product requirements and high-level scope.

Build & run (local)

- Build solution:

  dotnet build "src/backend/PetManager.slnx"

- Run API (development):

  # Powershell

  $env:ASPNETCORE_ENVIRONMENT = 'Development'
  dotnet run --project src/backend/PetManager.Api/PetManager.Api.csproj

Key project-specific conventions

- Domain entities derive from `ModelBase` and manage timestamps/status internally. Use provided methods (e.g. `UpdateTimestamps()`).
- Minimal Program.cs: DI registrations are manual — add services and DbContext here.
- Controllers use attribute routing (e.g. `api/[controller]`) and return `IActionResult`.
- No authentication is configured yet, but `UseAuthorization()` is present — add auth schemes before that middleware.

Database & Migrations (notes)

- No connection string or DbContext registration is present. When adding DB support:
  - Add `Microsoft.EntityFrameworkCore` provider package (Postgres/SqlServer).
  - Register `AppDbContext` in `Program.cs` via `builder.Services.AddDbContext<AppDbContext>(...)`.
  - Create migrations using `dotnet ef` CLI, example:

    dotnet ef migrations add Initial -p src/backend/PetManager.Infrastructure -s src/backend/PetManager.Api

Patterns to preserve

- Keep business logic in `PetManager.Application` / `PetManager.Domain` — controllers should be thin.
- Use value-object-like methods on domain models to mutate state (e.g., `ChangeName`, `ChangeStatus`).

Where not to guess

- There are no tests or CI configs in the repo — do not assume test frameworks or pipelines.

When you modify startup or DB wiring

- Update `src/backend/PetManager.Api/Program.cs` only. Keep `Program.cs` minimal and register services there.

If anything is unclear

- Ask for the preferred DB provider, authentication approach, or where to put new integration tests.

Keep this file short — prefer adding small, targeted updates with examples as repository patterns evolve.
