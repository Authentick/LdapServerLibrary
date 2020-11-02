using System;
using System.Formats.Asn1;
using System.Numerics;
using LdapServer.Models;
using LdapServer.Models.Operations;
using LdapServer.Parser.Decoder;

namespace LdapServer.Parser
{
    internal class PacketParser
    {
        internal LdapMessage TryParsePacket(byte[] input)
        {
            AsnReader reader = new AsnReader(input, AsnEncodingRules.BER);
            AsnReader sequenceReader = reader.ReadSequence();
            BigInteger messageId = sequenceReader.ReadInteger();

            TagClass tagClass = sequenceReader.PeekTag().TagClass;
            int tagValue = sequenceReader.PeekTag().TagValue;

            if(tagClass != TagClass.Application) {
                throw new ArgumentException("Input type is expected to be " + TagClass.Application + " but was " + tagClass);
            }

            IProtocolOp message = DecodeApplicationData(tagValue, sequenceReader);

            return new LdapMessage(messageId, message);            
        }

        private IProtocolOp DecodeApplicationData(int tagValue, AsnReader reader) {
            switch(tagValue) {
                case 0:
                    BindRequestDecoder decoder = new BindRequestDecoder();
                    return decoder.TryDecode(reader);
                default:
                    throw new NotImplementedException("The decoder for " + tagValue + " is not implemented.");
            }
        }
    }
}
