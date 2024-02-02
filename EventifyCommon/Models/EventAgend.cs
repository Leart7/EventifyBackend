using EventifyCommon.Models.AbstractModels;

namespace EventifyCommon.Models
{
    public class EventAgend : BaseModel
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Speaker { get; set; }
        public int EventId { get; set; }
    }
}
