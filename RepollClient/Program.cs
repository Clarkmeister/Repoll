using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.Diagnostics;
using System.Threading;

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
                Console.WriteLine("Status = " + sc.Status);
                Console.WriteLine("Can Pause and Continue = " + sc.CanPauseAndContinue);
                Console.WriteLine("Can ShutDown = " + sc.CanShutdown);
                Console.WriteLine("Can Stop = " + sc.CanStop);
                if (sc.Status == ServiceControllerStatus.Stopped)
                {
                    sc.Start();
                    while (sc.Status == ServiceControllerStatus.Stopped)
                    {
                        Thread.Sleep(1000);
                        sc.Refresh();
                    }
                    Console.WriteLine("Service Status = " + sc.Status);
                }
                sc.ExecuteCommand((int)SimpleServiceCustomCommands.StopWorker);
                sc.ExecuteCommand((int)SimpleServiceCustomCommands.RestartWorker);
                Console.WriteLine("executed commands");
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
