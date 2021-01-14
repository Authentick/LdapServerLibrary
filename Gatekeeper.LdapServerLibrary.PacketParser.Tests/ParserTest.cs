using System.Collections.Generic;
using Gatekeeper.LdapServerLibrary.PacketParser.Models;
using Gatekeeper.LdapServerLibrary.PacketParser;
using Xunit;
using Gatekeeper.LdapServerLibrary.PacketParser.Models.Operations.Request;
using System;
using SemanticComparison.Fluent;
using Gatekeeper.LdapServerLibrary.PacketParser.Models.Operations;

namespace Gatekeeper.LdapServerLibrary.PacketParser.Tests
{
    public class ParserTest
    {
        [Theory]
        [MemberData(nameof(GetData))]
        public void TestParse(string input, LdapMessage expected)
        {
            PacketParser.Parser parser = new PacketParser.Parser();

            LdapMessage message = parser.TryParsePacket(Convert.FromBase64String(input));

            System.Console.WriteLine(((SearchRequest)message.ProtocolOp).DerefAliases);

            expected.ProtocolOp.AsSource()
                .OfLikeness<SearchRequest>()
                .ShouldEqual((SearchRequest)message.ProtocolOp);
        }

        public static IEnumerable<object[]> GetData()
        {
            return new List<object[]>
            {
                new object[] {
                    "MDYCAQJjMQQRZGM9ZXhhbXBsZSxkYz1jb20KAQIKAQACAQACAQABAQCjCwQCY24EBXRlc3QxMAA=",
                    new LdapMessage(
                        new System.Numerics.BigInteger(2),
                        new SearchRequest
                        {
                            DerefAliases = SearchRequest.DerefAliasesEnum.NeverDerefAliases,
                            BaseObject = "dc=example,dc=com",
                            SizeLimit = 0,
                            TimeLimit = 0,

                        }
                    ),
                },
            };
        }
    }
}
