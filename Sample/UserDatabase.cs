using System.Collections.Generic;

namespace Sample
{
    internal class UserDatabase
    {
        private readonly List<User> Users = new List<User>{
            new User{
                Dn = "cn=test1,dc=example,dc=com",
                Attributes = new Dictionary<string, List<string>>(){
                    {"email", new List<string>(){"test1@example.com"}},
                    {"role", new List<string>(){"Administrator"}},
                    {"objectclass", new List<string>(){"inetOrgPerson"}},
                    {"displayname", new List<string>() {"Test User 1"}},
                    {"uid", new List<string>() {"test1"}},
                },
            },
            new User{
                Dn = "cn=test2,dc=example,dc=com",
                Attributes = new Dictionary<string, List<string>>(){
                    {"email", new List<string>(){"test2@example.com", "test2-alias@example.com"}},
                    {"role", new List<string>(){"Employee"}},
                    {"objectclass", new List<string>(){"inetOrgPerson"}},
                    {"displayname", new List<string>() {"Test User 2"}},
                    {"uid", new List<string>() {"test2"}},
                },
            },
            new User{
                Dn = "cn=test3,dc=example,dc=com",
                Attributes = new Dictionary<string, List<string>>(){
                    {"email", new List<string>(){"test3@example.com"}},
                    {"objectclass", new List<string>(){"inetOrgPerson"}},
                    {"displayname", new List<string>() {"Test User 3"}},
                    {"uid", new List<string>() {"test3"}},
                },
            },
            new User{
                Dn = "cn=benutzer4,dc=example,dc=com",
                Attributes = new Dictionary<string, List<string>>(){
                    {"email", new List<string>(){"benutzer4@example.com"}},
                    {"objectclass", new List<string>(){"inetOrgPerson"}},
                    {"displayname", new List<string>() {"Benutzer 4"}},
                    {"uid", new List<string>() {"test4"}},
                },
            },
        };

        internal List<User> GetUserDatabase()
        {
            return Users;
        }

        internal class User
        {
            internal string Dn { get; set; }
            internal Dictionary<string, List<string>> Attributes { get; set; }
        }
    }
}
