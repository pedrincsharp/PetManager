using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PetManager.Application.DTO;
using PetManager.Application.Interfaces;
using PetManager.Api.Models;

namespace PetManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto req)
    {
        if (!ModelState.IsValid)
        {
            var errors = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            var bad = ApiResponse<object>.Error("400", errors, null);
            return BadRequest(bad);
        }

        try
        {
            var tokenResponse = await _authService.LoginAsync(req.Username, req.Password);
            var resp = ApiResponse<object>.Success("200", "Login realizado com sucesso", tokenResponse);
            return Ok(resp);
        }
        catch (InvalidOperationException ex)
        {
            var errorResp = ApiResponse<object>.Error("401", ex.Message, null);
            return Unauthorized(errorResp);
        }
    }

    [AllowAnonymous]
    [HttpPost("token")]
    public async Task<IActionResult> GetToken([FromBody] ApiKeyTokenRequestDto req)
    {
        if (!ModelState.IsValid)
        {
            var errors = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            var bad = ApiResponse<object>.Error("400", errors, null);
            return BadRequest(bad);
        }

        try
        {
            var tokenResponse = await _authService.GenerateTokensByApiKeyAsync(req.ApiKey);
            var resp = ApiResponse<object>.Success("200", "Token gerado com sucesso", tokenResponse);
            return Ok(resp);
        }
        catch (InvalidOperationException ex)
        {
            var errorResp = ApiResponse<object>.Error("401", ex.Message, null);
            return Unauthorized(errorResp);
        }
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto req)
    {
        if (!ModelState.IsValid)
        {
            var errors = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            var bad = ApiResponse<object>.Error("400", errors, null);
            return BadRequest(bad);
        }

        try
        {
            var tokenResponse = await _authService.RefreshAccessTokenAsync(req.RefreshToken);
            var resp = ApiResponse<object>.Success("200", "Token atualizado com sucesso", tokenResponse);
            return Ok(resp);
        }
        catch (InvalidOperationException ex)
        {
            var errorResp = ApiResponse<object>.Error("401", ex.Message, null);
            return Unauthorized(errorResp);
        }
    }
}
