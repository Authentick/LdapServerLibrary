using Sample;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Sample.Tests.Integration
{
    public class LdapSearchTests
    {
        private void StartServer()
        {
            Thread.Sleep(1000);

            Sample.Program program = new Sample.Program();
            new Thread(async () =>
            {
                Thread.CurrentThread.IsBackground = true;
                await Sample.Program.Main(new string[0]);
            }).Start();
        }

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
        public void TestSimpleEqualSearch()
        {
            StartServer();

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
Email: test1@example.com
Role: Administrator

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
