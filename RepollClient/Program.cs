using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.ServiceModel;
using RepollInterfaces;

namespace RepollClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string uri = "net.tcp://localhost:6565/RepollService";
            var endpoint = new EndpointAddress(uri);

            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            ChannelFactory<IWCFRepollService> channelFactory = new ChannelFactory<IWCFRepollService>(binding);
            IWCFRepollService proxy = channelFactory.CreateChannel(endpoint);

            var products = proxy.ListProducts();

            foreach (var item in products)
            {
                Console.WriteLine(item);
            }
            Console.ReadLine();
        }
    }
}
