using EventifyCommon.Models;

namespace EventifyWebApi.DTOs
{
    public class LikeDto
    {
        public int Id { get; set; }
        public EventDto Event { get; set; }
    }
}
