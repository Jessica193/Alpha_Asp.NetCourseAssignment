using BusinessLibrary.Interfaces;
using BusinessLibrary.Models;
using DataLibrary.Entities;
using DataLibrary.Interfaces;
using DataLibrary.Models;
using DataLibrary.Repositories;
using DomainLibrary.Extentions;
using DomainLibrary.Models;
using System.Diagnostics;
using System.IO.Pipelines;

namespace BusinessLibrary.Services;

public class ClientService(IClientRepository clientRepository) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;



    public async Task<ClientResult> AddClientAsync(AddClientFormData form)
    {
        if (form == null)
            return new ClientResult { Succeeded = false, StatusCode = 400, Error = "Form data is null" };

        var existResult = await _clientRepository.ExistsAsync(x => x.ClientName == form.ClientName);
        if (existResult.Succeeded)
            return new ClientResult { Succeeded = false, StatusCode = 400, Error = "Client already exists" };

        try
        {
            var clientEntity = form.MapTo<ClientEntity>();
            var result = await _clientRepository.CreateAsync(clientEntity);

            return result.Succeeded
            ? new ClientResult { Succeeded = true, StatusCode = 200 }
            : new ClientResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new ClientResult { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }

    public async Task<ClientResult<IEnumerable<Client>>> GetAllClientsAsync()
    {
        var result = await _clientRepository.GetAllAsync();
        return result.MapTo<ClientResult<IEnumerable<Client>>>();
    }

    public async Task<ClientResult<Client>> GetClientByIdAsync(int id)
    {
        var result = await _clientRepository.GetOneAsync(x => x.Id == id);
       
        return new ClientResult<Client>
        {
            Succeeded = result.Succeeded,
            StatusCode = result.StatusCode,
            Error = result.Error,
            Result = result.Result
        };   
    }

    public async Task<ClientResult> EditClientAsync(Client model)
    {
        try
        {
            // Hämta befintlig entity (trackad av EF)
            var entityResult = await _clientRepository.GetOneEntityAsync(x => x.Id == model.Id);
            if (!entityResult.Succeeded || entityResult.Result == null)
                return new ClientResult { Succeeded = false, StatusCode = 404, Error = "Client not found" };

            var entity = entityResult.Result;

            // Uppdatera fälten direkt på trackad entity
            entity.ClientName = model.ClientName;
            entity.Email = model.Email;
            entity.Location = model.Location;
            entity.Phone = model.Phone;
            if (!string.IsNullOrEmpty(model.ImagePath))
                entity.ImagePath = model.ImagePath;

            var updateResult = await _clientRepository.UpdateAsync(entity);

            return new ClientResult
            {
                Succeeded = updateResult.Succeeded,
                StatusCode = updateResult.StatusCode,
                Error = updateResult.Error
            };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new ClientResult { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }




}
