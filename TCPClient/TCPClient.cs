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

namespace TCPClient
{
    public partial class TCPClient : Form
    {
        
        private Thread readThread;
        private Socket socket;
        private byte[] result = new byte[1024];
        public TCPClient()
        {
            InitializeComponent();
            this.txtIP.Text = "127.0.0.1";
            this.txtPort.Text = "5000";
            this.txtIP.ReadOnly = this.txtPort.ReadOnly = true;
        }

        private void btnSwitch_Click(object sender, EventArgs e)
        {
            string ip = this.txtIP.Text;
            int port = Convert.ToInt32(this.txtPort.Text);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.BeginConnect(new IPEndPoint(IPAddress.Parse(ip), port), new AsyncCallback(ConnectCallback), null);
            ShowLog("请求连接服务器!");
        }

        private void ConnectCallback(object state)
        {
            readThread = new Thread(Read);
            readThread.Start(socket);
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
                //clientSocket.BeginReceive(result, 0, result.Length, SocketFlags.None, new AsyncCallback(ReadCallBack), new object());
                int length = clientSocket.Receive(result);
                string str = string.Empty;
                str = Encoding.ASCII.GetString(result);
                ShowLog(str);
            }
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
                if (readThread.IsAlive)
                {
                    readThread.Abort();
                }
            }
            catch(Exception ex) 
            {
                this.ShowLog(ex.Message);
            }
            finally 
            {
                socket.Disconnect(false);
                socket.Dispose();
                socket.Close();
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string str = this.txtWrite.Text;
            result = Encoding.ASCII.GetBytes(str);
            this.socket.Send(result);
        }

    }
}
