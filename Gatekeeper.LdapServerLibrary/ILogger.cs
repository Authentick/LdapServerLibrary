using System;

namespace Gatekeeper.LdapServerLibrary {
    public interface ILogger 
    {
        void LogException(Exception e);    
    }
}
