using System.ComponentModel.DataAnnotations;

namespace Forum.WebApi.Models.Topic
{
    public class CreateTopicDto
    {
        [Required(ErrorMessage = "Please enter title.")]
        public string Title { get; set; }
        public string Text { get; set; }

        [Required(ErrorMessage = "Author's id should be provided.")]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Community's id should be provided.")]
        public int CommunityId { get; set; }
    }
}
