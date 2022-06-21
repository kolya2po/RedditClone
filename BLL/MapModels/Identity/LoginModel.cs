namespace Business.MapModels.Identity
{
    /// <summary>
    /// Represents model that encapsulates user's login data.
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// User's email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User's password.
        /// </summary>
        public string Password { get; set; }
    }
}
