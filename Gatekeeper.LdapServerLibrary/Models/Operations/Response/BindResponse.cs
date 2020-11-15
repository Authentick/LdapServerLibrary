namespace Gatekeeper.LdapServerLibrary.Models.Operations.Response
{
    internal class BindResponse : IProtocolOp
    {
        internal readonly LdapResult LdapResult;

        internal BindResponse(LdapResult ldapResult)
        {
            LdapResult = ldapResult;
        }

        int IProtocolOp.GetTag()
        {
            return 1;
        }
    }
}