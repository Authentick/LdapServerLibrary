using System.Formats.Asn1;

namespace Gatekeeper.LdapServerLibrary.Models.Operations
{
    internal interface IProtocolOp
    {
        internal int GetTag();
    }
}
