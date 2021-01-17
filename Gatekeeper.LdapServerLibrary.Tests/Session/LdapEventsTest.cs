using Xunit;
using Gatekeeper.LdapServerLibrary.Session.Events;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Gatekeeper.LdapServerLibrary.Session.Replies;
using System.Net;

namespace Gatekeeper.LdapServerLibrary.Tests.Session
{
    public class LdapEventsTest
    {
        [Fact]
        public async Task TestOnAuthenticationRequest()
        {
            LdapEvents events = new LdapEvents();

            IAuthenticationEvent authEventMock = new Mock<IAuthenticationEvent>().Object;
            bool result = await events.OnAuthenticationRequest(new ClientContext(IPAddress.Parse("127.0.0.1")), authEventMock);

            Assert.False(result);
        }

        [Fact]
        public async Task TestOnSearchRequest()
        {
            LdapEvents events = new LdapEvents();

            ISearchEvent searchEventMock = new Mock<ISearchEvent>().Object;

            List<SearchResultReply> result = await events.OnSearchRequest(new ClientContext(IPAddress.Parse("127.0.0.1")), searchEventMock);

            Assert.Empty(result);
        }
    }
}
