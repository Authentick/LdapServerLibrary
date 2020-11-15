using System.Formats.Asn1;
using Gatekeeper.LdapServerLibrary.Models.Operations.Response;
using static Gatekeeper.LdapServerLibrary.Session.Replies.SearchResultReply;

namespace Gatekeeper.LdapServerLibrary.Parser.Encoder
{
    internal class SearchResultEntryEncoder : IApplicationEncoder<SearchResultEntry>
    {
        public AsnWriter TryEncode(AsnWriter writer, SearchResultEntry message)
        {
            Asn1Tag searchResultEntryApplication = new Asn1Tag(TagClass.Application, 4);

            using (writer.PushSequence(searchResultEntryApplication))
            {
                writer.WriteOctetString(System.Text.Encoding.ASCII.GetBytes(message.SearchResultReply.CommonName));
                using (writer.PushSequence())
                {
                    foreach (Attribute attribute in message.SearchResultReply.Attributes)
                    {
                        using (writer.PushSequence())
                        {
                            writer.WriteOctetString(System.Text.Encoding.ASCII.GetBytes(attribute.Key));
                            using (writer.PushSetOf())
                            {
                                foreach (string value in attribute.Values)
                                {
                                    writer.WriteOctetString(System.Text.Encoding.ASCII.GetBytes(value));
                                }
                            }
                        }
                    }
                }
            }

            return writer;
        }
    }
}