using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class Bai_04 : Form
    {
        public Bai_04()
        {
            InitializeComponent();
        }

        private void btnTCPClient_Click(object sender, EventArgs e)
        {
            var TCP_Client = new TCP_Client();
            TCP_Client.Show();
        }

        private void btnTCPServer_Click(object sender, EventArgs e)
        {
            var TCP_Server = new TCP_Server();
            TCP_Server.Show();
        }
    }
}
