namespace Gatekeeper.LdapServerLibrary.Models.Operations.Request
{
    internal class ExtendedRequest : IProtocolOp
    {
        int IProtocolOp.GetTag()
        {
            return 23;
        }
    }
}
