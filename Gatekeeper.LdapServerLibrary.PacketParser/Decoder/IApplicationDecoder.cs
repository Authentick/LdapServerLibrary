using System;
using System.Formats.Asn1;
using Gatekeeper.LdapServerLibrary.PacketParser.Models.Operations;

namespace Gatekeeper.LdapServerLibrary.PacketParser.Decoder
{
    internal interface IApplicationDecoder<T> where T : IProtocolOp
    {
        T TryDecode(AsnReader reader, byte[] input);
    }
}
