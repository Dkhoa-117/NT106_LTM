using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace test
{
    public partial class TCP_Client : Form
    {
        TcpClient tcpClient = new TcpClient();
        NetworkStream readStream;
        NetworkStream writeStream;
        Thread _Thread;
        string UserName = "Anonymous\n";
        public TCP_Client()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if(tcpClient.Connected)
                Send();
            else
            {
                MessageBox.Show("Khong co ket noi", "WARNING", MessageBoxButtons.OK);
                return;
            }
            rtxbDataSend.Clear();
        }
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            Disconnect();
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            rtxbDataSend.Enabled = true;
            CheckForIllegalCrossThreadCalls = false;
            
            _Thread = new Thread(new ThreadStart(ClientThread));
            _Thread.Start();

            //Giau txbUsername
            txbUserName.Enabled = false;

            //Giau nut Start
            btnStart.Enabled = false;
            btnStart.Visible = false;

            //Hien thi nut Disconnect
            btnDisconnect.Enabled = true;
            btnDisconnect.Visible = true;
        }

        private void TCP_Client_Load(object sender, EventArgs e)
        {
            rtxbDataSend.Enabled = false;
            btnDisconnect.Visible = false;
            btnDisconnect.Enabled = false;
        }

        //**Gui khi nhan phim Enter**
        private void rtxbMessage_KeyUp(object sender, KeyEventArgs e)
        {
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        Byte[] buffer = Encoding.UTF8.GetBytes($"{rtxbDataSend.Text}");
        //        if (buffer.Length < 1)
        //        {
        //            MessageBox.Show("Hay Nhap Van Ban! ", "WARNING", MessageBoxButtons.OK);
        //            return;
        //        }
        //        writeStream.Write(buffer, 0, buffer.Length);
        //        writeStream.Flush();
        //    }
        }
        public void ClientThread()
        {
            Connect();
            while (tcpClient.Connected)
            {
                Receive();
            }
        }

        //**Nhan Du Lieu Tu Server**
        public void Receive()
        {
            Byte[] buffer = new Byte[1];
            string Message = "";
            do
            {
                try
                {
                    readStream.Read(buffer, 0, buffer.Length);
                }
                catch
                {
                    MessageBox.Show("Mat Ket Noi", "WARING", MessageBoxButtons.OK);
                    break;
                }
                Message += Encoding.UTF8.GetString(buffer);
                    
            } while (Message[Message.Length - 1] != 10);
            rtxbDataReceive.Text = Message;
        }

        //**Gui Du Lieu**
        public void Send()
        {
            Byte[] buffer = Encoding.UTF8.GetBytes($"{rtxbDataSend.Text}\n");
            if (buffer.Length < 1)
            {
                MessageBox.Show("Hay Nhap Van Ban! ", "WARNING", MessageBoxButtons.OK);
                return;
            }
            writeStream.Write(buffer, 0, buffer.Length);
        }

        //**Thiet Lap Ket Noi**
        public void Connect()
        {
            tcpClient.Connect(IPAddress.Parse("127.0.0.1"), 4080);
            readStream = tcpClient.GetStream();
            writeStream = tcpClient.GetStream();
            //Receive();
            if(txbUserName.Text.Length > 0)
            {
                UserName = txbUserName.Text + "\n";
            }
            Byte[] send = Encoding.UTF8.GetBytes(UserName);
            writeStream.Write(send, 0, send.Length);
        }

        //**Dong Ket Noi**
        public void Disconnect()
        {
            //thong bao dong ket noi cho Server
            Byte[] buffer = Encoding.UTF8.GetBytes($"*{UserName.Remove(UserName.Length - 1)} disconnected*\n");
            writeStream.Write(buffer, 0, buffer.Length);
            writeStream.Flush();

            readStream.Close();
            writeStream.Close();
            tcpClient.Close();
            btnDisconnect.Enabled = false;
        }

        //**Luu tru du lieu da duoc chinh sua tu Server**
        //Dua thong tin file da luu qua lsbSaveFfile
        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        //**Xoa noi dung tren textbox**
        private void btnClear_Click(object sender, EventArgs e)
        {
            switch(MessageBox.Show("Xoa tat ca", "WARNING", MessageBoxButtons.YesNo))
            {
                case DialogResult.Yes:
                    rtxbDataReceive.Clear();
                    rtxbDataSend.Clear();
                    break;
                case DialogResult.No:
                    break;
            }
            

        }

        //**Mo file da luu**
        private void btnOpen_Click(object sender, EventArgs e)
        {

        }


        //**Xoa file da luu**
        private void btnDelete_Click(object sender, EventArgs e)
        {

        }
    }
}

