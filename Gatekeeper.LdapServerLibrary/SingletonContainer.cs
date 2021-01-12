using System.Security.Cryptography.X509Certificates;
using Gatekeeper.LdapServerLibrary.Engine;
using Gatekeeper.LdapServerLibrary.Parser;

namespace Gatekeeper.LdapServerLibrary
{
    internal class SingletonContainer
    {
        private static LdapEvents LdapEventListener = new LdapEvents();
        private static OperationMapper OperationMapper = new OperationMapper();
        private static HandlerMapper HandlerMapper = new HandlerMapper();
        private static ILogger? Logger;
        private static X509Certificate2? Certificate;

        static internal void SetLogger(ILogger logger)
        {
            Logger = logger;
        }

        static internal ILogger? GetLogger()
        {
            return Logger;
        }

        static internal void SetLdapEventListener(LdapEvents listener)
        {
            LdapEventListener = listener;
        }

        static internal LdapEvents GetLdapEventListener()
        {
            return LdapEventListener;
        }

        static internal void SetCertificate(X509Certificate2 certificate)
        {
            Certificate = certificate;
        }

        static internal X509Certificate? GetCertificate()
        {
            return Certificate;
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
