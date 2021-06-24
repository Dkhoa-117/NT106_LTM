
namespace test
{
    partial class Bai_04
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnTCPClient = new System.Windows.Forms.Button();
            this.btnTCPServer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnTCPClient
            // 
            this.btnTCPClient.Location = new System.Drawing.Point(12, 149);
            this.btnTCPClient.Name = "btnTCPClient";
            this.btnTCPClient.Size = new System.Drawing.Size(306, 89);
            this.btnTCPClient.TabIndex = 0;
            this.btnTCPClient.Text = "TCP Client";
            this.btnTCPClient.UseVisualStyleBackColor = true;
            this.btnTCPClient.Click += new System.EventHandler(this.btnTCPClient_Click);
            // 
            // btnTCPServer
            // 
            this.btnTCPServer.Location = new System.Drawing.Point(437, 149);
            this.btnTCPServer.Name = "btnTCPServer";
            this.btnTCPServer.Size = new System.Drawing.Size(306, 89);
            this.btnTCPServer.TabIndex = 1;
            this.btnTCPServer.Text = "TCP Server";
            this.btnTCPServer.UseVisualStyleBackColor = true;
            this.btnTCPServer.Click += new System.EventHandler(this.btnTCPServer_Click);
            // 
            // Bai_04
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 326);
            this.Controls.Add(this.btnTCPServer);
            this.Controls.Add(this.btnTCPClient);
            this.Name = "Bai_04";
            this.Text = "Lab03_Bai04";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnTCPClient;
        private System.Windows.Forms.Button btnTCPServer;
    }
}

