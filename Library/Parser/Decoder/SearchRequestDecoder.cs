using System.Formats.Asn1;
using System.Numerics;
using LdapServer.Models.Operations.Request;

namespace LdapServer.Parser.Decoder
{
    internal class SearchRequestDecoder : IApplicationDecoder<SearchRequest>
    {
        public SearchRequest TryDecode(AsnReader reader)
        {
            SearchRequest searchRequest = new SearchRequest();

            return searchRequest;
        }
    }
}