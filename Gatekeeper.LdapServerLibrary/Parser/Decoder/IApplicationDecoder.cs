using System;
using System.Formats.Asn1;
using Gatekeeper.LdapServerLibrary.Models.Operations;

namespace Gatekeeper.LdapServerLibrary.Parser.Decoder
{
    internal interface IApplicationDecoder<T> where T : IProtocolOp
    {
        T TryDecode(AsnReader reader);
    }
}
