using EventifyCommon.Models;

namespace EventifyWebApi.DTOs
{
    public class ReportEventDto
    {
        public string ReportEventReason { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public EventDto Event { get; set; }

    }
}
