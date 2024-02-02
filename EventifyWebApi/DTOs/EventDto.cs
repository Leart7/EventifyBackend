using Swashbuckle.AspNetCore.Annotations;

namespace EventifyWebApi.DTOs
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Organizer { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? City { get; set; }
        public string About { get; set; }
        public double? Price { get; set; }
        public int? Capacity { get; set; }
        public bool EndSales { get; set; }
        public string Category { get; set; }
        public string Currency { get; set; }
        public string Format { get; set; }
        public string Language { get; set; }
        public string Type { get; set; }
        public string UserId { get; set; }
        public UserDto User { get; set; }
        public IEnumerable<string> ImageUrls { get; set; }
        public ICollection<EventAgendDto> EventAgends { get; set; }
        public IEnumerable<string> Tags { get; set; }

    }
}
