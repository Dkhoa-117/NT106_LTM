using System;
using System.IO;
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
            if (tcpClient.Connected)
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
        { }
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
            Byte[] ReceiveBytes = new Byte[1];
            NetworkStream networkStream = tcpClient.GetStream();
            NetworkStream readStream = networkStream;
            string Message = null;
            do
            {
                readStream.Read(ReceiveBytes, 0, ReceiveBytes.Length);
                Message += Encoding.ASCII.GetString(ReceiveBytes);
            } while (Message[Message.Length - 1] != 0);

            rtxbDataReceive.Text = Message;
        }

        //**Gui Du Lieu**
        public void Send()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.ShowDialog();
            FileStream fs;

            if (sfd.FileName != "")
            {
                fs = new FileStream(sfd.FileName, FileMode.Create);
            }
            else
            {
                MessageBox.Show("Chon File!", "WARNING", MessageBoxButtons.OK);
                return;
            }
            using (StreamWriter sw = new StreamWriter(fs))
            {
                string output = rtxbDataSend.Text + "\0";
                sw.Flush();
                sw.Write(output);
            }
            if (fs != null)
            {
                MessageBox.Show("Da Ghi Thanh Cong", "Thanh Cong", MessageBoxButtons.OK);
            }
            fs.Close();
            tcpClient.Client.SendFile(sfd.FileName);
        }

        //**Thiet Lap Ket Noi**
        public void Connect()
        {
            tcpClient.Connect(IPAddress.Parse("127.0.0.1"), 4040);
            readStream = tcpClient.GetStream();
            writeStream = tcpClient.GetStream();
            //Receive();
            if (txbUserName.Text.Length > 0)
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
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.ShowDialog();
            FileStream fs;
            if (sfd.FileName == "")
            {
                MessageBox.Show("Chon file", "WARNING", MessageBoxButtons.OK);
                return;
            }
            else
            {
                fs = new FileStream(sfd.FileName, FileMode.Create);
            }

            using (StreamWriter sw = new StreamWriter(fs))
            {
                string output = rtxbDataReceive.Text;
                sw.Flush();
                sw.Write(output);
            }
            fs.Close();
            lsbSaveFile.Items.Add(sfd.FileName);
        }

        //**Xoa noi dung tren textbox**
        private void btnClear_Click(object sender, EventArgs e)
        {
            switch (MessageBox.Show("Xoa tat ca", "WARNING", MessageBoxButtons.YesNo))
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
            if (lsbSaveFile.SelectedItem == null)
            {
                try
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.ShowDialog();
                    FileStream fs = new FileStream(ofd.FileName, FileMode.OpenOrCreate);
                    StreamReader sr = new StreamReader(fs);
                    string content = sr.ReadToEnd();
                    rtxbDataSend.Text = content;
                    fs.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Không thể mở thư mục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                string curItem = lsbSaveFile.SelectedItem.ToString();
                try
                {

                    FileStream fs = new FileStream(curItem, FileMode.Open);
                    StreamReader sr = new StreamReader(fs);
                    string content = sr.ReadToEnd();
                    rtxbDataSend.Text = content;
                    fs.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Không thể mở thư mục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        //**Xoa file da luu**
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lsbSaveFile.SelectedItem == null)
            {
                MessageBox.Show("Chọn file để xoá", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                string filename = lsbSaveFile.SelectedItem.ToString();
                File.Delete(filename);
                lsbSaveFile.Items.Remove(filename);
            }
        }
    }
}

