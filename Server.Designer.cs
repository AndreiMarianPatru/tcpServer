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
            this.bStartServer = new System.Windows.Forms.Button();
            this.bStopServer = new System.Windows.Forms.Button();
            this.tchat = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // bStartServer
            // 
            this.bStartServer.Location = new System.Drawing.Point(858, 57);
            this.bStartServer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bStartServer.Name = "bStartServer";
            this.bStartServer.Size = new System.Drawing.Size(324, 118);
            this.bStartServer.TabIndex = 0;
            this.bStartServer.Text = "Start Server";
            this.bStartServer.UseVisualStyleBackColor = true;
            this.bStartServer.Click += new System.EventHandler(this.button1_Click);
            // 
            // bStopServer
            // 
            this.bStopServer.Location = new System.Drawing.Point(858, 251);
            this.bStopServer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bStopServer.Name = "bStopServer";
            this.bStopServer.Size = new System.Drawing.Size(324, 117);
            this.bStopServer.TabIndex = 2;
            this.bStopServer.Text = "Stop Server";
            this.bStopServer.UseVisualStyleBackColor = true;
            this.bStopServer.Click += new System.EventHandler(this.bStopServer_Click);
            // 
            // tchat
            // 
            this.tchat.Location = new System.Drawing.Point(58, 43);
            this.tchat.Multiline = true;
            this.tchat.Name = "tchat";
            this.tchat.Size = new System.Drawing.Size(739, 614);
            this.tchat.TabIndex = 3;
            this.tchat.TextChanged += new System.EventHandler(this.tchat_TextChanged);
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 692);
            this.Controls.Add(this.tchat);
            this.Controls.Add(this.bStopServer);
            this.Controls.Add(this.bStartServer);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Server";
            this.Text = "Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bStartServer;
        private System.Windows.Forms.Button bStopServer;
        public System.Windows.Forms.TextBox tchat;
    }
}