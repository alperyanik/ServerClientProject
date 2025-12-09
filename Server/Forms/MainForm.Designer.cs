namespace Server.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.RichTextBox rtbMessages;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Label lblPort;

        private void InitializeComponent()
        {
            this.rtbMessages = new System.Windows.Forms.RichTextBox();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblIP = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();

            this.SuspendLayout();


            this.rtbMessages.Location = new System.Drawing.Point(12, 12);
            this.rtbMessages.Size = new System.Drawing.Size(460, 260);
            this.rtbMessages.ReadOnly = true;


            this.lblIP.Location = new System.Drawing.Point(12, 280);
            this.lblIP.Size = new System.Drawing.Size(25, 22);
            this.lblIP.Text = "IP:";


            this.txtIP.Location = new System.Drawing.Point(40, 280);
            this.txtIP.Size = new System.Drawing.Size(150, 22);
            this.txtIP.Text = "127.0.0.1";


            this.lblPort.Location = new System.Drawing.Point(200, 280);
            this.lblPort.Size = new System.Drawing.Size(35, 22);
            this.lblPort.Text = "Port:";


            this.txtPort.Location = new System.Drawing.Point(240, 280);
            this.txtPort.Size = new System.Drawing.Size(80, 22);
            this.txtPort.Text = "5000";


            this.btnStart.Location = new System.Drawing.Point(330, 278);
            this.btnStart.Size = new System.Drawing.Size(70, 25);
            this.btnStart.Text = "Başlat";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);


            this.btnStop.Location = new System.Drawing.Point(410, 278);
            this.btnStop.Size = new System.Drawing.Size(62, 25);
            this.btnStop.Text = "Durdur";
            this.btnStop.Enabled = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);


            this.lblStatus.Location = new System.Drawing.Point(12, 310);
            this.lblStatus.Size = new System.Drawing.Size(460, 22);
            this.lblStatus.Text = "Durum: Hazır";


            this.ClientSize = new System.Drawing.Size(484, 341);
            this.Controls.Add(this.rtbMessages);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblIP);
            this.Controls.Add(this.lblPort);

            this.Name = "MainForm";
            this.Text = "Server Chat";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
