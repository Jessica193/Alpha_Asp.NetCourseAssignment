using BusinessLibrary.Interfaces;
using BusinessLibrary.Models;
using DataLibrary.Interfaces;
using DomainLibrary.Extentions;
using DomainLibrary.Models;

namespace BusinessLibrary.Services;

public interface IStatusService
{
    Task<StatusResult<StatusModel>> GetOneStatusByIdAsync(int id);
    Task<StatusResult<StatusModel>> GetOneStatusByNameAsync(string statusName);
    Task<StatusResult<IEnumerable<StatusModel>>> GetAllStatusesAsync();
}

public class StatusService(IStatusRepository statusRepository) : IStatusService
{
    private readonly IStatusRepository _statusRepository = statusRepository;

    public async Task<StatusResult<IEnumerable<StatusModel>>> GetAllStatusesAsync()
    {
        var result = await _statusRepository.GetAllAsync();
        return result.Succeeded
            ? new StatusResult<IEnumerable<StatusModel>> { Succeeded = true, StatusCode = 200, Result = result.Result }
            : new StatusResult<IEnumerable<StatusModel>> { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };


    }


    public async Task<StatusResult<StatusModel>> GetOneStatusByNameAsync(string statusName)
    {
        var result = await _statusRepository.GetOneAsync(x => x.Status == statusName);
        return result.Succeeded
              ? new StatusResult<StatusModel> { Succeeded = true, StatusCode = 200, Result = result.Result }
              : new StatusResult<StatusModel> { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }


    public async Task<StatusResult<StatusModel>> GetOneStatusByIdAsync(int id)
    {
        var result = await _statusRepository.GetOneAsync(x => x.Id == id);
        return result.Succeeded
           ? new StatusResult<StatusModel> { Succeeded = true, StatusCode = 200, Result = result.Result }
           : new StatusResult<StatusModel> { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }
}
