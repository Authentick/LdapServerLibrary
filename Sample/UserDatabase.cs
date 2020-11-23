using System.Collections.Generic;

namespace Sample
{
    internal class UserDatabase
    {
        private readonly List<User> Users = new List<User>{
            new User{
                Cn = "cn=test1,dc=example,dc=com",
                Attributes = new Dictionary<string, List<string>>(){
                    {"email", new List<string>(){"test1@example.com"}},
                    {"role", new List<string>(){"Administrator"}},
                    {"objectClass", new List<string>(){"inetOrgPerson"}},
                },
            },
            new User{
                Cn = "cn=test2,dc=example,dc=com",
                Attributes = new Dictionary<string, List<string>>(){
                    {"email", new List<string>(){"test2@example.com", "test2-alias@example.com"}},
                    {"role", new List<string>(){"Employee"}},
                    {"objectClass", new List<string>(){"inetOrgPerson"}},
                },
            },
            new User{
                Cn = "cn=test3,dc=example,dc=com",
                Attributes = new Dictionary<string, List<string>>(){
                    {"email", new List<string>(){"test3@example.com"}},
                    {"objectClass", new List<string>(){"inetOrgPerson"}},
                },
            },
            new User{
                Cn = "cn=benutzer4,dc=example,dc=com",
                Attributes = new Dictionary<string, List<string>>(){
                    {"email", new List<string>(){"benutzer4@example.com"}},
                    {"objectClass", new List<string>(){"inetOrgPerson"}},
                },
            },
        };

        internal List<User> GetUserDatabase()
        {
            return Users;
        }

        internal class User
        {
            internal string Cn { get; set; }
            internal Dictionary<string, List<string>> Attributes { get; set; }
        }
    }
}
