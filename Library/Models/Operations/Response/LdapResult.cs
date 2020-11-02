namespace LdapServer.Models.Operations.Response
{
    internal class LdapResult
    {
        internal readonly ResultCodeEnum ResultCode;
        internal readonly string? MatchedDN;
        internal readonly string? ErrorMessage;

        internal LdapResult(
            ResultCodeEnum resultCode,
            string? matchedDn,
            string? errorMessage
            )
        {
            ResultCode = resultCode;
            MatchedDN = matchedDn;
            ErrorMessage = errorMessage;
        }

        internal enum ResultCodeEnum
        {
            Success = 1,
            InappropriateAuthentication = 48,
            InvalidCredentials = 49,
        }
    }
}