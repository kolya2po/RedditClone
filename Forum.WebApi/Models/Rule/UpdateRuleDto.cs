using System.ComponentModel.DataAnnotations;

namespace Forum.WebApi.Models.Rule
{
    public class UpdateRuleDto
    {
        [Required(ErrorMessage = "Rule's id should be provided.")]
        public int Id { get; set; }
        public string Title { get; set; }
        public string RuleText { get; set; }
    }
}
