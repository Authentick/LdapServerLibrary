using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Gatekeeper.LdapServerLibrary;
using Gatekeeper.LdapServerLibrary.Session.Events;
using Gatekeeper.LdapServerLibrary.Session.Replies;
using static Gatekeeper.LdapServerLibrary.Session.Events.SearchEvent;

namespace Sample
{
    class LdapEventListener : LdapEvents
    {
        public override Task<bool> OnAuthenticationRequest(ClientContext context, AuthenticationEvent authenticationEvent)
        {
            if (authenticationEvent.Username == "cn=Manager,dc=ldap,dc=net" && authenticationEvent.Password == "test")
            {
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public override Task<List<SearchResultReply>> OnSearchRequest(ClientContext context, SearchEvent searchEvent)
        {
            int? limit = searchEvent.SizeLimit;

            EqualityMatchFilter filter = (EqualityMatchFilter) searchEvent.Filter;

            // Load the user database that queries will be executed against
            UserDatabase dbContainer = new UserDatabase();
            IQueryable<UserDatabase.User> userDb = dbContainer.GetUserDatabase().AsQueryable();

            ParameterExpression pe = Expression.Parameter(typeof(UserDatabase.User), "user");
            var valueExpr = Expression.Property(pe, "Cn");

            Expression left = Expression.Call(valueExpr, typeof(string).GetMethod("ToLower", System.Type.EmptyTypes));
            Expression right = Expression.Constant("cn=" + filter.AssertionValue + ",dc=ldap,dc=net");
            Expression e1 = Expression.Equal(left, right);

            MethodCallExpression whereCallExpression = Expression.Call(
                typeof(Queryable),
                "Where",
                new Type[] { userDb.ElementType },
                userDb.Expression,
                Expression.Lambda<Func<UserDatabase.User, bool>>(e1, new ParameterExpression[] { pe }));

            IQueryable<UserDatabase.User> results = userDb.Provider.CreateQuery<UserDatabase.User>(whereCallExpression);

            List<SearchResultReply> replies = new List<SearchResultReply>();
            foreach (UserDatabase.User user in results)
            {
                List<SearchResultReply.Attribute> attributes = new List<SearchResultReply.Attribute>();
                SearchResultReply reply = new SearchResultReply(
                    user.Cn,
                    attributes
                );

                foreach(KeyValuePair<string, List<string>> attribute in user.Attributes) {
                    SearchResultReply.Attribute attributeClass = new SearchResultReply.Attribute(attribute.Key, attribute.Value);
                    attributes.Add(attributeClass);
                }

                replies.Add(reply);
            }

            return Task.FromResult(replies);


            // Searches include filters that you need to search against, 
            // such as `ApplyRecursiveFilter` would do
            /*            IFilterChoice filter = searchEvent.Filter;

                        userDb = ApplyRecursiveFilter(filter, userDb);

                        // Searches can also have a limit
                        if (limit != null)
                        {
                            userDb = userDb.Take((int)limit);
                        }

                        // Searches also can have a limit
                        List<SearchResultReply> replies = new List<SearchResultReply>();


                        SearchResultReply reply1 = new SearchResultReply(
                            "cn=test,dc=ldap,dc=net",
                            new List<SearchResultReply.Attribute>()
                        );
                        SearchResultReply reply2 = new SearchResultReply(
                            "cn=test1,dc=ldap,dc=net",
                            new List<SearchResultReply.Attribute>{
                                new SearchResultReply.Attribute("Email", new List<string>{"bar@bar.com", "foo@foo.com"})
                            }
                        );

                        replies.Add(reply1);
                        replies.Add(reply2);*/

        }

        private IQueryable<UserDatabase.User> ApplyRecursiveFilter(IFilterChoice filter, IQueryable<UserDatabase.User> userDb)
        {
            switch (filter)
            {
                case AndFilter andFilter:
                    List<IFilterChoice> andFilters = andFilter.Filters;
                    foreach (IFilterChoice singleAndFilter in andFilters)
                    {

                    }
                    break;
                case OrFilter orFilter:
                    break;
                case NotFilter notFilter:
                    break;
                default:
                    throw new NotImplementedException("Filter for " + filter.GetType() + " is not implemented");
            }

            return userDb;
        }
    }
}
