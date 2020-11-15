using System.Formats.Asn1;
using System.Numerics;
using Gatekeeper.LdapServerLibrary.Models.Operations.Request;

namespace Gatekeeper.LdapServerLibrary.Parser.Decoder
{
    internal class UnbindRequestDecoder : IApplicationDecoder<UnbindRequest>
    {
        public UnbindRequest TryDecode(AsnReader reader)
        {
            return new UnbindRequest();
        }
    }
}