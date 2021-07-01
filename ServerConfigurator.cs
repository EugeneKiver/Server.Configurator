using System;
// ServerConfig
using System.Configuration;
using System.Collections.Specialized;

using System.Net;

namespace Server.Configurator
{
    public class ServerConfigurator
    {
        //NameValueCollection appSettings = ConfigurationManager.AppSettings;

        // Singleton header start
        private ServerConfigurator() { }

        private static ServerConfigurator _instance;
        private NameValueCollection appSettings;

        public IPAddress IpAddress { get; private set; } = IPAddress.Loopback;
        public int Port { get; private set; } = 11000;
        private bool loopback = false;

        public static ServerConfigurator GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ServerConfigurator();
                _instance.Configure();

            }
            return _instance;
        }

        private bool Configure()
        {
            appSettings = ConfigurationManager.AppSettings;

            /*foreach (string s in appSettings.AllKeys)
                Console.WriteLine("Key: " + s + " Value: " + appSettings.Get(s));
            Console.ReadLine();*/

            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IpAddress = ipHostInfo.AddressList[2]; // TODO fix auto address

            // Override local address with Loopback if configured
            if (Boolean.TryParse((appSettings.Get("ip_loopback")), out loopback))
            {
                Console.WriteLine("Loopback - ON");
                IpAddress = IPAddress.Loopback;
            }

            // IPv4
            int port = 0; // TODO
            if (!Int32.TryParse((appSettings.Get("ip_port")), out port))
            {
                Console.WriteLine("No valid ip_port provided in App.config");
                return false;
            }
            Port = port; // TODO
            return true;
        }

        /*public NameValueCollection GetSettings()
        {
            return appSettings;
        }*/

    }
}
