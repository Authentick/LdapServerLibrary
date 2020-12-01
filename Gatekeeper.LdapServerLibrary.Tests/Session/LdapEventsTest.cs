using Xunit;
using Gatekeeper.LdapServerLibrary.Session.Events;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Gatekeeper.LdapServerLibrary.Session.Replies;

namespace Gatekeeper.LdapServerLibrary.Tests.Session
{
    public class LdapEventsTest
    {
        [Fact]
        public async Task TestOnAuthenticationRequest()
        {
            LdapEvents events = new LdapEvents();

            IAuthenticationEvent authEventMock = new Mock<IAuthenticationEvent>().Object;
            bool result = await events.OnAuthenticationRequest(new ClientContext(), authEventMock);

            Assert.False(result);
        }

        [Fact]
        public async Task TestOnSearchRequest()
        {
            LdapEvents events = new LdapEvents();

            ISearchEvent searchEventMock = new Mock<ISearchEvent>().Object;

            List<SearchResultReply> result = await events.OnSearchRequest(new ClientContext(), searchEventMock);

            Assert.Empty(result);
        }
    }
}
