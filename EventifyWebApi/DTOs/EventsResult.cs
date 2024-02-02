using EventifyCommon.Models;

namespace EventifyWebApi.DTOs
{
    public class EventsResult
    {
        public List<Event> ResultEvents { get; set; }
        public int TotalPages { get; set; }
    }
}
