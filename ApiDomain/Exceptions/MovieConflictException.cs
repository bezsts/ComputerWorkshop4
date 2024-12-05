namespace ApiDomain.Exceptions
{
    public class MovieConflictException : Exception
    {
        public MovieConflictException()
        {

        }

        public MovieConflictException(string message) : base(message)
        {

        }

        public MovieConflictException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
