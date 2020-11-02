using LdapServer.Engine;
using LdapServer.Parser;

namespace LdapServer
{
    internal class SingletonContainer
    {
        private static LdapEvents LdapEventListener = new LdapEvents();
        private static OperationMapper OperationMapper = new OperationMapper();
        private static HandlerMapper HandlerMapper = new HandlerMapper();

        static internal void SetLdapEventListener(LdapEvents listener)
        {
            LdapEventListener = listener;
        }

        static internal LdapEvents GetLdapEventListener()
        {
            return LdapEventListener;
        }

        static internal OperationMapper GetOperationMapper()
        {
            return OperationMapper;
        }

        static internal HandlerMapper GetHandlerMapper()
        {
            return HandlerMapper;
        }
    }
}