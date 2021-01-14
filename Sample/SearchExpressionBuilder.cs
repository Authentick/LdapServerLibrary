using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Gatekeeper.LdapServerLibrary.Session.Events;
using static Gatekeeper.LdapPacketParserLibrary.Models.Operations.Request.SearchRequest;

namespace Sample
{
    internal class SearchExpressionBuilder
    {
        private readonly ISearchEvent _searchEvent;

        public SearchExpressionBuilder(ISearchEvent searchEvent)
        {
            _searchEvent = searchEvent;
        }

        public Expression Build(IFilterChoice filter, Expression itemExpression)
        {

            Expression? filterExpr = null;
            switch (filter)
            {
                case AndFilter af:
                    filterExpr = BuildAndFilter(af, itemExpression);
                    break;
                case OrFilter of:
                    filterExpr = BuildOrFilter(of, itemExpression);
                    break;
                case PresentFilter pf:
                    filterExpr = BuildPresentFilter(pf, itemExpression);
                    break;
                case EqualityMatchFilter eq:
                    filterExpr = BuildEqualityFilter(eq, itemExpression);
                    break;
                case SubstringFilter sf:
                    filterExpr = BuildSubstringFilter(sf, itemExpression);
                    break;
                default:
                    throw new NotImplementedException("Filter for " + filter.GetType() + " is not implemented");
            }

            return BuildWithBaseFilter(filterExpr, itemExpression);
        }

        private Expression BuildWithBaseFilter(Expression filterExpr, Expression itemExpr)
        {
            if (_searchEvent.SearchRequest.BaseObject == "")
            {
                return filterExpr;
            }
            else if (_searchEvent.SearchRequest.BaseObject.StartsWith("dc="))
            {
                MemberExpression dnExpr = Expression.Property(itemExpr, "Dn");
                MethodCallExpression valueExprEndsWith = Expression.Call(dnExpr, typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }), Expression.Constant(_searchEvent.SearchRequest.BaseObject));

                return Expression.And(valueExprEndsWith, filterExpr);
            }
            else
            {
                MemberExpression left = Expression.Property(itemExpr, "Dn");
                ConstantExpression right = Expression.Constant(_searchEvent.SearchRequest.BaseObject);
                BinaryExpression equalExpr = Expression.Equal(left, right);

                return Expression.And(equalExpr, filterExpr);
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
                MemberExpression dnProperty = Expression.Property(itemExpression, "Dn");
                string baseObj = (_searchEvent.SearchRequest.BaseObject == "") ? "" : "," + _searchEvent.SearchRequest.BaseObject;

                Regex regex = new Regex("^cn=" + suppliedRegex + Regex.Escape(baseObj) + "$", RegexOptions.Compiled);
                ConstantExpression regexConst = Expression.Constant(regex);

                MethodInfo methodInfo = typeof(Regex).GetMethod("IsMatch", new Type[] { typeof(string) });
                Expression[] callExprs = new Expression[] { dnProperty };

                return Expression.Call(regexConst, methodInfo, callExprs);
            }
            else
            {
                Expression attributeExpr = Expression.Property(itemExpression, "Attributes");

                // Pair to search for
                ParameterExpression keyValuePair = Expression.Parameter(typeof(KeyValuePair<string, List<string>>), "a");

                // rsl
                ParameterExpression regexStringList = Expression.Parameter(typeof(string), "rsl");

                // regex.IsMatch(rsl)
                Regex regex = new Regex("^" + suppliedRegex + "$", RegexOptions.Compiled);
                ConstantExpression regexConst = Expression.Constant(regex);
                MethodInfo methodInfo = typeof(Regex).GetMethod("IsMatch", new Type[] { typeof(string) });
                Expression[] callExprs = new Expression[] { regexStringList };
                MethodCallExpression regexMatchExpr = Expression.Call(regexConst, methodInfo, callExprs);

                // {rsl => regex.IsMatch(rsl)}
                var regexLambda = Expression.Lambda<Func<string, bool>>(regexMatchExpr, regexStringList);

                // a.Value.Any(rsl => regex.IsMatch(rsl))
                Expression subExprValue = Expression.Property(keyValuePair, "Value");
                MethodInfo regexAnyMethodInfo = typeof(Enumerable).GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).First(m => m.Name == "Any" && m.GetParameters().Count() == 2).MakeGenericMethod(typeof(string));
                MethodCallExpression regexAnyCallExpr = Expression.Call(regexAnyMethodInfo, subExprValue, regexLambda);

                // (a.Key == attributeName)
                Expression subExprLeftAttributeName = Expression.Property(keyValuePair, "Key");
                Expression subExprRightAttributeName = Expression.Constant(filter.AttributeDesc.ToLower());
                Expression subExprAttributeName = Expression.Equal(subExprLeftAttributeName, subExprRightAttributeName);

                // ((a.Key == attributeName) && a.Value.Any(rsl => regex.IsMatch(rsl)))
                Expression attributeExprMatch = Expression.And(subExprAttributeName, regexAnyCallExpr);

                // {a => ((a.Key == attributeName) And a.Value.Any(rsl => regex.IsMatch(rsl)))}
                var lambda = Expression.Lambda<Func<KeyValuePair<string, List<string>>, bool>>(attributeExprMatch, keyValuePair);

                MethodInfo anyMethod = typeof(Enumerable).GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).First(m => m.Name == "Any" && m.GetParameters().Count() == 2).MakeGenericMethod(typeof(KeyValuePair<string, List<string>>));
                return Expression.Call(anyMethod, attributeExpr, lambda);
            }
        }

        private Expression BuildEqualityFilter(EqualityMatchFilter filter, Expression itemExpression)
        {
            if (filter.AttributeDesc == "cn")
            {
                Expression left = Expression.Property(itemExpression, "Dn");
                string baseObj = (_searchEvent.SearchRequest.BaseObject == "") ? "" : "," + _searchEvent.SearchRequest.BaseObject;
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
