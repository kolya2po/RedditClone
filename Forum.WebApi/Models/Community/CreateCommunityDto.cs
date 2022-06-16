using System.ComponentModel.DataAnnotations;

namespace Forum.WebApi.Models.Community
{
    public class CreateCommunityDto
    {
        [Required(ErrorMessage = "Please enter title.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter information about community.")]
        public string About { get; set; }

        [Required(ErrorMessage = "Creator's id should be provided.")]
        public int CreatorId { get; set; }
    }
}
