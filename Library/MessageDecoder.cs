using System;
using System.Formats.Asn1;
using System.Numerics;
using LdapServer.Models;
using LdapServer.Models.Operations.Request;

namespace LdapServer
{
    internal class MessageDecoder
    {
        internal LdapMessage TryDecode(Byte[] input)
        {
            AsnReader reader = new AsnReader(input, AsnEncodingRules.BER);
            AsnReader sequenceReader = reader.ReadSequence();

            BigInteger messageId = sequenceReader.ReadInteger();

            // BindRequest
            Asn1Tag bindRequestApplication = new Asn1Tag(TagClass.Application, 0);
            // UnbindRequest
            Asn1Tag unbindRequestApplication = new Asn1Tag(TagClass.Application, 2);
            if (sequenceReader.PeekTag().HasSameClassAndValue(bindRequestApplication))
            {
                AsnReader subReader = sequenceReader.ReadSequence(bindRequestApplication);

                BigInteger version = subReader.ReadInteger();

                string nameString = System.Text.Encoding.ASCII.GetString(subReader.ReadOctetString());

                Asn1Tag authContext = new Asn1Tag(TagClass.ContextSpecific, 0);
                string authString = System.Text.Encoding.ASCII.GetString(subReader.ReadOctetString(authContext));

                subReader.ThrowIfNotEmpty();
                reader.ThrowIfNotEmpty();

                BindRequest bindRequest = new BindRequest(version, nameString, authString);
                LdapMessage message = new LdapMessage(
                    messageId,
                    bindRequest
                );

                return message;
            } 

            if (sequenceReader.PeekTag().HasSameClassAndValue(unbindRequestApplication)) {
                return new LdapMessage(messageId, new UnbindRequest());
            }

            throw new Exception("Message not supported: " + sequenceReader.PeekTag());
        }
    }
}