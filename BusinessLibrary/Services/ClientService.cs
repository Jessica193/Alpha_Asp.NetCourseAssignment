using BusinessLibrary.Interfaces;
using BusinessLibrary.Models;
using DataLibrary.Interfaces;
using DataLibrary.Repositories;
using DomainLibrary.Extentions;

namespace BusinessLibrary.Services;

public interface IClientService
{
    Task<ClientResult> GetClientsAsync();
}




public class ClientService(IClientRepository clientRepository) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;


    public async Task<ClientResult> GetClientsAsync()
    {
        var result = await _clientRepository.GetAllAsync();
        return result.MapTo<ClientResult>();
    }

}
