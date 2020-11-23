using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using static Gatekeeper.LdapServerLibrary.Session.Events.SearchEvent;

namespace Sample
{
    internal class SearchExpressionBuilder
    {
        public static Expression Build(IFilterChoice filter, Expression itemExpression)
        {
            switch (filter)
            {
                case AndFilter af:
                    return BuildAndFilter(af, itemExpression);
                case OrFilter of:
                    return BuildOrFilter(of, itemExpression);
                case PresentFilter pf:
                    return BuildPresentFilter(pf, itemExpression);
                case EqualityMatchFilter eq:
                    return BuildEqualityFilter(eq, itemExpression);
                default:
                    throw new NotImplementedException("Filter for " + filter.GetType() + " is not implemented");
            }
        }

        private static Expression BuildOrFilter(OrFilter filter, Expression itemExpression)
        {
            List<Expression> expressions = new List<Expression>();

            Expression orFilterExpr = null;
            foreach(IFilterChoice subFilter in filter.Filters) {
                Expression subExpr = Build(subFilter, itemExpression);
                if(orFilterExpr == null) {
                    orFilterExpr = subExpr;
                } else {
                    orFilterExpr = Expression.Or(orFilterExpr, subExpr);
                }
            }

            return orFilterExpr;
        }

        private static Expression BuildAndFilter(AndFilter filter, Expression itemExpression)
        {
            List<Expression> expressions = new List<Expression>();

            Expression andFilterExpr = null;
            foreach(IFilterChoice subFilter in filter.Filters) {
                Expression subExpr = Build(subFilter, itemExpression);
                if(andFilterExpr == null) {
                    andFilterExpr = subExpr;
                } else {
                    andFilterExpr = Expression.And(andFilterExpr, subExpr);
                }
            }

            return andFilterExpr;
        }

        private static Expression BuildPresentFilter(PresentFilter filter, Expression itemExpression)
        {
            Expression attributeExpr = Expression.Property(itemExpression, "Attributes");
            Expression attributeContainsKey = Expression.Call(attributeExpr, typeof(Dictionary<string, List<string>>).GetMethod("ContainsKey", new Type[] { typeof(string) }), Expression.Constant(filter.Value));
            
            return attributeContainsKey;
        }

        private static Expression BuildEqualityFilter(EqualityMatchFilter filter, Expression itemExpression)
        {
            if (filter.AttributeDesc == "cn")
            {
                Expression left = Expression.Property(itemExpression, "Cn");
                Expression right = Expression.Constant("cn=" + filter.AssertionValue + ",dc=example,dc=com");
                return Expression.Equal(left, right);
            }
            else
            {
                Expression attributeExpr = Expression.Property(itemExpression, "Attributes");

                // Pair to search for
                ParameterExpression keyValuePair = Expression.Parameter(typeof(KeyValuePair<string, List<string>>), "a");

                // (a.Key == attributeName)
                Expression subExprLeftAttributeName = Expression.Property(keyValuePair, "Key");
                Expression subExprRightAttributeName = Expression.Constant(filter.AttributeDesc);
                Expression subExprAttributeName = Expression.Equal(subExprLeftAttributeName, subExprRightAttributeName);

                // a.Value.Contains(attributeValue) 
                Expression subExprValue = Expression.Property(keyValuePair, "Value");
                Expression subExprContains = Expression.Call(subExprValue, typeof(List<string>).GetMethod("Contains", new Type[] { typeof(string) }), Expression.Constant(filter.AssertionValue));

                // ((a.Key == attributeName) && a.Value.Contains(attributeValue))
                Expression attributeExprMatch = Expression.And(subExprAttributeName, subExprContains);

                // {a => ((a.Key == attributeName) And a.Value.Contains(attributeValue))}
                var lambda = Expression.Lambda<Func<KeyValuePair<string, List<string>>, bool>>(attributeExprMatch, keyValuePair);

                MethodInfo anyMethod = typeof(Enumerable).GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).First(m => m.Name == "Any" && m.GetParameters().Count() == 2).MakeGenericMethod(typeof(KeyValuePair<string, List<string>>));
                return Expression.Call(anyMethod, attributeExpr, lambda);
            }
        }
    }
}
