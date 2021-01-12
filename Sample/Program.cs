using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Gatekeeper.LdapServerLibrary;

namespace Sample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            LdapServer server = new LdapServer
            {
                Port = 3389,
            };
            server.RegisterEventListener(new LdapEventListener());
            server.RegisterLogger(new ConsoleLogger());
            server.RegisterCertificate(new X509Certificate2(GetTlsCertificatePath()));
            await server.Start();
        }

        private static string GetTlsCertificatePath()
        {
            var certificateStream = System.Reflection.Assembly.GetAssembly(typeof(Sample.Program)).GetManifestResourceStream("Sample.example_certificate.pfx");
            string path = Path.GetTempFileName();
            var fileStream = File.Create(path);
            certificateStream.Seek(0, SeekOrigin.Begin);
            certificateStream.CopyTo(fileStream);
            fileStream.Close();
            return path;
        }
    }
}
