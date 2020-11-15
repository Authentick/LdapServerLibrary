namespace Gatekeeper.LdapServerLibrary.Models.Operations.Response
{
    internal class SearchResultDone : IProtocolOp
    {
        internal readonly LdapResult LdapResult;

        internal SearchResultDone(LdapResult ldapResult)
        {
            LdapResult = ldapResult;
        }

        int IProtocolOp.GetTag()
        {
            return 5;
        }
    }
}