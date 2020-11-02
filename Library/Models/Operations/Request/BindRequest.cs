using System.Numerics;

namespace LdapServer.Models.Operations.Request
{
    class BindRequest : IRequest
    {
        public readonly BigInteger Version;
        public readonly string Name;
        public readonly string Authentication;

        public BindRequest(
            BigInteger version,
            string name,
            string authentication)
        {
            Version = version;
            Name = name;
            Authentication = authentication;
        }
    }
}