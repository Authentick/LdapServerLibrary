using System;
using System.Formats.Asn1;
using System.Numerics;
using LdapServer.Models;
using LdapServer.Models.Operations.Response;

namespace LdapServer
{
    internal class MessageEncoder
    {
        internal byte[] TryEncode(LdapMessage response)
        {
            AsnWriter writer = new AsnWriter(AsnEncodingRules.BER);

            if (typeof(BindResponse) == response.ProtocolOp.GetType())
            {
                BindResponse bindResponse = (BindResponse)response.ProtocolOp;

                Asn1Tag bindResponseApplication = new Asn1Tag(TagClass.Application, 1);

                using (writer.PushSequence())
                {
                    writer.WriteInteger(response.MessageId);
                    using (writer.PushSequence(bindResponseApplication))
                    {
                        writer.WriteEnumeratedValue(bindResponse.LdapResult.ResultCode);
                        writer.WriteOctetString(System.Text.Encoding.ASCII.GetBytes(""));
                        writer.WriteOctetString(System.Text.Encoding.ASCII.GetBytes("Stuff failed"));
                    }
                }


                Byte[] data = writer.Encode();
                String textData = System.Text.Encoding.ASCII.GetString(data);

                return data;
            }

            throw new Exception("Message not supported");
        }
    }
}