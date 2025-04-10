using BusinessLibrary.Interfaces;
using BusinessLibrary.Models;
using DomainLibrary.Models;
using System.Diagnostics;

namespace WebbApp.ViewModels;

public class ClientsViewModel
{
    private readonly IClientService _clientService;

    public ClientsViewModel(IClientService clientService)
    {
        _clientService = clientService;
    }

    public IEnumerable<Client> Clients { get; set; } = [];
    public EditClientViewModel EditClientForm { get; set; } = new EditClientViewModel();
    public AddClientViewmodel AddClientForm { get; set; } = new AddClientViewmodel();


    public async Task<IEnumerable<Client>> PopulateClientsAsync()
    {
        var result = await _clientService.GetAllClientsAsync();
        if (result.Succeeded)
        {
            if (result.Result == null) return [];

            Clients = result.Result.ToList();
        }

        return []; 
    }
}
