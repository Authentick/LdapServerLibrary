using System.Formats.Asn1;
using Gatekeeper.LdapServerLibrary.Models.Operations;

namespace Gatekeeper.LdapServerLibrary.Parser.Encoder
{
    internal interface IApplicationEncoder<T> where T : IProtocolOp
    {
        AsnWriter TryEncode(AsnWriter writer, T message);
    }
}
