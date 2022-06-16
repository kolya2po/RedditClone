using System;
using System.ComponentModel.DataAnnotations;

namespace Forum.WebApi.Models.Identity
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = "User's id wasn't provided.")]
        public int Id { get; set; }
        public string UserName { get; set; }

        [DataType(DataType.Date)]
        [BirthDateValidation]
        public DateTime? BirthDate { get; set; }
    }
}
