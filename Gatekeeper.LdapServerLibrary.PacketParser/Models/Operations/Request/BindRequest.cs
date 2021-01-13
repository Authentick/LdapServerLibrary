using System.Formats.Asn1;
using System.Numerics;
using Gatekeeper.LdapServerLibrary.PacketParser.Models.Operations;

namespace Gatekeeper.LdapServerLibrary.PacketParser.Models.Operations.Request
{
    internal class BindRequest : IProtocolOp
    {
        internal const int Tag = 0;
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
    }
}
