using IntuitBack.Application.Interfaces;
using IntuitBack.Domain.Entities;
using IntuitBack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IntuitBack.Application.Services;

public class LogService : ILogService
{
    private readonly AppDbContext _context;
    public LogService(AppDbContext context) => _context = context;

    public async Task RegistrarAsync(string nivel, string mensaje, string? detalle = null)
    {
        var log = new Log
        {
            Nivel = nivel,
            Mensaje = mensaje,
            Detalle = detalle,
            FechaCreacion = DateTime.UtcNow
        };
        _context.Logs.Add(log);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Log>> ObtenerUltimosAsync(int cantidad = 100) =>
        await _context.Logs
            .OrderByDescending(l => l.FechaCreacion)
            .Take(cantidad)
            .ToListAsync();
}
