namespace Marc2.Domain.Exceptions
{
    public class UserEmailExistsExeption : BadRequestException
    {
        public UserEmailExistsExeption(string email)
            : base(String.Format("User with the specified email {0} already exists", email)) { }
    }
}
