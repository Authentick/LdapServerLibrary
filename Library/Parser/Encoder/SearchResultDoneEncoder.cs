using System.Formats.Asn1;
using LdapServer.Models.Operations.Response;

namespace LdapServer.Parser.Encoder
{
    internal class SearchResultDoneEncoder : IApplicationEncoder<SearchResultDone>
    {
        public AsnWriter TryEncode(AsnWriter writer, SearchResultDone message)
        {
            Asn1Tag bindResponseApplication = new Asn1Tag(TagClass.Application, 5);

            using (writer.PushSequence(bindResponseApplication))
            {
                writer.WriteEnumeratedValue(message.LdapResult.ResultCode);
                writer.WriteOctetString(System.Text.Encoding.ASCII.GetBytes(""));
                writer.WriteOctetString(System.Text.Encoding.ASCII.GetBytes(""));
            }

            return writer;
        }
    }
}