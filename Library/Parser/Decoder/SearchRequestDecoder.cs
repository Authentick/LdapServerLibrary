using System.Formats.Asn1;
using System.Numerics;
using LdapServer.Models.Operations.Request;

namespace LdapServer.Parser.Decoder
{
    internal class SearchRequestDecoder : IApplicationDecoder<SearchRequest>
    {
        public SearchRequest TryDecode(AsnReader reader)
        {
            Asn1Tag bindRequestApplication = new Asn1Tag(TagClass.Application, 3);
            AsnReader subReader = reader.ReadSequence(bindRequestApplication);

            string baseDn = System.Text.Encoding.ASCII.GetString(subReader.ReadOctetString());
            SearchRequest.ScopeEnum scope = subReader.ReadEnumeratedValue<SearchRequest.ScopeEnum>();
            SearchRequest.DerefAliasesEnum deref = subReader.ReadEnumeratedValue<SearchRequest.DerefAliasesEnum>();
            BigInteger sizeLimit = subReader.ReadInteger();
            BigInteger timeLimit = subReader.ReadInteger();
            bool typesOnly = subReader.ReadBoolean();

            switch(subReader.PeekTag().TagValue) {
                case 3:
                    AsnReader subsubreader = subReader.ReadSequence(new Asn1Tag(TagClass.ContextSpecific, 3));
                    string attributeDescription = System.Text.Encoding.ASCII.GetString(subsubreader.ReadOctetString());
                    string assertionValue =  System.Text.Encoding.ASCII.GetString(subsubreader.ReadOctetString());
                    break;
            }

            SearchRequest searchRequest = new SearchRequest();

            return searchRequest;
        }
    }
}