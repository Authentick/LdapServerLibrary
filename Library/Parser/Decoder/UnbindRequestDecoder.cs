using System.Formats.Asn1;
using System.Numerics;
using LdapServer.Models.Operations.Request;

namespace LdapServer.Parser.Decoder
{
    internal class UnbindRequestDecoder : IApplicationDecoder<UnbindRequest>
    {
        public UnbindRequest TryDecode(AsnReader reader)
        {
            return new UnbindRequest();
        }
    }
}