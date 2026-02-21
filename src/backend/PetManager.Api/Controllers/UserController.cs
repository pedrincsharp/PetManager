using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PetManager.Application.DTO;
using PetManager.Application.Interfaces;
using PetManager.Api.Models;
using Microsoft.AspNetCore.Authorization;


namespace PetManager.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;
    private readonly PetManager.Application.Interfaces.IUserMapper _mapper;

    public UserController(IUserService service, PetManager.Application.Interfaces.IUserMapper mapper)
    {
        _service = service;
        _mapper = mapper;
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
        var dto = _mapper.ToDto(user);
        var resp = ApiResponse<object>.Success("201", "Registro criado com sucesso", dto);
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, resp);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto req)
    {
        var updated = await _service.UpdateUserAsync(id, req.Name, req.Email, req.Cellphone, req.Document, req.Role, req.Username, req.Password, req.OldPassword);
        var dto = _mapper.ToDto(updated!);
        var resp = ApiResponse<object>.Success("200", "Registro atualizado com sucesso", dto);
        return Ok(resp);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _service.GetAllUsersAsync();
        var list = _mapper.ToDto(users);
        var resp = ApiResponse<object>.Success("200", "Lista retornada com sucesso", list);
        return Ok(resp);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var user = await _service.GetUserAsync(id);
        var dto = _mapper.ToDto(user);
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

    
}
