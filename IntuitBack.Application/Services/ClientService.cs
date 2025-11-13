using IntuitBack.Application.Interfaces;
using IntuitBack.Domain.Entities;
using IntuitBack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IntuitBack.Application.Services;

public class ClienteService : IClienteService
{
    private readonly AppDbContext _context;
    public ClienteService(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Cliente>> GetAllAsync() =>
        await _context.Clientes.Where(e => e.Eliminado == false).AsNoTracking().ToListAsync();

    public async Task<Cliente?> GetByIdAsync(int id) =>
        await _context.Clientes.Where(e => e.Id == id && e.Eliminado == false).FirstOrDefaultAsync();

    public async Task<IEnumerable<Cliente>> SearchAsync(string term)
    {
        if (string.IsNullOrWhiteSpace(term))
            return Enumerable.Empty<Cliente>();

        term = term.Trim().ToLower();

        var parts = term.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 1 || parts.Length > 4)
            return Enumerable.Empty<Cliente>();

        var query = _context.Clientes
            .Where(c => !c.Eliminado)
            .Select(c => new
            {
                Cliente = c,
                Nombre = c.Nombres.ToLower(),
                Apellido = c.Apellidos.ToLower()
            });

        switch (parts.Length)
        {
            case 1:
                query = query.Where(c =>
                    EF.Functions.Like(c.Nombre, $"%{parts[0]}%") ||
                    EF.Functions.Like(c.Apellido, $"%{parts[0]}%"));
                break;

            case 2:
                query = query.Where(c =>
                    (EF.Functions.Like(c.Nombre, $"%{parts[0]}%") &&
                     EF.Functions.Like(c.Apellido, $"%{parts[1]}%")) ||
                    (EF.Functions.Like(c.Nombre, $"%{parts[1]}%") &&
                     EF.Functions.Like(c.Apellido, $"%{parts[0]}%")) ||
                    EF.Functions.Like(c.Nombre + " " + c.Apellido, $"%{parts[0]}%{parts[1]}%"));
                break;

            case 3:
                query = query.Where(c =>
                    EF.Functions.Like(c.Nombre + " " + c.Apellido, $"%{string.Join(" ", parts)}%"));
                break;

            case 4:
                query = query.Where(c =>
                    EF.Functions.Like(c.Nombre + " " + c.Apellido, $"%{string.Join(" ", parts)}%"));
                break;
        }

        return await query
            .Select(c => c.Cliente)
            .ToListAsync();
    }


    public async Task<Cliente> InsertAsync(Cliente cliente)
    {
        cliente.FechaCreacion = DateTime.Now;
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        return cliente;
    }

    public async Task<bool> UpdateAsync(Cliente cliente)
    {
        var existing = await _context.Clientes.FindAsync(cliente.Id);
        if (existing is null) return false;

        cliente.FechaModificacion = DateTime.Now;
        cliente.FechaCreacion = existing.FechaCreacion;
        cliente.Eliminado = existing.Eliminado;

        _context.Entry(existing).CurrentValues.SetValues(cliente);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null) return false;

        cliente.Eliminado = true;

        _context.Update(cliente);
        int entidadesActualizadas = await _context.SaveChangesAsync();

        return entidadesActualizadas == 1;
    }
}
