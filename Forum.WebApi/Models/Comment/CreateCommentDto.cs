using System.ComponentModel.DataAnnotations;

namespace Forum.WebApi.Models.Comment
{
    public class CreateCommentDto
    {
        [Required(ErrorMessage = "Please enter text.")]
        public string Text { get; set; }

        [Required(ErrorMessage = "Author's id should be provided.")]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Topic's id should be provided.")]
        public int TopicId { get; set; }
    }
}
