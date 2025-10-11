using System.ServiceProcess;

namespace HL7forwardTCPtoHTTPS
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /*  open CMD in administrator mode
            cd C:\Windows\Microsoft.NET\Framework64\v4.0.30319
            installutil "path\to\your\build\directory\HL7endpointTCPlistener.exe"
            installutil /u "path\to\your\build\directory\HL7endpointTCPlistener.exe"
        */
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new TCPtoHTTPS()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
