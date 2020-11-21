using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
            if (authenticationEvent.Username == "cn=Manager,dc=example,dc=com" && authenticationEvent.Password == "test")
            {
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        private Expression BuildExpression(IFilterChoice filter, Expression ItemExpression)
        {
            switch (filter)
            {
                case EqualityMatchFilter eq:
                    if (eq.AttributeDesc == "cn")
                    {
                        Expression left = Expression.Property(ItemExpression, "Cn");
                        Expression right = Expression.Constant("cn=" + eq.AssertionValue + ",dc=example,dc=com");
                        return Expression.Equal(left, right);
                    }
                    else
                    {
                        Expression attributeExpr = Expression.Property(ItemExpression, "Attributes");

                        // Pair to search for
                        ParameterExpression keyValuePair = Expression.Parameter(typeof(KeyValuePair<string, List<string>>), "a");

                        // (a.Key == attributeName)
                        Expression subExprLeftAttributeName = Expression.Property(keyValuePair, "Key");
                        Expression subExprRightAttributeName = Expression.Constant(eq.AttributeDesc);
                        Expression subExprAttributeName = Expression.Equal(subExprLeftAttributeName, subExprRightAttributeName);

                        // a.Value.Contains(attributeValue) 
                        Expression subExprValue = Expression.Property(keyValuePair, "Value");
                        Expression subExprContains = Expression.Call(subExprValue, typeof(List<string>).GetMethod("Contains", new Type[] { typeof(string) }), Expression.Constant(eq.AssertionValue));

                        // ((a.Key == attributeName) && a.Value.Contains(attributeValue))
                        Expression attributeExprMatch = Expression.And(subExprAttributeName, subExprContains);

                        // {a => ((a.Key == attributeName) And a.Value.Contains(attributeValue))}
                        var lambda = Expression.Lambda<Func<KeyValuePair<string, List<string>>, bool>>(attributeExprMatch, keyValuePair);

                        MethodInfo allMethod = typeof(Enumerable).GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).First(m => m.Name == "Any" && m.GetParameters().Count() == 2).MakeGenericMethod(typeof(KeyValuePair<string, List<string>>));
                        return Expression.Call(allMethod, attributeExpr, lambda);
                    }
                default:
                    throw new NotImplementedException("Filter for " + filter.GetType() + " is not implemented");
            }
        }

        public override Task<List<SearchResultReply>> OnSearchRequest(ClientContext context, SearchEvent searchEvent)
        {
            int? limit = searchEvent.SizeLimit;

            // Load the user database that queries will be executed against
            UserDatabase dbContainer = new UserDatabase();
            IQueryable<UserDatabase.User> userDb = dbContainer.GetUserDatabase().AsQueryable();

            var itemExpression = Expression.Parameter(typeof(UserDatabase.User));
            var conditions = BuildExpression(searchEvent.Filter, itemExpression);
            var queryLambda = Expression.Lambda<Func<UserDatabase.User, bool>>(conditions, itemExpression);
            var predicate = queryLambda.Compile();

            var results = userDb.Where(predicate).ToList();

            List<SearchResultReply> replies = new List<SearchResultReply>();
            foreach (UserDatabase.User user in results)
            {
                List<SearchResultReply.Attribute> attributes = new List<SearchResultReply.Attribute>();
                SearchResultReply reply = new SearchResultReply(
                    user.Cn,
                    attributes
                );

                foreach (KeyValuePair<string, List<string>> attribute in user.Attributes)
                {
                    SearchResultReply.Attribute attributeClass = new SearchResultReply.Attribute(attribute.Key, attribute.Value);
                    attributes.Add(attributeClass);
                }

                replies.Add(reply);
            }

            return Task.FromResult(replies);
        }
    }
}
