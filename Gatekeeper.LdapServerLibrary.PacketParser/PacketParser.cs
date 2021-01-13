using System;
using System.Formats.Asn1;
using System.Numerics;
using System.Runtime.Serialization;
using Gatekeeper.LdapServerLibrary.PacketParser.Models;
using Gatekeeper.LdapServerLibrary.PacketParser.Models.Operations;
using Gatekeeper.LdapServerLibrary.PacketParser.Decoder;
using Gatekeeper.LdapServerLibrary.PacketParser.Models.Operations.Request;

namespace Gatekeeper.LdapServerLibrary.PacketParser
{
    public class PacketParser
    {
        public LdapMessage TryParsePacket(byte[] input)
        {
            AsnReader reader = new AsnReader(input, AsnEncodingRules.BER);
            AsnReader sequenceReader = reader.ReadSequence();
            BigInteger messageId = sequenceReader.ReadInteger();

            TagClass tagClass = sequenceReader.PeekTag().TagClass;
            int tagValue = sequenceReader.PeekTag().TagValue;

            if (tagClass != TagClass.Application)
            {
                throw new ArgumentException("Input type is expected to be " + TagClass.Application + " but was " + tagClass);
            }

            IProtocolOp message = DecodeApplicationData(tagValue, sequenceReader);

            return new LdapMessage(messageId, message);
        }

        private IProtocolOp DecodeApplicationData(int tagValue, AsnReader reader)
        {
            IProtocolOp? result = null;
            switch (tagValue)
            {
                case BindRequest.Tag:
                    BindRequestDecoder bindRequestDecoder = new BindRequestDecoder();
                    result = bindRequestDecoder.TryDecode(reader);
                    break;
                case ExtendedRequest.Tag:
                    ExtendedRequestDecoder extendedRequestDecoder = new ExtendedRequestDecoder();
                    result = extendedRequestDecoder.TryDecode(reader);
                    break;
                case SearchRequest.Tag:
                    SearchRequestDecoder searchRequestDecoder = new SearchRequestDecoder();
                    result = searchRequestDecoder.TryDecode(reader);
                    break;
                case UnbindRequest.Tag:
                    UnbindRequestDecoder unbindRequestDecoder = new UnbindRequestDecoder();
                    result = unbindRequestDecoder.TryDecode(reader);
                    break;
            }

            if (result != null)
            {
                return result;
            }

            throw new NotImplementedException("The decoder for " + tagValue + " is not implemented.");
        }
    }
}
