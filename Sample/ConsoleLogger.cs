using System;
using Gatekeeper.LdapServerLibrary;

namespace Sample
{
    class ConsoleLogger : ILogger
    {
        public void LogException(Exception e)
        {
            System.Console.WriteLine(e.ToString());
        }
    }
}