using System.Formats.Asn1;
using LdapServer.Models.Operations;

namespace LdapServer.Parser.Encoder
{
    internal interface IApplicationEncoder<T> where T : IProtocolOp
    {
        AsnWriter TryEncode(AsnWriter writer, T message);
    }
}
