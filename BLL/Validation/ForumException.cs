using System;

namespace Business.Validation
{
    /// <inheritdoc/>
    public class ForumException : Exception
    {
        /// <inheritdoc />
        public ForumException(string message) : base(message)
        {
        }
    }
}
