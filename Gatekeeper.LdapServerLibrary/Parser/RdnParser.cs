using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("Gatekeeper.LdapServerLibrary.Tests")]
namespace Gatekeeper.LdapServerLibrary.Parser
{
    internal class RdnParser
    {
        internal static Dictionary<string, List<string>> ParseRdnString(string rdn)
        {
            string[] rdnAttributePairs = rdn.Split(',');

            Dictionary<string, List<string>> parsedRdn = new Dictionary<string, List<string>>();

            foreach (string rdnAttribute in rdnAttributePairs)
            {
                string[] rdnAttributePair = rdnAttribute.Split('=');
                if (rdnAttributePair.Length == 2)
                {
                    if (parsedRdn.ContainsKey(rdnAttributePair[0]))
                    {
                        parsedRdn[rdnAttributePair[0]].Add(rdnAttributePair[1]);
                    }
                    else
                    {
                        parsedRdn.Add(rdnAttributePair[0], new List<string> { rdnAttributePair[1] });
                    }
                }
            }

            return parsedRdn;
        }
    }
}
