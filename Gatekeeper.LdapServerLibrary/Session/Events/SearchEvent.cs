using Gatekeeper.LdapServerLibrary.PacketParser.Models.Operations.Request;

namespace Gatekeeper.LdapServerLibrary.Session.Events
{
    public class SearchEvent : ISearchEvent
    {
        public SearchRequest SearchRequest { get; set; } = null!;
    }
}
