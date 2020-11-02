namespace LdapServer.Models.Operations.Response
{
    internal class BindResponse : IProtocolOp
    {
        internal readonly LdapResult LdapResult;

        internal BindResponse(LdapResult ldapResult)
        {
            LdapResult = ldapResult;
        }

        internal override int GetTag()
        {
            return 1;
        }
    }
}