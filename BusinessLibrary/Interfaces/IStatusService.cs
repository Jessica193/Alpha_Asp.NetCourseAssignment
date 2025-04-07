using BusinessLibrary.Models;

namespace BusinessLibrary.Interfaces
{
    public interface IStatusService
    {
        Task<StatusResult> GetStatusesAsync();
    }
}