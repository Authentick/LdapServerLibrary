using LdapServer.Models.Operations;

namespace LdapServer.Parser.Encoder
{
    internal interface IApplicationEncoder<T> where T : IProtocolOp
    {
        byte[] TryEncode(T response);
    }
}
