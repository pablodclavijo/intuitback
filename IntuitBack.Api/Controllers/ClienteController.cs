using Microsoft.AspNetCore.Mvc;
using Mapster;
using IntuitBack.Application.Interfaces;
using IntuitBack.Domain.Entities;
using IntuitBack.IntuitBack.Application.DTOs.Cliente;

namespace IntuitBack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clienteService;
    private readonly ILogService _logService;

    public ClientesController(IClienteService clienteService, ILogService logService)
    {
        _clienteService = clienteService;
        _logService = logService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var clientes = await _clienteService.GetAllAsync();
        var clientesDto = clientes.Adapt<IEnumerable<GetClienteDto>>();

        await _logService.RegistrarAsync("Info", "Consulta de todos los clientes");
        return Ok(clientesDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var cliente = await _clienteService.GetByIdAsync(id);
        if (cliente == null)
        {
            await _logService.RegistrarAsync("Warning", $"Cliente no encontrado (ID={id})");
            return NotFound();
        }

        var clienteDto = cliente.Adapt<GetClienteDto>();
        await _logService.RegistrarAsync("Info", $"Consulta de cliente (ID={id})");
        return Ok(clienteDto);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string nombre)
    {
        var clientes = await _clienteService.SearchAsync(nombre);
        var clientesDto = clientes.Adapt<IEnumerable<GetClienteDto>>();

        await _logService.RegistrarAsync("Info", $"Búsqueda de clientes por nombre '{nombre}'");
        return Ok(clientesDto);
    }

    [HttpPost]
    public async Task<IActionResult> Insert([FromBody] CreateClienteDto clienteDto)
    {
        if (!ModelState.IsValid)
        {
            await _logService.RegistrarAsync("Warning", "Intento de inserción con datos inválidos");
            return BadRequest(ModelState);
        }

        var cliente = clienteDto.Adapt<Cliente>();
        var creado = await _clienteService.InsertAsync(cliente);
        var creadoDto = creado.Adapt<GetClienteDto>();

        await _logService.RegistrarAsync("Info", $"Cliente creado correctamente (ID={creado.Id})");
        return CreatedAtAction(nameof(Get), new { id = creado.Id }, creadoDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateClienteDto clienteDto)
    {
        var clienteExiste = await _clienteService.GetByIdAsync(id) is not null;

        if (!clienteExiste)
        {
            await _logService.RegistrarAsync("Error", $"Se intentó actualizar un cliente inexistente (ID = {id})");
            return NotFound();
        }

        var cliente = clienteDto.Adapt<Cliente>();
        var actualizado = await _clienteService.UpdateAsync(cliente);
        if (!actualizado)
        {
            await _logService.RegistrarAsync("Warning", $"Ocurrió un error al intentar actualizar cliente (ID = {id})");
            return BadRequest();
        }

        await _logService.RegistrarAsync("Info", $"Cliente actualizado correctamente (ID={id})");
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var eliminado = await _clienteService.DeleteAsync(id);
        if (!eliminado)
        {
            await _logService.RegistrarAsync("Warning", $"Intento de eliminación sobre cliente inexistente (ID={id})");
            return NotFound();
        }

        await _logService.RegistrarAsync("Info", $"Cliente eliminado correctamente (ID={id})");
        return NoContent();
    }
}
