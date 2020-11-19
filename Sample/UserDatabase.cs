using System.Collections.Generic;

namespace Sample
{
    internal class UserDatabase
    {
        private readonly List<User> Users = new List<User>{
            new User{
                Cn = "cn=test1,dc=example,dc=com",
                Attributes = new Dictionary<string, List<string>>(){
                    {"Email", new List<string>(){"test1@example.com"}},
                    {"Role", new List<string>(){"Administrator"}},
                },
            },
            new User{
                Cn = "cn=test2,dc=example,dc=com",
                Attributes = new Dictionary<string, List<string>>(){
                    {"Email", new List<string>(){"test2@example.com", "test2-alias@example.com"}},
                    {"Role", new List<string>(){"Employee"}},
                },
            },
            new User{
                Cn = "cn=test3,dc=example,dc=com",
                Attributes = new Dictionary<string, List<string>>(){
                    {"Email", new List<string>(){"test3@example.com"}},
                },
            },
            new User{
                Cn = "cn=benutzer4,dc=example,dc=com",
                Attributes = new Dictionary<string, List<string>>(){
                    {"Email", new List<string>(){"benutzer4@example.com"}},
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
