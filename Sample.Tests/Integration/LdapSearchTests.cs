using System.Diagnostics;
using Xunit;

namespace Sample.Tests.Integration
{
    public class LdapSearchTests : IClassFixture<LdapServerFixture>
    {
        private string ExecuteLdapSearch(string search)
        {
            return ExecuteLdapSearch(search, "dc=example,dc=com");
        }

        private string ExecuteLdapSearch(string search, string baseDn)
        {
            return ExecuteLdapSearch(search, baseDn, "cn=Manager,dc=example,dc=com", "ValidManagerPassword");
        }

        private string ExecuteLdapSearch(string search, string baseDn, string bindUser, string password)
        {
            string arguments = "";

            if (password != null)
            {
                arguments += "-w " + password + " ";
            }

            arguments += "-H ldap://localhost:3389 -b \"" + baseDn + "\" ";

            if (bindUser != null)
            {
                arguments += "-D \"" + bindUser + "\" ";
            }
            arguments += search;

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

            startInfo = new ProcessStartInfo()
            {
                FileName = "/usr/bin/ldapsearch",
                Arguments = arguments + " -ZZ",
                RedirectStandardError = true,
                RedirectStandardOutput = true,
            };
            startInfo.EnvironmentVariables.Add("LDAPTLS_REQCERT", "never");

            Process tlsProcess = new Process { StartInfo = startInfo };
            tlsProcess.Start();
            tlsProcess.WaitForExit();

            error = tlsProcess.StandardError.ReadToEnd();
            if (error != "")
            {
                return error;
            }

            string standardOut = p.StandardOutput.ReadToEnd();
            string tlsStandardOut = tlsProcess.StandardOutput.ReadToEnd();


            Assert.Equal(standardOut.Replace("\nsearch: 2\n", "\nsearch: 3\n"), tlsStandardOut);

            return standardOut;
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
objectclass: inetOrgPerson
displayname: Test User 1
uid: test1

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
objectclass: inetOrgPerson
displayname: Test User 2
uid: test2

# search result
search: 2
result: 0 Success

# numResponses: 2
# numEntries: 1
".Replace("\r", "");

            Assert.Equal(expected, output);
        }

        [Fact]
        public void TestPresenceObjectclassSearch()
        {
            string output = ExecuteLdapSearch("\"(objectclass=*)\"");
            string expected = @"# extended LDIF
#
# LDAPv3
# base <dc=example,dc=com> with scope subtree
# filter: (objectclass=*)
# requesting: ALL
#

# test1, example.com
dn: cn=test1,dc=example,dc=com
email: test1@example.com
role: Administrator
objectclass: inetOrgPerson
displayname: Test User 1
uid: test1

# test2, example.com
dn: cn=test2,dc=example,dc=com
email: test2@example.com
email: test2-alias@example.com
role: Employee
objectclass: inetOrgPerson
displayname: Test User 2
uid: test2

# test3, example.com
dn: cn=test3,dc=example,dc=com
email: test3@example.com
objectclass: inetOrgPerson
displayname: Test User 3
uid: test3

# benutzer4, example.com
dn: cn=benutzer4,dc=example,dc=com
email: benutzer4@example.com
objectclass: inetOrgPerson
displayname: Benutzer 4
uid: test4

# search result
search: 2
result: 0 Success

# numResponses: 5
# numEntries: 4
".Replace("\r", "");

            Assert.Equal(expected, output);
        }

        [Fact]
        public void TestSimpleAndFilter()
        {
            string output = ExecuteLdapSearch("\"(&(objectclass=*)(email=test1@example.com))\"");
            string expected = @"# extended LDIF
#
# LDAPv3
# base <dc=example,dc=com> with scope subtree
# filter: (&(objectclass=*)(email=test1@example.com))
# requesting: ALL
#

# test1, example.com
dn: cn=test1,dc=example,dc=com
email: test1@example.com
role: Administrator
objectclass: inetOrgPerson
displayname: Test User 1
uid: test1

# search result
search: 2
result: 0 Success

# numResponses: 2
# numEntries: 1
".Replace("\r", "");

            Assert.Equal(expected, output);
        }

        [Fact]
        public void TestSimpleOrFilter()
        {
            string output = ExecuteLdapSearch("\"(|(email=test1@example.com)(email=test2@example.com))\"");
            string expected = @"# extended LDIF
#
# LDAPv3
# base <dc=example,dc=com> with scope subtree
# filter: (|(email=test1@example.com)(email=test2@example.com))
# requesting: ALL
#

# test1, example.com
dn: cn=test1,dc=example,dc=com
email: test1@example.com
role: Administrator
objectclass: inetOrgPerson
displayname: Test User 1
uid: test1

# test2, example.com
dn: cn=test2,dc=example,dc=com
email: test2@example.com
email: test2-alias@example.com
role: Employee
objectclass: inetOrgPerson
displayname: Test User 2
uid: test2

# search result
search: 2
result: 0 Success

# numResponses: 3
# numEntries: 2
".Replace("\r", "");

            Assert.Equal(expected, output);
        }

