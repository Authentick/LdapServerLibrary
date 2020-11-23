using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Numerics;
using Gatekeeper.LdapServerLibrary.Models.Operations.Request;
using static Gatekeeper.LdapServerLibrary.Session.Events.SearchEvent;

namespace Gatekeeper.LdapServerLibrary.Parser.Decoder
{
    internal class SearchRequestDecoder : IApplicationDecoder<SearchRequest>
    {
        public SearchRequest TryDecode(AsnReader reader)
        {
            SearchRequest searchRequest = new SearchRequest();

            Asn1Tag bindRequestApplication = new Asn1Tag(TagClass.Application, 3);
            AsnReader subReader = reader.ReadSequence(bindRequestApplication);
            searchRequest.BaseObject = System.Text.Encoding.ASCII.GetString(subReader.ReadOctetString());
            SearchRequest.ScopeEnum scope = subReader.ReadEnumeratedValue<SearchRequest.ScopeEnum>();
            SearchRequest.DerefAliasesEnum deref = subReader.ReadEnumeratedValue<SearchRequest.DerefAliasesEnum>();
            BigInteger sizeLimit = subReader.ReadInteger();
            BigInteger timeLimit = subReader.ReadInteger();
            bool typesOnly = subReader.ReadBoolean();

            searchRequest.Filter = DecodeSearchFilter(subReader);

            //            subReader.ThrowIfNotEmpty();

            return searchRequest;
        }

        private TFilter DecodeAttributeValueAssertionFilter<TFilter>(AsnReader reader) where TFilter : AttributeValueAssertionFilter, new()
        {
            AsnReader subReader = reader.ReadSequence(new Asn1Tag(TagClass.ContextSpecific, reader.PeekTag().TagValue));
            string attributeDescription = System.Text.Encoding.ASCII.GetString(subReader.ReadOctetString());
            string assertionValue = System.Text.Encoding.ASCII.GetString(subReader.ReadOctetString());

            return new TFilter { AssertionValue = assertionValue, AttributeDesc = attributeDescription };
        }

        private List<IFilterChoice> DecodeRecursiveFilterSets(AsnReader reader)
        {
            AsnReader subReader = reader.ReadSetOf(new Asn1Tag(TagClass.ContextSpecific, reader.PeekTag().TagValue));
            List<IFilterChoice> filters = new List<IFilterChoice>();

            while (subReader.HasData)
            {
                filters.Add(DecodeSearchFilter(subReader));
            }

            return filters;
        }

        private IFilterChoice DecodeSearchFilter(AsnReader reader)
        {
            switch (reader.PeekTag().TagValue)
            {
                case 0:
                    return new AndFilter { Filters = DecodeRecursiveFilterSets(reader) };
                case 1:
                    return new OrFilter { Filters = DecodeRecursiveFilterSets(reader) };
                case 2:
                    return new NotFilter { Filter = DecodeSearchFilter(reader) };
                case 3:
                    return DecodeAttributeValueAssertionFilter<EqualityMatchFilter>(reader);
                case 5:
                    return DecodeAttributeValueAssertionFilter<GreaterOrEqualFilter>(reader);
                case 6:
                    return DecodeAttributeValueAssertionFilter<LessOrEqualFilter>(reader);
                case 7:
                    return new PresentFilter { Value = System.Text.Encoding.ASCII.GetString(reader.ReadOctetString(new Asn1Tag(TagClass.ContextSpecific, reader.PeekTag().TagValue))) };
                case 8:
                    return DecodeAttributeValueAssertionFilter<ApproxMatchFilter>(reader);
                default:
                    throw new NotImplementedException("Cannot decode the tag: " + reader.PeekTag().TagValue);
            }
        }

    }
}
