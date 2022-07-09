using System;
using System.ComponentModel.DataAnnotations;

namespace Forum.WebApi.Models.Identity
{
    public class RegistrationDto
    {
        [Required(ErrorMessage = "Please enter email.")]
        [EmailAddress(ErrorMessage = "String should be valid email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter user name.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter password.")]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "Password should contain minimum 5 characters.")]
        public string Password { get; set; }

        [DataType(DataType.Date)]
        [BirthDateValidation(ErrorMessage = "Please enter valid birth date.")]
        public DateTime? BirthDate { get; set; }
    }
}
