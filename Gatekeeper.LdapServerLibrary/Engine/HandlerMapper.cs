using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Gatekeeper.LdapServerLibrary.Engine.Handler;

namespace Gatekeeper.LdapServerLibrary.Engine
{
    internal class HandlerMapper
    {
        private Dictionary<Type, Type> HandlerMapperCache = new Dictionary<Type, Type>();

        public HandlerMapper()
        {
            PopulateHandlerMapper();
        }

        private void PopulateHandlerMapper()
        {
            IEnumerable<Type> types = from t in Assembly.GetExecutingAssembly().GetTypes()
                                      where t.GetInterfaces().Any(i =>
                                        i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<>)
                                      )
                                      select t;

            types.ToList().ForEach(t =>
            {
                Type operationType = t.GetInterfaces()[0].GenericTypeArguments[0];

                HandlerMapperCache.Add(operationType, t);
            });
        }

        internal Type GetHandlerForType(Type t)
        {
            return HandlerMapperCache.Single(x => x.Key == t).Value;
        }
    }
}