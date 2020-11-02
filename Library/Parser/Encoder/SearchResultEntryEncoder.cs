using System.Formats.Asn1;
using LdapServer.Models.Operations.Response;

namespace LdapServer.Parser.Encoder
{
    internal class SearchResultEntryEncoder : IApplicationEncoder<SearchResultEntry>
    {
        public AsnWriter TryEncode(AsnWriter writer, SearchResultEntry message)
        {
            Asn1Tag searchResultEntryApplication = new Asn1Tag(TagClass.Application, 4);

            using (writer.PushSequence(searchResultEntryApplication))
            {
                writer.WriteOctetString(System.Text.Encoding.ASCII.GetBytes("foobar@test.com"));
                using(writer.PushSequence()) {}
            }

            return writer;
        }
    }
}