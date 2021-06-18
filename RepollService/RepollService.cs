using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Runtime.InteropServices;
using System.IO;
using System.ServiceModel;
using RepollInterfaces;

namespace RepollService
{
    public enum SimpleServiceCustomCommands
    { StopWorker = 128, RestartWorker, CheckWorker };
    public partial class RepollService : ServiceBase
    {
        private readonly string filePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Repoll\repos.json";
        private readonly string directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Repoll";
        public static List<Tuple<string,string>> repos = new List<Tuple<string,string>>();
        private int eventId = 1;
        private static ServiceStatus serviceStatus = new ServiceStatus();
        ServiceHost host;

        public RepollService(string[] args)
        {
            InitializeComponent();

            //Create Event Logger
            repollEventLog = new EventLog();
            if (!EventLog.SourceExists("MySource"))
            {
                EventLog.CreateEventSource(
                    "MySource", "MyNewLog");
            }
            repollEventLog.Source = "RepollSource";
            repollEventLog.Log = "RepollLog";
        }

        protected override void OnStart(string[] args)
        {
            //Change Service Status
            serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;

            //Create Repo Storage File if it doesn't exist and read in any saved data.
            try
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                if (!File.Exists(filePath))
                {
                    using (var file = File.Create(filePath)) { }
                }
                var temp = File.ReadAllText(filePath).ToObject<List<Tuple<string,string>>>();
                if (temp != null)
                {
                    repos = temp;
                }
            }
            catch (Exception e)
            {
                //Report any errors that occur to Event Viewer.
                repollEventLog.WriteEntry(e.Message, EventLogEntryType.Error, eventId++);
            }

            //Start Service Listener
            try
            {
                var uri = new Uri("net.tcp://localhost:6565/RepollService");
                var binding = new NetTcpBinding(SecurityMode.None);
                host = new ServiceHost(typeof(WCFRepollService), uri);
                host.AddServiceEndpoint(typeof(IWCFRepollService), binding, "");
                host.Open();
            }
            catch(Exception e)
            {
                repollEventLog.WriteEntry(e.Message, EventLogEntryType.Error, eventId++);
            }

            repollEventLog.WriteEntry("Server is Open", EventLogEntryType.Information, eventId++);
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

        }

        protected override void OnStop()
        {
            UpdateServiceState(ServiceState.SERVICE_STOP_PENDING, 100000);

            //Save to repos to file in JSON format.
            if (File.Exists(filePath))
            {
                try
                {
                    File.WriteAllText(filePath, repos.ToJsonString());
                }
                catch (Exception e)
                {
                    repollEventLog.WriteEntry(e.Message, EventLogEntryType.Error, eventId++);
                }
            }
            else
            {
                repollEventLog.WriteEntry("repos.json no longer exists", EventLogEntryType.Warning, eventId++);
            }

            //Close Service Listener
            host.Close();

            // Update the service state to Stopped.
            UpdateServiceState(ServiceState.SERVICE_STOPPED);
        }
        protected override void OnCustomCommand(int command)
        {
            repollEventLog.WriteEntry("Command Recieved", EventLogEntryType.Information, eventId++);
            switch (command)
            {
                case (int)SimpleServiceCustomCommands.RestartWorker:
                    repollEventLog.WriteEntry("RestartWorker", EventLogEntryType.Information, eventId++);
                    break;
                case (int)SimpleServiceCustomCommands.CheckWorker:
                    repollEventLog.WriteEntry("CheckWorker", EventLogEntryType.Information, eventId++);
                    break;
                default:
                    break;
            }
        }

        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(System.IntPtr handle, ref ServiceStatus serviceStatus);

        private void UpdateServiceState(ServiceState serviceState, int dwWaitHint = 30000)
        {
            serviceStatus.dwCurrentState = serviceState;
            serviceStatus.dwWaitHint = dwWaitHint;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        /* //Unused Methods:
        protected override void OnPause()
        {
            repollEventLog.WriteEntry("In OnContinue.");
        }
        protected override void OnContinue()
        {
            repollEventLog.WriteEntry("In OnContinue.");
        }
        protected override void OnShutdown()
        {
            repollEventLog.WriteEntry("In OnContinue.");
        }
*/
        //private void SetTimer()
        //{
        //    Timer timer = new Timer();
        //    timer.Interval = 60000; // 60 seconds
        //    timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
        //    timer.Start();
        //}

        //private void OnTimer(object sender, ElapsedEventArgs e)
        //{
        //    repollEventLog.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
        //}
    }
}
