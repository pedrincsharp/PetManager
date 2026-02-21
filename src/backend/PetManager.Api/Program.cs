using Microsoft.EntityFrameworkCore;
using PetManager.Infrastructure;
using PetManager.Infrastructure.Repositories;
using PetManager.Application.Interfaces;
using PetManager.Application.Services;
using PetManager.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Adiciona suporte a Controllers
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register AppDbContext (PostgreSQL). Ensure Npgsql EF Core provider is installed:
// dotnet add src/backend/PetManager.Infrastructure/PetManager.Infrastructure.csproj package Npgsql.EntityFrameworkCore.PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrEmpty(connectionString))
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(connectionString));
}

// Register application services and repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Swagger apenas em desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ApiExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();