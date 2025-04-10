using BusinessLibrary.Models;
using DomainLibrary.Models;

namespace BusinessLibrary.Interfaces
{
    public interface IStatusService
    {
        Task<StatusResult<IEnumerable<StatusModel>>> GetAllStatusesAsync();
        Task<StatusResult<StatusModel>> GetOneStatusByIdAsync(int id);
        Task<StatusResult<StatusModel>> GetOneStatusByNameAsync(string statusName);
    }
}