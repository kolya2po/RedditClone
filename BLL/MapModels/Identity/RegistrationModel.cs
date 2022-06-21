using System;

namespace Business.MapModels.Identity
{
    /// <summary>
    /// Represents model that encapsulates user's registration data.
    /// </summary>
    public class RegistrationModel
    {
        /// <summary>
        /// User's email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User's name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// User's password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// User's birth date.
        /// </summary>
        public DateTime BirthDate { get; set; }
    }
}
