using System.Runtime.Serialization;

namespace QueenGame.Exceptions
{
    internal class GameGeneratorException : Exception
    {
        public GameGeneratorException()
        {
        }

        public GameGeneratorException(string? message) : base(message)
        {
        }

        public GameGeneratorException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected GameGeneratorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
