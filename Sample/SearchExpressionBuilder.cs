using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Gatekeeper.LdapServerLibrary.Session.Events;
using static Gatekeeper.LdapServerLibrary.Session.Events.SearchEvent;

namespace Sample
{
    internal class SearchExpressionBuilder
    {
        private readonly SearchEvent _searchEvent;

        public SearchExpressionBuilder(SearchEvent searchEvent)
        {
            _searchEvent = searchEvent;
        }

        public Expression Build(IFilterChoice filter, Expression itemExpression)
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
                case SubstringFilter sf:
                    return BuildSubstringFilter(sf, itemExpression);
                default:
                    throw new NotImplementedException("Filter for " + filter.GetType() + " is not implemented");
            }
        }

        private Expression BuildOrFilter(OrFilter filter, Expression itemExpression)
        {
            List<Expression> expressions = new List<Expression>();

            Expression orFilterExpr = null;
            foreach (IFilterChoice subFilter in filter.Filters)
            {
                Expression subExpr = Build(subFilter, itemExpression);
                if (orFilterExpr == null)
                {
                    orFilterExpr = subExpr;
                }
                else
                {
                    orFilterExpr = Expression.Or(orFilterExpr, subExpr);
                }
            }

            return orFilterExpr;
        }

        private Expression BuildAndFilter(AndFilter filter, Expression itemExpression)
        {
            List<Expression> expressions = new List<Expression>();

            Expression andFilterExpr = null;
            foreach (IFilterChoice subFilter in filter.Filters)
            {
                Expression subExpr = Build(subFilter, itemExpression);
                if (andFilterExpr == null)
                {
                    andFilterExpr = subExpr;
                }
                else
                {
                    andFilterExpr = Expression.And(andFilterExpr, subExpr);
                }
            }

            return andFilterExpr;
        }

        private Expression BuildPresentFilter(PresentFilter filter, Expression itemExpression)
        {
            Expression attributeExpr = Expression.Property(itemExpression, "Attributes");
            Expression attributeContainsKey = Expression.Call(attributeExpr, typeof(Dictionary<string, List<string>>).GetMethod("ContainsKey", new Type[] { typeof(string) }), Expression.Constant(filter.Value.ToLower()));

            return attributeContainsKey;
        }

        private Expression BuildSubstringFilter(SubstringFilter filter, Expression itemExpression)
        {
            string suppliedRegex = "";

            if (filter.Initial != null)
            {
                suppliedRegex = Regex.Escape(filter.Initial);
            }
            else
            {
                suppliedRegex = ".*";
            }

            foreach (string anyString in filter.Any)
            {
                suppliedRegex = suppliedRegex + ".*" + Regex.Escape(anyString) + ".*";
            }

            if (filter.Final != null)
            {
                suppliedRegex = suppliedRegex + Regex.Escape(filter.Final);
            }
            else
            {
                suppliedRegex = suppliedRegex + ".*";
            }

            if (filter.AttributeDesc == "cn")
            {
                MemberExpression cnProperty = Expression.Property(itemExpression, "Cn");
                string baseObj = (_searchEvent.BaseObject == "") ? "" : "," + _searchEvent.BaseObject;

                Regex regex = new Regex("^cn=" + suppliedRegex + Regex.Escape(baseObj) + "$", RegexOptions.Compiled);
                ConstantExpression regexConst = Expression.Constant(regex);

                MethodInfo methodInfo = typeof(Regex).GetMethod("IsMatch", new Type[] { typeof(string) });
                Expression[] callExprs = new Expression[] { cnProperty };

                return Expression.Call(regexConst, methodInfo, callExprs);
            }
            else
            {
                // TODO
                throw new NotImplementedException("Not implemented");
            }
        }

        private Expression BuildEqualityFilter(EqualityMatchFilter filter, Expression itemExpression)
        {
            if (filter.AttributeDesc == "cn")
            {
                Expression left = Expression.Property(itemExpression, "Cn");
                string baseObj = (_searchEvent.BaseObject == "") ? "" : "," + _searchEvent.BaseObject;
                Expression right = Expression.Constant("cn=" + filter.AssertionValue + baseObj);
                return Expression.Equal(left, right);
            }
            else
            {
                Expression attributeExpr = Expression.Property(itemExpression, "Attributes");

                // Pair to search for
                ParameterExpression keyValuePair = Expression.Parameter(typeof(KeyValuePair<string, List<string>>), "a");

                // (a.Key == attributeName)
                Expression subExprLeftAttributeName = Expression.Property(keyValuePair, "Key");
                Expression subExprRightAttributeName = Expression.Constant(filter.AttributeDesc.ToLower());
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
