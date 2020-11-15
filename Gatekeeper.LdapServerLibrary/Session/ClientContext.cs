namespace Gatekeeper.LdapServerLibrary {
    public class ClientContext {
        public bool IsAuthenticated { get; set; }
        public string? UserId { get; set; }
    }
}