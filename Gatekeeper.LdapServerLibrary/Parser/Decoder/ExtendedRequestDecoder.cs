using System.Formats.Asn1;
using Gatekeeper.LdapServerLibrary.Models.Operations.Request;

namespace Gatekeeper.LdapServerLibrary.Parser.Decoder
{
    internal class ExtendedRequestDecoder : IApplicationDecoder<ExtendedRequest>
    {
        public ExtendedRequest TryDecode(AsnReader reader)
        {
            return new ExtendedRequest();
        }
    }
}
