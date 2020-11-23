namespace Gatekeeper.LdapServerLibrary.Models.Operations.Response
{
    internal class ExtendedOperationResponse : IProtocolOp
    {
        internal readonly LdapResult LdapResult;

        internal ExtendedOperationResponse(LdapResult ldapResult)
        {
            LdapResult = ldapResult;
        }

        int IProtocolOp.GetTag()
        {
            return 24;
        }
    }
}