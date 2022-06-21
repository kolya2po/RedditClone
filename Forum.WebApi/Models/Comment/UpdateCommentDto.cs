using System.ComponentModel.DataAnnotations;

namespace Forum.WebApi.Models.Comment
{
    public class UpdateCommentDto
    {
        [Required(ErrorMessage = "Id should be provided.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Author's id should be provided.")]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Topic's id should be provided.")]
        public int TopicId { get; set; }
        public string Text { get; set; }
        public string PostingDate { get; set; }
    }
}
