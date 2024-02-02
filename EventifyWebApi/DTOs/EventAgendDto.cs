using System.ComponentModel.DataAnnotations;

namespace EventifyWebApi.DTOs
{
    public class EventAgendDto
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Speaker { get; set; }
    }
}
