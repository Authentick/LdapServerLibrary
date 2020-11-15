using System;
using System.Formats.Asn1;
using System.Numerics;
using System.Reflection;
using System.Runtime.Serialization;
using Gatekeeper.LdapServerLibrary.Models;
using Gatekeeper.LdapServerLibrary.Models.Operations;

namespace Gatekeeper.LdapServerLibrary.Parser
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

            if (tagClass != TagClass.Application)
            {
                throw new ArgumentException("Input type is expected to be " + TagClass.Application + " but was " + tagClass);
            }

            IProtocolOp message = DecodeApplicationData(tagValue, sequenceReader);

            return new LdapMessage(messageId, message);
        }

        internal Byte[] TryEncodePacket(LdapMessage message)
        {
            OperationMapper mapper = SingletonContainer.GetOperationMapper();
            Type encoder = mapper.GetEncoderForTag(message.ProtocolOp.GetTag());

            AsnWriter writer = new AsnWriter(AsnEncodingRules.BER);

            object? result = null;
            using (writer.PushSequence())
            {
                writer.WriteInteger(message.MessageId);

                var parameters = new object[] { writer, message.ProtocolOp };
                object? invokableClass = FormatterServices.GetUninitializedObject(encoder);

                if (invokableClass != null)
                {
                    MethodInfo? method = encoder.GetMethod("TryEncode");
                    if (method != null)
                    {
                        result = method.Invoke(invokableClass, parameters);
                    }
                }
            }

            if(result != null) {
                Byte[] data = writer.Encode();
                return data;
            }

            throw new NotImplementedException("The encoder for " + message.ProtocolOp.GetTag() + " is not implemented.");
        }

        private IProtocolOp DecodeApplicationData(int tagValue, AsnReader reader)
        {
            OperationMapper mapper = SingletonContainer.GetOperationMapper();
            Type decoder = mapper.GetDecoderForTag(tagValue);

            var parameters = new object[] { reader };
            object? invokableClass = FormatterServices.GetUninitializedObject(decoder);

            if (invokableClass != null)
            {
                MethodInfo? method = decoder.GetMethod("TryDecode");
                if (method != null)
                {
                    object result = method.Invoke(invokableClass, parameters);
                    if (result != null)
                    {
                        return (IProtocolOp)result;
                    }
                }
            }

            throw new NotImplementedException("The decoder for " + tagValue + " is not implemented.");
        }
    }
}
