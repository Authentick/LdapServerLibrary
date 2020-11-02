using System;
using System.Formats.Asn1;
using System.Numerics;
using System.Reflection;
using System.Runtime.Serialization;
using LdapServer.Models;
using LdapServer.Models.Operations;

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

            if (tagClass != TagClass.Application)
            {
                throw new ArgumentException("Input type is expected to be " + TagClass.Application + " but was " + tagClass);
            }

            IProtocolOp message = DecodeApplicationData(tagValue, sequenceReader);

            return new LdapMessage(messageId, message);
        }

        private IProtocolOp DecodeApplicationData(int tagValue, AsnReader reader)
        {
            OperationMapper mapper = new OperationMapper();
            System.Console.WriteLine("bar");

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
