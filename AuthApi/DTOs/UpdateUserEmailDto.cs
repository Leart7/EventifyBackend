using System.ComponentModel.DataAnnotations;

namespace AuthApi.DTOs
{
    public class UpdateUserEmailDto
    {
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
