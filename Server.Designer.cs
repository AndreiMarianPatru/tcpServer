namespace tcpServer
{
    partial class Server
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tchat = new System.Windows.Forms.TextBox();
            this.mConsole = new System.Windows.Forms.TextBox();
            this.bSend = new System.Windows.Forms.Button();
            this.bStartServer = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tchat
            // 
            this.tchat.Location = new System.Drawing.Point(58, 43);
            this.tchat.Multiline = true;
            this.tchat.Name = "tchat";
            this.tchat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tchat.Size = new System.Drawing.Size(739, 525);
            this.tchat.TabIndex = 3;
            this.tchat.TextChanged += new System.EventHandler(this.tchat_TextChanged);
            // 
            // mConsole
            // 
            this.mConsole.Location = new System.Drawing.Point(58, 589);
            this.mConsole.Multiline = true;
            this.mConsole.Name = "mConsole";
            this.mConsole.Size = new System.Drawing.Size(739, 71);
            this.mConsole.TabIndex = 4;
            // 
            // bSend
            // 
            this.bSend.Location = new System.Drawing.Point(858, 589);
            this.bSend.Name = "bSend";
            this.bSend.Size = new System.Drawing.Size(324, 71);
            this.bSend.TabIndex = 5;
            this.bSend.Text = "Send!";
            this.bSend.UseVisualStyleBackColor = true;
            this.bSend.Click += new System.EventHandler(this.bSend_Click);
            // 
            // bStartServer
            // 
            this.bStartServer.Location = new System.Drawing.Point(858, 43);
            this.bStartServer.Name = "bStartServer";
            this.bStartServer.Size = new System.Drawing.Size(300, 64);
            this.bStartServer.TabIndex = 6;
            this.bStartServer.Text = "Start Server";
            this.bStartServer.UseVisualStyleBackColor = true;
            this.bStartServer.Click += new System.EventHandler(this.bStartServer_Click);
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(858, 202);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(300, 109);
            this.textBox1.TabIndex = 7;
            this.textBox1.Text = "/start server\r\n/stop server\r\n/list users\r\n/list rooms\r\n/createroom <id(int)> <nam" +
    "e(string)>\r\n";
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 692);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.bStartServer);
            this.Controls.Add(this.bSend);
            this.Controls.Add(this.mConsole);
            this.Controls.Add(this.tchat);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Server";
            this.Text = "Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.TextBox tchat;
        private System.Windows.Forms.TextBox mConsole;
        private System.Windows.Forms.Button bSend;
        private System.Windows.Forms.Button bStartServer;
        private System.Windows.Forms.TextBox textBox1;
    }
}