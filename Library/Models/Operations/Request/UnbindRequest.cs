namespace LdapServer.Models.Operations.Request
{
    internal class UnbindRequest : IProtocolOp
    {
        int IProtocolOp.GetTag()
        {
            return 2;
        }
    }
}
