namespace LdapServer.Models.Operations.Response
{
    internal class SearchResultEntry : IProtocolOp
    {
        int IProtocolOp.GetTag()
        {
            return 4;
        }
    }
}