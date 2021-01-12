using System.Threading;

namespace Sample.Tests.Integration
{
    public class LdapServerFixture
    {
        public LdapServerFixture()
        {
            StartServer();
        }

        private void StartServer()
        {
            Sample.Program program = new Sample.Program();
            new Thread(async () =>
            {
                Thread.CurrentThread.IsBackground = true;
                await Sample.Program.Main(new string[0]);
            }).Start();
            Thread.Sleep(1000);
        }
    }
}
