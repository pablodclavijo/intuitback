using IntuitBack.Domain.Entities;

namespace IntuitBack.Application.Interfaces;

public interface IClienteService
{
    Task<IEnumerable<Cliente>> GetAllAsync();
    Task<Cliente?> GetByIdAsync(int id);
    Task<IEnumerable<Cliente>> SearchAsync(string term);
    Task<Cliente> InsertAsync(Cliente cliente);
    Task<bool> UpdateAsync(Cliente cliente);
    Task<bool> DeleteAsync(int id);

}
