namespace Marc2.Domain.Exceptions
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(int id)
            : base(String.Format("User with identifier {0} does not exist", id)) { }

        public UserNotFoundException(string email)
            : base(String.Format("User with the specified email address {0} does not exist", email)) { }
    }
}
