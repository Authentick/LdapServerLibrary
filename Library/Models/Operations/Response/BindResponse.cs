namespace LdapServer.Models.Operations.Response
{
    internal class BindResponse : IResponse
    {
        internal readonly LdapResult LdapResult;

        internal BindResponse(LdapResult ldapResult)
        {
            LdapResult = ldapResult;
        }
    }
}