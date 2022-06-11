using System;

namespace Business.Validation
{
    public class ForumException : Exception
    {
        public ForumException(string message) : base(message)
        {
        }
    }
}
