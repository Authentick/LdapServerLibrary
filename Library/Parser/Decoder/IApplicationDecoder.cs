using System.Formats.Asn1;
using LdapServer.Models.Operations;

namespace LdapServer.Parser.Decoder
{
    internal interface IApplicationDecoder<T> where T : IProtocolOp
    {
        T TryDecode(AsnReader reader);
    }
}
