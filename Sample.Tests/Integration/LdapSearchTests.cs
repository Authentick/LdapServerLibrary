using Sample;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Sample.Tests.Integration
{
    public class LdapSearchTests : IClassFixture<LdapServerFixture>
    {
        private string ExecuteLdapSearch(string search)
        {
            string arguments = "-w test -H ldap://localhost:3389 -b \"dc=example,dc=com\" -D \"cn=Manager,dc=example,dc=com\" " + search;
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "/usr/bin/ldapsearch",
                Arguments = arguments,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
            };

            Process p = new Process { StartInfo = startInfo };
            p.Start();
            p.WaitForExit();

            string error = p.StandardError.ReadToEnd();
            if (error != "")
            {
                return error;
            }

            return p.StandardOutput.ReadToEnd();
        }

        [Fact]
        public void TestSimpleEqualCnSearch()
        {
            string output = ExecuteLdapSearch("\"cn=test1\"");
            string expected = @"# extended LDIF
#
# LDAPv3
# base <dc=example,dc=com> with scope subtree
# filter: cn=test1
# requesting: ALL
#

# test1, example.com
dn: cn=test1,dc=example,dc=com
email: test1@example.com
role: Administrator

# search result
search: 2
result: 0 Success

# numResponses: 2
# numEntries: 1
".Replace("\r", "");

            Assert.Equal(expected, output);
        }

        [Fact]
        public void TestSimpleEqualAttributeSearch()
        {
            string output = ExecuteLdapSearch("\"(email=test2-alias@example.com)\"");
            string expected = @"# extended LDIF
#
# LDAPv3
# base <dc=example,dc=com> with scope subtree
# filter: (email=test2-alias@example.com)
# requesting: ALL
#

# test2, example.com
dn: cn=test2,dc=example,dc=com
email: test2@example.com
email: test2-alias@example.com
role: Employee

# search result
search: 2
result: 0 Success

# numResponses: 2
# numEntries: 1
".Replace("\r", "");

            Assert.Equal(expected, output);
        }
    }
}
