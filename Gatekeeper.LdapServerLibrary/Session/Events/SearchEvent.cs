using Gatekeeper.LdapPacketParserLibrary.Models.Operations.Request;

namespace Gatekeeper.LdapServerLibrary.Session.Events
{
    public class SearchEvent : ISearchEvent
    {
        public SearchRequest SearchRequest { get; set; } = null!;
    }
}
