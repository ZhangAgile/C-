using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCPServer
{
    public partial class TCPServer : Form
    {
        private byte[] result = new byte[1024];
        private Socket server;
        private Thread listenThread;
        private Thread readThread;
        private Socket TmpSocket;
        private string ip;
        private int port;

        private static Dictionary<string, Socket> dicSocket = new Dictionary<string, Socket>();

        public static ManualResetEvent allDone = new ManualResetEvent(false);
        public TCPServer()
        {
            InitializeComponent();
            this.Text = "TCP服务器";
            this.txtIP.Text = "127.0.0.1";
            this.txtPort.Text = "5000";
            this.txtPort.ReadOnly = this.txtIP.ReadOnly = true;
        }

        private void btnSwitch_Click(object sender, EventArgs e)
        {
            ip = this.txtIP.Text;
            port = Convert.ToInt32(this.txtPort.Text);
            IPAddress address = IPAddress.Parse(ip);
            IPEndPoint endPoint = new IPEndPoint(address, port);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //SocketAsyncEventArgs
            try
            {
                server.Bind(endPoint);//bind
                server.Listen(10);
            }
            catch (Exception ex)
            {
                this.ShowLog(ex.Message);
            }
            //ThreadPool.QueueUserWorkItem(new WaitCallback(AcceptClient));
            this.ShowLog("服务器开始侦听...");
            listenThread = new Thread(AcceptClient);
            listenThread.Start();
            
        }

        private void AcceptClient()
        {
            //SocketAsyncEventArgs
            try
            {
                while (true)
                {
                    //allDone.Reset();

                    server.BeginAccept(new AsyncCallback(AcceptClientCallBack),null);
                    //allDone.WaitOne();

                    //TmpSocket = server.Accept();
                    //this.AcceptClientCallBack(null);
                }
            }
            catch (Exception e)
            {
                this.ShowLog(e.Message);
            }
        }
        private void AcceptClientCallBack(IAsyncResult result)
        {
            TmpSocket = server.EndAccept(result);
            dicSocket.Add(TmpSocket.RemoteEndPoint.ToString(), TmpSocket);
            //if (result != null)
            //{
            //    //接收到客户端请求之后,需要进行收发数据
            //    TmpSocket = this.server.EndAccept(result);
            //}
            string str = "Hello !";
            byte[] sendBytes = Encoding.ASCII.GetBytes(str);
            TmpSocket.Send(sendBytes);
            this.ShowLog("Accept:" + TmpSocket.RemoteEndPoint);
            readThread = new Thread(Read);
            readThread.Start(TmpSocket);
        }


        private void Read(object client)
        {
            Socket clientSocket = (Socket)client;
            if (clientSocket == null)
            {
                return;
            }
            while (true)
            {
                if (clientSocket.Connected)
                {
                    //clientSocket.BeginReceive(result, 0, result.Length, SocketFlags.None, new AsyncCallback(ReadCallBack), new object());
                    result = new byte[1024];
                    int length = clientSocket.Receive(result);
                    string str = string.Empty;
                    if (length != 0)
                        str = Encoding.ASCII.GetString(result);
                    ShowLog(str);
                }
            }
        }
        private void ReadCallBack(IAsyncResult result)
        {
            if (result == null)
                return;
            //TODO show Log
        }

        private void ShowLog(string str)
        {
            if (this.txtLog.InvokeRequired)
            {
                this.txtLog.Invoke(new Action(() =>
                {
                    this.txtLog.Text += "\r\n";
                    this.txtLog.Text += string.Format("{0}:{1}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), str).ToString();
                }));
            }
            else
            {
                ShowLogText(str);
            }
        }
        private void ShowLogText(string str)
        {
            if (string.IsNullOrWhiteSpace(this.txtLog.Text))
            {
                this.txtLog.Text += string.Format("{0}:{1}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), str).ToString();
            }
            else
            {
                this.txtLog.Text += "\r\n";
                this.txtLog.Text += string.Format("{0}:{1}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), str).ToString();
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            try
            {
                if (this.listenThread.IsAlive)
                {
                    this.listenThread.Abort();
                }
                if (this.readThread.IsAlive)
                {
                    this.readThread.Abort();
                }
            }
            catch
            { }
            finally
            {
                this.server.Dispose();
                this.server.Close();
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string str = this.txtWrite.Text;
            result = Encoding.ASCII.GetBytes(str);
            this.TmpSocket.Send(result);
        }

    }
}
