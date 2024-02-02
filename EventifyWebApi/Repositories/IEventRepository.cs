using EventifyCommon.Models;
using EventifyWebApi.DTOs;

namespace EventifyWebApi.Repositories
{
    public interface IEventRepository
    {
        Task<(List<Event> Events, int TotalPages)> GetAllEvents(string? name = null, string? category = null, string? language = null, string? currency = null, string? format = null, string? city = null, double? bottomLeftLatitude = null, double? bottomLeftLongitude = null, double? topRightLatitude = null, double? topRightLongitude = null, bool? online = false, bool? free = false, bool? paid = false, string? dateFilter = null, int pageNumber = 1);
        Task<(List<Event> Events, int TotalPages)> GetEventsFromFollowings(string userId, int pageNumber = 1);
        Task<(List<Event> Events, int TotalEvents)> GetUpcomingEventsFromUser(string userId);
        Task<(List<Event> Events, int TotalEvents)> GetPastEventsFromUser(string userId);
        Task<int> GetTotalNumberOfEvents(string userId);
        Task<List<Event>> SuggestEvents(string userId, int eventId);
        Task<Event?> DeleteEvent(int id);
        Task<Event?> GetEvent(int id);
        Task<Event> CreateEvent(Event Event, ICollection<IFormFile> images, List<string> tags, List<EventAgend> eventAgends, string userId);  
        Task<Event?> UpdateEvent(int id, UpdateEventRequestDto eventDto);
    }
}
