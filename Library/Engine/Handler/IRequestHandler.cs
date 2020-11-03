using System.Threading.Tasks;
using LdapServer.Models.Operations;

namespace LdapServer.Engine.Handler
{
    internal interface IRequestHandler<T> where T : IProtocolOp
    {
        internal Task<HandlerReply> Handle(ClientContext context, LdapEvents eventListener, T operation);
    }
}
