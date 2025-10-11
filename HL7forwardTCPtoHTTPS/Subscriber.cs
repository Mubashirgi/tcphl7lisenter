using RestSharp;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HL7TCPtoHTTPS
{
    public class Subscriber : IDisposable
    {
        private Socket listener, receiver;
        private RestClient client;
        private bool KeepListening = true;
        private string ForwardToHost;
        public Subscriber(IPEndPoint endPoint, string ForwardToHost, string ForwardToUser, string ForwardToPassword)
        {
            client = new RestClient();
            client.Authenticator = new HttpBasicAuthenticator(ForwardToUser, ForwardToPassword);
            this.ForwardToHost = ForwardToHost;

            //request = new RestRequest(ForwardToHost, Method.POST);
            //request.AddHeader("Content-Type", "text/plain");

            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(endPoint);
            EventViewerLog("", string.Format("Listening to port {0}", endPoint));
            //Console.WriteLine("Listening to port {0}", endPoint);
            listener.Listen(3);
        }
        public void Listen()
        {
            // Declare your variables.
            // Do not declare variables inside loops like for, foreach, while etc.
            // Because with every iteration, a new variable will be created.
            // If your loop iterates 1000 times, you will end up creating 1000 variables instead of just one variable.
            byte[] buffer;
            int count;
            string data;
            string tempData;
            RestResponse response;
            int start;
            int end;
            StringBuilder frame;
            try
            {
                // true here make sure that the thread keep listening to the port.
                while (KeepListening)
                {
                    buffer = new byte[4096];

                    // Take care of incoming connection ...
                    receiver = listener.Accept();
                    //Console.WriteLine("Taking care of incoming connection.");
                    // Handle the message if one is received.
                    while (true)
                    {
                        try
                        {
                            count = receiver.Receive(buffer);
                            data = Encoding.UTF8.GetString(buffer, 0, count);

                            // Search for a Vertical Tab (VT) character to find start of MLLP frame.
                            start = data.IndexOf((char)0x0b);
                            if (start >= 0)
                            {
                                // Search for a File Separator (FS) character to find the end of the frame.
                                end = data.IndexOf((char)0x1c);
                                if (end > start)
                                {
                                    // Remove the MLLP charachters
                                    //tempData = Encoding.UTF8.GetString(buffer, 4, count - 12);
                                    tempData = Encoding.UTF8.GetString(buffer, 1, count - 1).Trim();

                                    // Forward Msg then acknowledge
                                    response = Forward(tempData);
                                    if (response != null)
                                    {
                                        if (response.StatusCode == HttpStatusCode.OK)
                                        {
                                            //Console.WriteLine("HL7 message forwarded.");
                                            frame = new StringBuilder();
                                            frame.Append((char)0x0b); // Vertical Tab (VT) character
                                            frame.Append(response.Content);
                                            frame.Append((char)0x1c); // File Separator (FS) character
                                            frame.Append((char)0x0d); // Carriage Return (CR) character
                                            receiver.Send(Encoding.UTF8.GetBytes(frame.ToString()));
                                        }
                                        else if (response.StatusCode == HttpStatusCode.Unauthorized)
                                        {
                                            EventViewerLog("Ex[Auth]:", "401 Unauthorised");
                                            //Console.WriteLine("Error: 401 Unauthorised");
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            EventViewerLog("Ex[Listen_While]:", ex.Message + "\n" + ex.StackTrace);
                        }
                    }

                    // close connection
                    receiver.Shutdown(SocketShutdown.Both);
                    receiver.Close();
                    //receiver.Dispose();

                    //GC.Collect(); // Garbage Collection
                    //Console.WriteLine("Connection closed.");
                }
                //EventViewerLog("", string.Format("KeepListening: {0}", KeepListening));
            }
            catch (Exception ex)
            {
                // Exception handling
                EventViewerLog("Ex[Listen]:", ex.Message + "\n" + ex.StackTrace);
                throw ex;
                //Console.WriteLine(string.Format("Error(Listen): {0}", ex.Message));
            }
        }
       
        public RestResponse Forward(string hl7Data)
        {
            try
            {
                RestRequest request = new RestRequest(this.ForwardToHost, Method.POST);
                request.AddHeader("Content-Type", "text/plain");
                request.AddParameter("text/plain", hl7Data, ParameterType.RequestBody);
                return (RestResponse)client.Execute(request);
            }
            catch (Exception ex)
            {
                // Exception handling
                EventViewerLog("Ex[Forward]:", ex.Message + "\n" + ex.StackTrace);
                return null;
            }
        }

        public void Stop()
        {
            KeepListening = false;
            
            listener.Shutdown(SocketShutdown.Both);
            listener.Close();
            //listener.Disconnect(true);
        }

        public void EventViewerLog(string From, string Msg)
        {
            //Entry in Event Viewer
            EventLog.WriteEntry("HL7endpointTCPlistener", string.Format("{0} {1}", From, Msg),
                                From.Contains("Ex[") ? EventLogEntryType.Error : EventLogEntryType.Information);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose managed resources
                if (listener != null)
                {
                    listener.Close();
                    listener.Dispose();
                }
                if (receiver != null)
                {
                    receiver.Close();
                    receiver.Dispose();
                }
            }
        }
    }
}
