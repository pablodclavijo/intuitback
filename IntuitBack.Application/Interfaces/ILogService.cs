using IntuitBack.Domain.Entities;

namespace IntuitBack.Application.Interfaces
{
    public interface ILogService
    {
        Task RegistrarAsync(string nivel, string mensaje, string? detalle = null);
        Task<IEnumerable<Log>> ObtenerUltimosAsync(int cantidad = 100);
    }
}
