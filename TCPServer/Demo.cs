using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCPServer
{
    public class Example
    {
        static ManualResetEvent clientDone = new ManualResetEvent(false);

        public static void Demo(System.Windows.Forms.Control outputBlock)
        {

            SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
            DnsEndPoint hostEntry = new DnsEndPoint("http://www.contoso.com", 80);

            // Create a socket and connect to the server
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(SocketEventArg_Completed);

            socketEventArg.RemoteEndPoint = hostEntry;
            socketEventArg.UserToken = sock;
            sock.ConnectAsync(socketEventArg);
            clientDone.WaitOne();
        }

        // A single callback is used for all socket operations. 
        // This method forwards execution on to the correct handler 
        // based on the type of completed operation
        static void SocketEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    ProcessConnect(e);
                    break;
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;
                default:
                    throw new Exception("Invalid operation completed");
            }
        }

        // Called when a ConnectAsync operation completes
        private static void ProcessConnect(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                // Successfully connected to the server

                // Send 'Hello World' to the server
                byte[] buffer = Encoding.UTF8.GetBytes("Hello World");
                e.SetBuffer(buffer, 0, buffer.Length);
                Socket sock = e.UserToken as Socket;
                bool willRaiseEvent = sock.SendAsync(e);
                if (!willRaiseEvent)
                {
                    ProcessSend(e);
                }
            }
            else
            {
                throw new SocketException((int)e.SocketError);
            }
        }

        // Called when a ReceiveAsync operation completes
        // </summary>
        private static void ProcessReceive(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                // Received data from server

                // Data has now been sent and received from the server. 
                // Disconnect from the server
                Socket sock = e.UserToken as Socket;
                sock.Shutdown(SocketShutdown.Send);
                sock.Close();
                clientDone.Set();
            }
            else
            {
                throw new SocketException((int)e.SocketError);
            }
        }


        // Called when a SendAsync operation completes
        private static void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                // Sent "Hello World" to the server successfully

                //Read data sent from the server
                Socket sock = e.UserToken as Socket;
                bool willRaiseEvent = sock.ReceiveAsync(e);
                if (!willRaiseEvent)
                {
                    ProcessReceive(e);
                }
            }
            else
            {
                throw new SocketException((int)e.SocketError);
            }
        }
    }
}
