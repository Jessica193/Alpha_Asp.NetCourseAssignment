using BusinessLibrary.Interfaces;
using BusinessLibrary.Models;
using DataLibrary.Interfaces;
using DomainLibrary.Extentions;

namespace BusinessLibrary.Services;

public interface IStatusService
{
    Task<StatusResult> GetStatusesAsync();
}


public class StatusService(IStatusRepository statusRepository) : IStatusService, IStatusService
{
    private readonly IStatusRepository _statusRepository = statusRepository;

    public async Task<StatusResult> GetStatusesAsync()
    {
        var result = await _statusRepository.GetAllAsync();
        return result.MapTo<StatusResult>();
    }
}
