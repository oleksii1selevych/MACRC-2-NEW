namespace Marc2.Domain.Exceptions
{
    public class InvalidUserCredentialsException : UnauthorizedException
    {
        public InvalidUserCredentialsException() : base("Incorrect user login or password")
        { }
    }
}
