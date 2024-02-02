namespace AuthApi.DTOs
{
    public class UpdatedUserResponseDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageUrl { get; set; }
    }
}
