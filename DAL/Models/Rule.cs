namespace Data.Models
{
    public class Rule : BaseModel
    {
        public string Title { get; set; }
        public string RuleText { get; set; }

        public int  CommunityId { get; set; }
        public Community Community { get; set; }
    }
}
