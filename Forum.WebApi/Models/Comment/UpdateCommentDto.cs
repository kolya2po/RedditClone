using System.ComponentModel.DataAnnotations;

namespace Forum.WebApi.Models.Comment
{
    public class UpdateCommentDto
    {
        [Required(ErrorMessage = "Id should be provided.")]
        public int Id { get; set; }
        public string Text { get; set; }
    }
}
