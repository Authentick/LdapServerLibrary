using System.Formats.Asn1;
using System.Text;
using Gatekeeper.LdapServerLibrary.Models.Operations.Response;

namespace Gatekeeper.LdapServerLibrary.Parser.Encoder
{
    internal class ExtendedOperationResponseEncoder : IApplicationEncoder<ExtendedOperationResponse>
    {
        public AsnWriter TryEncode(AsnWriter writer, ExtendedOperationResponse message)
        {
            Asn1Tag responseApplicationTag = new Asn1Tag(TagClass.Application, 24);
            using (writer.PushSequence(responseApplicationTag))
            {
                writer.WriteEnumeratedValue(message.LdapResult.ResultCode);
                writer.WriteOctetString(System.Text.Encoding.ASCII.GetBytes(""));
                writer.WriteOctetString(System.Text.Encoding.ASCII.GetBytes(""));                

                if (message.ResponseName != null)
                {
                    using (writer.PushOctetString(new Asn1Tag(TagClass.ContextSpecific, 10)))
                    {
                        writer.WriteOctetString(Encoding.ASCII.GetBytes(message.ResponseName));
                    }
                }
                if (message.ResponseValue != null)
                {
                    using (writer.PushOctetString(new Asn1Tag(TagClass.ContextSpecific, 11)))
                    {
                        writer.WriteOctetString(Encoding.ASCII.GetBytes(message.ResponseValue));
                    }
                }
            }

            return writer;
        }
    }
}