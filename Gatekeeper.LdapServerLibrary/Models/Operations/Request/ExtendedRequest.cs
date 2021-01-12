namespace Gatekeeper.LdapServerLibrary.Models.Operations.Request
{
    internal class ExtendedRequest : IProtocolOp
    {
        public string RequestName { get; set; } = null!;
        public string? RequestValue { get; set; }

        int IProtocolOp.GetTag()
        {
            return 23;
        }
    }
}
