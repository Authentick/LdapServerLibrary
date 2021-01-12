namespace Gatekeeper.LdapServerLibrary.Models.Operations.Response
{
    internal class ExtendedOperationResponse : IProtocolOp
    {
        internal readonly string? ResponseName;
        internal readonly string? ResponseValue;
        internal readonly LdapResult LdapResult;

        internal ExtendedOperationResponse(LdapResult ldapResult, string? responseName, string? responseValue)
        {
            LdapResult = ldapResult;
            ResponseName = responseName;
            ResponseValue = responseValue;
        }

        int IProtocolOp.GetTag()
        {
            return 24;
        }
    }
}