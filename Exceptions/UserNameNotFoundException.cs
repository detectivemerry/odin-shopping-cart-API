namespace OdinShopping.Exceptions
{
    public class UserNameNotFoundException : OdinShoppingException
    {
        public UserNameNotFoundException(string username)
    : base($"Username {username} was not found.")
        {
        }
    }
}
