namespace Data.Models
{
    /// <summary>
    /// Represents community's rule.
    /// </summary>
    public class Rule : BaseModel
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
        /// Rule's community id.
        /// </summary>

        public int  CommunityId { get; set; }
    }
}
