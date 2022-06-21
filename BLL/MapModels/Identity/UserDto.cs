namespace Business.MapModels.Identity
{
    /// <summary>
    /// DTO that encapsulates user's id and token.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// User's id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User's JWT token.
        /// </summary>
        public string Token { get; set; }
    }
}
