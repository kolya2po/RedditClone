namespace Business.MapModels
{
    /// <summary>
    /// Represents Rule mapped model.
    /// </summary>
    public class RuleModel : BaseModel
    {
        /// <summary>
        /// Rule's title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Rule's text.
        /// </summary>
        public string RuleText { get; set; }

        /// <summary>
        /// Rule's community's id.
        /// </summary>
        public int CommunityId { get; set; }
    }
}
