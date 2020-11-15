using System.Formats.Asn1;
using System.Numerics;

namespace Gatekeeper.LdapServerLibrary.Models.Operations.Request
{
    internal class BindRequest : IProtocolOp
    {
        internal readonly BigInteger Version;
        internal readonly string Name;
        internal readonly string Authentication;

        internal BindRequest(
            BigInteger version,
            string name,
            string authentication)
        {
            Version = version;
            Name = name;
            Authentication = authentication;
        }

        int IProtocolOp.GetTag()
        {
            return 0;
        }
    }
}
