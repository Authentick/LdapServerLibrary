using System.Collections.Generic;
using Xunit;
using Gatekeeper.LdapServerLibrary.Parser;

namespace Gatekeeper.LdapServerLibrary.Tests.Parser
{
    public class RdnParserTest
    {
        [Theory]
        [MemberData(nameof(GetData))]
        public void TestParseRdnString(string rdn, Dictionary<string, List<string>> expected)
        {
            Assert.Equal(expected, RdnParser.ParseRdnString(rdn));
        }

        public static IEnumerable<object[]> GetData()
        {
            return new List<object[]>
            {
                new object[] { "uid=test1,ou=People,dc=example,dc=com", new Dictionary<string, List<string>> { { "uid", new List<string> { "test1" }}, {"ou", new List<string> {"People"}}, {"dc", new List<string>{"example", "com"}} }},
                new object[] { "InvalidRn", new Dictionary<string, List<string>>() },
            };
        }
    }
}
