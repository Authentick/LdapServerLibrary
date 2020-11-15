namespace Gatekeeper.LdapServerLibrary.Session.Events
{
    public class AuthenticationEvent
    {
        public readonly string Username;
        public readonly string Password;

        public AuthenticationEvent(
            string username,
            string password
            )
        {
            Username = username;
            Password = password;
        }
    }
}