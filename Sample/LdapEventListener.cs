using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Gatekeeper.LdapServerLibrary;
using Gatekeeper.LdapServerLibrary.Session.Events;
using Gatekeeper.LdapServerLibrary.Session.Replies;

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

        public override Task<List<SearchResultReply>> OnSearchRequest(ClientContext context, SearchEvent searchEvent)
        {
            int? limit = searchEvent.SizeLimit;

            // Load the user database that queries will be executed against
            UserDatabase dbContainer = new UserDatabase();
            IQueryable<UserDatabase.User> userDb = dbContainer.GetUserDatabase().AsQueryable();

            var itemExpression = Expression.Parameter(typeof(UserDatabase.User));
            SearchExpressionBuilder searchExpressionBuilder = new SearchExpressionBuilder(searchEvent);
            var conditions = searchExpressionBuilder.Build(searchEvent.Filter, itemExpression);
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
