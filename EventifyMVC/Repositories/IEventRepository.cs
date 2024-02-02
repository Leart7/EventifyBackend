using EventifyCommon.Models;

namespace EventifyMVC.Repositories
{
    public interface IEventRepository
    {
        Task<(List<Event> Events, int TotalPages, int TotalResults)> GetAll(string? title = null, string? category = null, string? language = null, string? format = null, string? currency = null, string? type = null, string? orderBy = "date_soonest_first", string? status = null, string? pastEvents = "false", string? online = null, int pageNumber = 1);
        Task<Event?> FindEvent(int id);
        Task UpdateEventStatus(int id, string newStatus);
    }
}
