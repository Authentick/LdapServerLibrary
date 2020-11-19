using System.Collections.Generic;

namespace Sample
{
    internal class UserDatabase
    {
        private readonly List<User> Users = new List<User>{
            new User{
                Cn = "cn=test1,dc=ldap,dc=net",
                Attributes = new Dictionary<string, List<string>>(){
                    {"Email", new List<string>(){"test1@ldap.net"}},
                    {"Role", new List<string>(){"Administrator"}},
                },
            },
            new User{
                Cn = "cn=test2,dc=ldap,dc=net",
                Attributes = new Dictionary<string, List<string>>(){
                    {"Email", new List<string>(){"test2@ldap.net", "test2-alias@ldap.net"}},
                    {"Role", new List<string>(){"Employee"}},
                },
            },
            new User{
                Cn = "cn=test3,dc=ldap,dc=net",
                Attributes = new Dictionary<string, List<string>>(){
                    {"Email", new List<string>(){"test3@ldap.net"}},
                },
            },
            new User{
                Cn = "cn=benutzer4,dc=ldap,dc=net",
                Attributes = new Dictionary<string, List<string>>(){
                    {"Email", new List<string>(){"benutzer4@ldap.net"}},
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
