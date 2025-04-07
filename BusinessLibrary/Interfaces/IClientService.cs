using BusinessLibrary.Models;

namespace BusinessLibrary.Interfaces
{
    public interface IClientService
    {
        Task<ClientResult> GetClientsAsync();
    }
}