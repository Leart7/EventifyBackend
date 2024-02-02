using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventifyCommon.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Bio { get; set; }
        public string? Organization { get; set; }
        public string? Website { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Zip { get; set; }
        public string? OrganizationAddress { get; set; }
        public string? OrganizationCity { get; set; }
        public string? OrganizationZip { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime Created_at { get; set; } = DateTime.Now;
        public DateTime Updated_at { get; set; } = DateTime.Now;
        [NotMapped]
        public IFormFile? Image { get; set; }

        public virtual ICollection<Follow> FollowerFollows { get; set; }
        public virtual ICollection<Follow> FollowedUserFollows { get; set; }
        public virtual ICollection<Event> Events { get; set; }
    }
}
