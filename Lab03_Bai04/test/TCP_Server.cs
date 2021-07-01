using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace test
{
    public partial class TCP_Server : Form
    {
        TcpClient server = new TcpClient();
        public class Client
        {
            public string UserName;
            public TcpClient tcpClient;
            public Client()
            {
                UserName = "";
                tcpClient = new TcpClient();
            }
        }
        List<Client> MembersList = new List<Client>();

        public TCP_Server()
        {
            InitializeComponent();
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            Thread _ServerThread = new Thread(new ThreadStart(Connect));
            _ServerThread.Start();
            btnListen.Enabled = false;
        }

        //**Thiet Lap Ket Noi**
        public void Connect()
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, 4040);
            server.Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Client.Bind(ip);
            server.Client.Listen(10);
            while (true)
            {
                Client client = new Client();
                client.tcpClient.Client = server.Client.Accept();

                NetworkStream Read = client.tcpClient.GetStream();
                Byte[] BytesName = new Byte[1];
                do
                {
                    Read.Read(BytesName, 0, BytesName.Length);
                    client.UserName += Encoding.UTF8.GetString(BytesName);
                } while (client.UserName[client.UserName.Length - 1] != 10);

                MembersList.Add(client);
                lsbMembers.Items.Insert(0, $"New Connect from {client.tcpClient.Client.RemoteEndPoint} || {client.UserName}");

                //Send Thread 
                Thread _thread = new Thread(Send);
                _thread.Start(client);
            }
        }

        //**Gui thong tin**
        public void Send(object o)
        {
            Client client = (Client)o;
            while (client.tcpClient.Connected)
            {
                Byte[] sendBytes = new Byte[1];

                try
                {
                    Receive(ref sendBytes, client);
                    //nap sendBytes vao 1 ham xu ly du lieu
                    sendBytes = FixData(ref sendBytes);

                    //**gui tra du lieu cho Client**
                    NetworkStream writeStream = client.tcpClient.GetStream();
                    if (sendBytes[0] == 0)
                        break;
                    writeStream.Write(sendBytes, 0, sendBytes.Length);
                    writeStream.Flush();
                }
                catch
                {
                    client.tcpClient.Close();

                    //**Cap Nhat Lai MemberList**
                    MembersList.Remove(client);
                    lsbMembers.Items.Clear();
                    foreach (var Member in MembersList)
                    {
                        lsbMembers.Items.Add(Member.tcpClient.Client.RemoteEndPoint + " || " + Member.UserName);
                    }
                    break;
                }

            }
        }

        //**Nhan thong tin**
        public void Receive(ref Byte[] ReceiveBytes, Client client)
        {
            NetworkStream networkStream = client.tcpClient.GetStream();
            NetworkStream readStream = networkStream;
            string Message = null;
            do
            {
                readStream.Read(ReceiveBytes, 0, ReceiveBytes.Length);
                Message += Encoding.ASCII.GetString(ReceiveBytes);
            } while (Message[Message.Length - 1] != 0);

            ReceiveBytes = Encoding.UTF8.GetBytes(Message);
        }

        //**Ham xu ly du lieu**
        public Byte[] FixData(ref Byte[] sendBytes)
        {
            string Data = Encoding.UTF8.GetString(sendBytes);
            string temp = RightPlace(ref Data);
            Data = CapitalizeFirst(ref temp);
            return Encoding.UTF8.GetBytes(Data + "\0");
        }

        //Dat dung cho
        public string RightPlace(ref string Data)
        {
            StringBuilder sb = new StringBuilder(Data = Data.Trim());
            if (!(Data[Data.Length - 1] == '.' || Data[Data.Length - 1] == '\0' || Data[Data.Length - 1] == '?' || Data[Data.Length - 1] == '!') || Data[Data.Length - 1] == ',' || Data[Data.Length - 1] == ':')
            {
                sb.Append(".");
            }

            sb.Replace(",", ",$$$").Replace(".", ".$$$").Replace("!", "!$$$").Replace("?", "?$$$").Replace(":", ":$$$").Replace(";", ";$$$").Replace("…", "…$$$").Replace("\r\n", "\r\n$$$").Replace("\r", "\r$$$").Replace("\n", "\n$$$").Replace("\0", "");
            string[] sentences = sb.ToString().Split("$$$", StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb1 = new StringBuilder();
            for (int i = 0; i < sentences.Length; i++)
            {
                string temp = sentences[i];

                char punc = temp[temp.Length - 1];
                temp = temp.TrimEnd(punc);

                sb1.Append(temp.Trim());
                if (punc == '\n' || punc == '\r')
                {
                    sb1.Append(punc);
                }
                else
                    sb1.AppendFormat("{0} ", punc);
            }
            return sb1.ToString().Trim();
        }
        //Viet hoa
        public string CapitalizeFirst(ref string s)
        {
            bool IsNewSentense = true;
            var result = new StringBuilder(s.Length);
            for (int i = 0; i < s.Length; i++)
            {
                if (IsNewSentense && char.IsLetter(s[i]))
                {
                    result.Append(char.ToUpper(s[i]));
                    IsNewSentense = false;
                }
                else
                    result.Append(s[i]);

                if (s[i] == '!' || s[i] == '?' || s[i] == '.' || s[i] == '\n')
                {
                    IsNewSentense = true;
                }
            }
            return result.ToString();
        }
    }
}
