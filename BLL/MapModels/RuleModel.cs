namespace Business.MapModels
{
    public class RuleModel : BaseModel
    {
        public string Title { get; set; }
        public string RuleText { get; set; }
        public int CommunityId { get; set; }
    }
}
