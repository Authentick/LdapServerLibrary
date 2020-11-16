using System.Linq;

namespace Sample {
    internal class UserDatabase {
        private IQueryable<User> Users;

        internal IQueryable<User> GetUserDatabase() {
            return Users;
        }

        internal class User {
            internal string name;
            internal string foo;
        }
    }
}