        [Fact]
        public void TestNoSuchObject()
        {
            string output = ExecuteLdapSearch("\"(email=test99@example.com)\"");
            string expected = @"# extended LDIF
#
# LDAPv3
# base <dc=example,dc=com> with scope subtree
# filter: (email=test99@example.com)
# requesting: ALL
#

# search result
search: 2
result: 32 No such object

# numResponses: 1
".Replace("\r", "");

            Assert.Equal(expected, output);
        }

        [Fact]
        public void TestCnSubstringSearch()
        {
            string output = ExecuteLdapSearch("\"(cn=t*st*)\"");
            string expected = @"# extended LDIF
#
# LDAPv3
# base <dc=example,dc=com> with scope subtree
# filter: (cn=t*st*)
# requesting: ALL
#

# test1, example.com
dn: cn=test1,dc=example,dc=com
email: test1@example.com
role: Administrator
objectclass: inetOrgPerson
displayname: Test User 1
uid: test1

# test2, example.com
dn: cn=test2,dc=example,dc=com
email: test2@example.com
email: test2-alias@example.com
role: Employee
objectclass: inetOrgPerson
displayname: Test User 2
uid: test2

# test3, example.com
dn: cn=test3,dc=example,dc=com
email: test3@example.com
objectclass: inetOrgPerson
displayname: Test User 3
uid: test3

# search result
search: 2
result: 0 Success

# numResponses: 4
# numEntries: 3
".Replace("\r", "");

            Assert.Equal(expected, output);
        }

        [Fact]
        public void TestDisplaynameSubstringSearch()
        {
            string output = ExecuteLdapSearch("\"(displayname=T*st*)\"");
            string expected = @"# extended LDIF
#
# LDAPv3
# base <dc=example,dc=com> with scope subtree
# filter: (displayname=T*st*)
# requesting: ALL
#

# test1, example.com
dn: cn=test1,dc=example,dc=com
email: test1@example.com
role: Administrator
objectclass: inetOrgPerson
displayname: Test User 1
uid: test1

# test2, example.com
dn: cn=test2,dc=example,dc=com
email: test2@example.com
email: test2-alias@example.com
role: Employee
objectclass: inetOrgPerson
displayname: Test User 2
uid: test2

# test3, example.com
dn: cn=test3,dc=example,dc=com
email: test3@example.com
objectclass: inetOrgPerson
displayname: Test User 3
uid: test3

# search result
search: 2
result: 0 Success

# numResponses: 4
# numEntries: 3
".Replace("\r", "");

            Assert.Equal(expected, output);
        }

        [Fact]
        public void TestSingleSearchWithStrictBaseDn()
        {
            string output = ExecuteLdapSearch("\"(objectClass=*)\"", "cn=benutzer4,dc=example,dc=com");
            string expected = @"# extended LDIF
#
# LDAPv3
# base <cn=benutzer4,dc=example,dc=com> with scope subtree
# filter: (objectClass=*)
# requesting: ALL
#

# benutzer4, example.com
dn: cn=benutzer4,dc=example,dc=com
email: benutzer4@example.com
objectclass: inetOrgPerson
displayname: Benutzer 4
uid: test4

# search result
search: 2
result: 0 Success

# numResponses: 2
# numEntries: 1
".Replace("\r", "");

            Assert.Equal(expected, output);
        }

        [Fact]
        public void TestSingleSearchWithUserThatIsNotAllowedToBind()
        {
            string output = ExecuteLdapSearch("\"(objectClass=*)\"", "cn=benutzer4,dc=example,dc=com", "cn=OnlyBindUser", "OnlyBindUserPassword");
            string expected = @"# extended LDIF
#
# LDAPv3
# base <cn=benutzer4,dc=example,dc=com> with scope subtree
# filter: (objectClass=*)
# requesting: ALL
#

# search result
search: 2
result: 32 No such object

# numResponses: 1
".Replace("\r", "");

            Assert.Equal(expected, output);
        }

        [Fact]
        public void TestSearchWithInvalidPassword()
        {
            string output = ExecuteLdapSearch("\"(objectClass=*)\"", "cn=benutzer4,dc=example,dc=com", "cn=OnlyBindUser", "NotACorrectBindUserPassword");
            string expected = @"ldap_bind: Inappropriate authentication (48)
".Replace("\r", "");

            Assert.Equal(expected, output);
        }

        [Fact]
        public void TestSearchWithoutPasswordAndBindUser()
        {
            string output = ExecuteLdapSearch("\"(objectClass=*)\"", "cn=benutzer4,dc=example,dc=com", null, null);
            string expected = @"ldap_sasl_interactive_bind_s: Inappropriate authentication (48)
".Replace("\r", "");

            Assert.Equal(expected, output);
        }
    }
}
