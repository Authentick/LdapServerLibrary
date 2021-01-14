using System.Formats.Asn1;
using System.Numerics;
using Gatekeeper.LdapServerLibrary.PacketParser.Models.Operations.Request;

namespace Gatekeeper.LdapServerLibrary.PacketParser.Decoder
{
    internal class UnbindRequestDecoder : IApplicationDecoder<UnbindRequest>
    {
        public UnbindRequest TryDecode(AsnReader reader, byte[] input)
        {
            return new UnbindRequest();
        }
    }
}