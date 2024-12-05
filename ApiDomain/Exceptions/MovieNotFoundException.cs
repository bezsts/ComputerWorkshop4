namespace ApiDomain.Exceptions
{
    public class MovieNotFoundException : Exception
    {
        public MovieNotFoundException()
        {

        }

        public MovieNotFoundException(string message) : base(message)
        {

        }

        public MovieNotFoundException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
