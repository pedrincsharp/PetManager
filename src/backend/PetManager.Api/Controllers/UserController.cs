using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PetManager.Application.DTO;
using PetManager.Application.Interfaces;
using PetManager.Domain.Models;
using PetManager.Api.Models;


namespace PetManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto req)
    {
        if (!ModelState.IsValid)
        {
            var errors = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            var bad = ApiResponse<object>.Error("400", errors, null);
            return BadRequest(bad);
        }

        var user = await _service.CreateUserAsync(req.Name, req.Email, req.Cellphone ?? string.Empty, req.Document, req.Role, req.Username, req.Password);
        var dto = MapToDto(user);
        var resp = ApiResponse<object>.Success("201", "Registro criado com sucesso", dto);
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, resp);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto req)
    {
        var updated = await _service.UpdateUserAsync(id, req.Name, req.Email, req.Cellphone, req.Document, req.Role, req.Username, req.Password, req.OldPassword);
        var dto = MapToDto(updated!);
        var resp = ApiResponse<object>.Success("200", "Registro atualizado com sucesso", dto);
        return Ok(resp);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _service.GetAllUsersAsync();
        var list = users.Select(MapToDto);
        var resp = ApiResponse<object>.Success("200", "Lista retornada com sucesso", list);
        return Ok(resp);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var user = await _service.GetUserAsync(id);
        var dto = MapToDto(user);
        var resp = ApiResponse<object>.Success("200", "Registro retornado com sucesso", dto);
        return Ok(resp);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        await _service.DeleteUserAsync(id);
        var resp = ApiResponse<object>.Success("200", "Registro removido (status=Deleted)", null);
        return Ok(resp);
    }

    [HttpPost("{id}/inactivate")]
    public async Task<IActionResult> InactivateUser(Guid id)
    {
        await _service.InactivateUserAsync(id);
        var resp = ApiResponse<object>.Success("200", "Registro inativado com sucesso", null);
        return Ok(resp);
    }
    private static PetManager.Application.DTO.UserResponseDto MapToDto(PetManager.Domain.Models.User u)
    {
        return new PetManager.Application.DTO.UserResponseDto(
            u.Id,
            u.Name,
            u.Email,
            string.IsNullOrEmpty(u.Cellphone) ? null : u.Cellphone,
            u.Document,
            u.Role,
            u.Username,
            u.CreatedAt,
            u.UpdatedAt,
            u.Status
        );
    }
}
