using BusinessLibrary.Models;
using DomainLibrary.Models;

namespace BusinessLibrary.Interfaces
{
    public interface IClientService
    {
        Task<ClientResult> AddClientAsync(AddClientFormData form);
        Task<ClientResult> DeleteClientAsync(int id);
        Task<ClientResult> EditClientAsync(Client model);
        Task<ClientResult<IEnumerable<Client>>> GetAllClientsAsync();
        Task<ClientResult<Client>> GetClientByIdAsync(int id);
    }
}