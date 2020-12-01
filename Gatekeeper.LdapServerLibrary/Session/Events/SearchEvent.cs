using System.Collections.Generic;

namespace Gatekeeper.LdapServerLibrary.Session.Events
{
    public class SearchEvent : ISearchEvent
    {
        public string BaseObject { get; set; } = null!;
        public ISearchEvent.ScopeEnum Scope { get; set; }
        public ISearchEvent.DerefAliasesEnum DerefAliases { get; set; }
        public int? SizeLimit { get; set; }
        public int TimeLimit { get; set; }
        public bool TypesOnly { get; set; }
        public ISearchEvent.IFilterChoice Filter { get; set; } = null!;
        public List<string> AttributeSelection { get; set; } = new List<string>();
    }
}
