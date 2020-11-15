using System.Collections.Generic;
using System.Threading.Tasks;
using Gatekeeper.LdapServerLibrary;
using Gatekeeper.LdapServerLibrary.Session.Events;
using Gatekeeper.LdapServerLibrary.Session.Replies;

namespace Sample
{
    class LdapEventListener : LdapEvents
    {
        public override Task<bool> OnAuthenticationRequest(ClientContext context, AuthenticationEvent authenticationEvent)
        {
            if (authenticationEvent.Username == "cn=Manager,dc=ldap,dc=net" && authenticationEvent.Password == "test")
            {
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public override Task<List<SearchResultReply>> OnSearchRequest(ClientContext context, SearchEvent searchEvent)
        {
            List<SearchResultReply> replies = new List<SearchResultReply>();


            SearchResultReply reply1 = new SearchResultReply(
                "cn=test,dc=ldap,dc=net",
                new List<SearchResultReply.Attribute>()
            );
            SearchResultReply reply2 = new SearchResultReply(
                "cn=test1,dc=ldap,dc=net",
                new List<SearchResultReply.Attribute>{
                    new SearchResultReply.Attribute("Email", new List<string>{"bar@bar.com", "foo@foo.com"})
                }
            );

            replies.Add(reply1);
            replies.Add(reply2);

            return Task.FromResult(replies);
        }
    }
}
