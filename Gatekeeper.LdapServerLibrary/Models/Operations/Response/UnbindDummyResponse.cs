namespace Gatekeeper.LdapServerLibrary.Models.Operations.Response
{
    internal class UnbindDummyResponse : IProtocolOp
    {
        int IProtocolOp.GetTag()
        {
            return -1;
        }
    }
}