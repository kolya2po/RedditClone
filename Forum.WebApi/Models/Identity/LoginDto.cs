using System.ComponentModel.DataAnnotations;

namespace Forum.WebApi.Models.Identity
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Please enter email.")]
        [EmailAddress(ErrorMessage = "String should be valid email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
