using System.Formats.Asn1;

namespace LdapServer.Models.Operations
{
    internal interface IProtocolOp
    {
        internal int GetTag();
    }
}
