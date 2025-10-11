using HL7TCPtoHTTPS;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.ServiceProcess;
using System.Threading;

namespace HL7forwardTCPtoHTTPS
{
    public partial class TCPtoHTTPS : ServiceBase
    {
        // Listen on TCP port then Forward to given URL on HTTPS
        //private static readonly byte[] ReceiveToIP = { 127, 0, 0, 1 };
        private static readonly string ReceiveToIP = ConfigurationManager.AppSettings["ReceiveToIP"];
        private static int ReceiveToPort = Convert.ToInt32(ConfigurationManager.AppSettings["ReceiveToPort"]);
        private static readonly string ForwardToHost = ConfigurationManager.AppSettings["ForwardToHost"];
        private static readonly string ForwardToUser = ConfigurationManager.AppSettings["ForwardToUser"];
        private static readonly string ForwardToPassword = ConfigurationManager.AppSettings["ForwardToPassword"];
        private IPEndPoint ListenerEndPoint;
        private Subscriber subscriber;
        private bool RunProc = true;
        private bool IsStartUp = true;
        private int WaitMinutes = 2;
        Thread ProcThread;

        public TCPtoHTTPS()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            RunProc = true;
            IsStartUp = true;
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            try
            {
                EventViewerLog("OnStart:", "Service started");
                //MainProcess();
                //Start Thread
                ProcThread = new Thread(MainProcess);
                ProcThread.Start();
            }
            catch (Exception Ex)
            {
                RunProc = false;
                EventViewerLog("Ex[OnStart]", Ex.Message + "\n" + Ex.StackTrace);
            }
        }

        protected override void OnStop()
        {
            try
            {
                if(subscriber!=null)
                    subscriber.Stop();
                RunProc = false;
                EventViewerLog("OnStop:", "Service stopped");
                ProcThread.Abort();
            }
            catch { }
        }

        void MainProcess()
        {
            try
            {
                if (IsStartUp)
                {
                    // To wait till Windows Native services running.
                    //Thread.Sleep(TimeSpan.FromMinutes(WaitMinutes));
                    IsStartUp = false;
                    ListenerEndPoint = new IPEndPoint(IPAddress.Parse(ReceiveToIP), ReceiveToPort);
                    //ListenerEndPoint = new IPEndPoint(new IPAddress(ReceiveToIP), ReceiveToPort);

                    EventViewerLog("MainProcess:", "Service started");
                }
                // PUT PROCESS IN THREAD -START ================================

                // Create a thread for listening to a port.
                subscriber = new Subscriber(ListenerEndPoint, ForwardToHost, ForwardToUser, ForwardToPassword);
                subscriber.Listen();

            }
            catch (Exception Ex)
            {
                if (subscriber != null)
                {
                    subscriber.Stop();
                    //subscriber.Dispose();
                }
                EventViewerLog("Ex[MainProcess]:", Ex.Message + "\n" + Ex.StackTrace);
                Thread.Sleep(TimeSpan.FromMinutes(WaitMinutes));
                if (RunProc)
                    MainProcess();
            }
        }


        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                if (subscriber != null)
                {
                    subscriber.Stop();
                    //subscriber.Dispose();
                }
                Exception Ex = (Exception)e.ExceptionObject;
                EventViewerLog("Ex[Unhandled]:", string.Format("{0}\n{1}\n{2}", Ex.GetType().FullName, Ex.Message, ((Ex.StackTrace != null) ? Ex.StackTrace : "")));
            }
            catch { }
        }

        public void EventViewerLog(string From, string Msg)
        {
            //Entry in Event Viewer
            EventLog.WriteEntry("HL7endpointTCPlistener", string.Format("{0} {1}",From, Msg), 
                                From.Contains("Ex[")? EventLogEntryType.Error : EventLogEntryType.Information);
        }
    }
}
