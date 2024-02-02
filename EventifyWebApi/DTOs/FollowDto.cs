using EventifyCommon.Models;

namespace EventifyWebApi.DTOs
{
    public class FollowDto
    {
        public int Id { get; set; }
        public string FollowedUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }
        public int FollowersCount { get; set; }
    }
}
