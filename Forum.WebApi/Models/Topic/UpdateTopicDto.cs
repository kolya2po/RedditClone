using System.ComponentModel.DataAnnotations;

namespace Forum.WebApi.Models.Topic
{
    public class UpdateTopicDto
    {
        [Required(ErrorMessage = "Topic's id should be provided.")]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        [Required(ErrorMessage = "Rating should be provided.")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Author's id should be provided.")]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Community's id should be provided.")]
        public int CommunityId { get; set; }

        [Required(ErrorMessage = "Posting date should be provided.")]
        public string PostingDate { get; set; }
    }
}
