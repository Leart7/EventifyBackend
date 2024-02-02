using System.ComponentModel.DataAnnotations;

namespace AuthApi.DTOs
{
    public class UpdateUserRequestDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Organization { get; set; }
        public string? Website { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Zip { get; set; }
        public string? OrganizationAddress { get; set; }
        public string? OrganizationCity { get; set; }
        public string? OrganizationZip { get; set; }
        public IFormFile? Image { get; set; }
    }
}
