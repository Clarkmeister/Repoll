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

namespace RepollClient
{
    class Program
    {
        public enum SimpleServiceCustomCommands
        { StopWorker = 128, RestartWorker, CheckWorker };
        static void Main(string[] args)
        {
            try
            {
                ServiceController[] scServices = ServiceController.GetServices();
                ServiceController sc = null;
                foreach (ServiceController scTemp in scServices)
                {
                    if (scTemp.ServiceName == "RepollService")
                    {
                        sc = scTemp;
                    }
                }
                if (sc.Status == ServiceControllerStatus.Stopped)
                {
                    /*
                     Need to add way to get directories.
                     */
                    sc.Start(new string[] { "Hello", "From", "Client" });
                    while (sc.Status == ServiceControllerStatus.Stopped)
                    {
                        Thread.Sleep(1000);
                        sc.Refresh();
                    }
                    Console.WriteLine("Service Status = " + sc.Status);
                }
                else if (sc.Status == ServiceControllerStatus.Running)
                {
                    sc.Stop();
                    while (sc.Status != ServiceControllerStatus.Stopped)
                    {
                        Thread.Sleep(1000);
                        sc.Refresh();
                    }
                    Console.WriteLine("Service Status = " + sc.Status);
                }
                
                Console.WriteLine("Press anything to exit...");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }

        }
    }
}
