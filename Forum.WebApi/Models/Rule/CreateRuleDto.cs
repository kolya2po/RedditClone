using System.ComponentModel.DataAnnotations;

namespace Forum.WebApi.Models.Rule
{
    public class CreateRuleDto
    {
        [Required(ErrorMessage = "Please enter title.")]
        public string Title { get; set; }
        public string RuleText { get; set; }

        [Required(ErrorMessage = "Community's id should be provided.")]
        public int CommunityId { get; set; }
    }
}
