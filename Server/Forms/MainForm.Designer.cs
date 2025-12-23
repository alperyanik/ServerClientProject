namespace Server.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

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
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.grpLog = new System.Windows.Forms.GroupBox();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlStatusBar = new System.Windows.Forms.Panel();
            this.lblStatusText = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();

            this.grpSettings.SuspendLayout();
            this.grpLog.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlStatusBar.SuspendLayout();
            this.SuspendLayout();

            // 
            // Form Properties
            // 
            this.BackColor = System.Drawing.Color.FromArgb(240, 240, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);

            // 
            // pnlHeader - Header Panel
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(0, 123, 255);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height = 50;
            this.pnlHeader.Controls.Add(this.lblTitle);

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(12, 10);
            this.lblTitle.Text = "🖥️ Şifreleme Sunucusu";

            // 
            // grpSettings - Settings Group
            // 
            this.grpSettings.Text = "⚙️ Sunucu Ayarları";
            this.grpSettings.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpSettings.ForeColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.grpSettings.Location = new System.Drawing.Point(12, 60);
            this.grpSettings.Size = new System.Drawing.Size(560, 80);
            this.grpSettings.Controls.Add(this.lblIP);
            this.grpSettings.Controls.Add(this.txtIP);
            this.grpSettings.Controls.Add(this.lblPort);
            this.grpSettings.Controls.Add(this.txtPort);
            this.grpSettings.Controls.Add(this.btnStart);
            this.grpSettings.Controls.Add(this.btnStop);

            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblIP.Location = new System.Drawing.Point(10, 25);
            this.lblIP.Text = "IP Adresi:";

            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(10, 45);
            this.txtIP.Size = new System.Drawing.Size(150, 23);
            this.txtIP.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtIP.Text = "127.0.0.1";
            this.txtIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPort.Location = new System.Drawing.Point(170, 25);
            this.lblPort.Text = "Port:";

            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(170, 45);
            this.txtPort.Size = new System.Drawing.Size(80, 23);
            this.txtPort.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtPort.Text = "5000";
            this.txtPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(270, 42);
            this.btnStart.Size = new System.Drawing.Size(130, 30);
            this.btnStart.Text = "▶️ Sunucuyu Başlat";
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStart.FlatAppearance.BorderSize = 0;
            this.btnStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);

            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(410, 42);
            this.btnStop.Size = new System.Drawing.Size(130, 30);
            this.btnStop.Text = "⏹️ Sunucuyu Durdur";
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.btnStop.ForeColor = System.Drawing.Color.White;
            this.btnStop.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnStop.FlatAppearance.BorderSize = 0;
            this.btnStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStop.Enabled = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);

            // 
            // grpLog - Log Group
            // 
            this.grpLog.Text = "📋 İletişim Logları";
            this.grpLog.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpLog.ForeColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.grpLog.Location = new System.Drawing.Point(12, 150);
            this.grpLog.Size = new System.Drawing.Size(560, 340);
            this.grpLog.Controls.Add(this.rtbMessages);
            this.grpLog.Controls.Add(this.btnClear);

            // 
            // rtbMessages
            // 
            this.rtbMessages.Location = new System.Drawing.Point(10, 25);
            this.rtbMessages.Size = new System.Drawing.Size(540, 275);
            this.rtbMessages.Font = new System.Drawing.Font("Consolas", 9F);
            this.rtbMessages.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.rtbMessages.ForeColor = System.Drawing.Color.FromArgb(0, 255, 0);
            this.rtbMessages.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbMessages.ReadOnly = true;

            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(450, 305);
            this.btnClear.Size = new System.Drawing.Size(100, 28);
            this.btnClear.Text = "🗑️ Temizle";
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);

            // 
            // pnlStatusBar - Status Bar
            // 
            this.pnlStatusBar.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            this.pnlStatusBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlStatusBar.Height = 30;
            this.pnlStatusBar.Controls.Add(this.lblStatusText);

            // 
            // lblStatusText
            // 
            this.lblStatusText.AutoSize = true;
            this.lblStatusText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatusText.ForeColor = System.Drawing.Color.FromArgb(255, 165, 0);
            this.lblStatusText.Location = new System.Drawing.Point(12, 7);
            this.lblStatusText.Text = "⏸️ Durum: Hazır - Başlatmak için butona tıklayın";

            // 
            // lblStatus (kept for compatibility)
            // 
            this.lblStatus.Visible = false;

            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(584, 530);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.grpSettings);
            this.Controls.Add(this.grpLog);
            this.Controls.Add(this.pnlStatusBar);
            this.Controls.Add(this.lblStatus);
            this.Name = "MainForm";
            this.Text = "🖥️ Şifreli İletişim Sunucusu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            this.grpLog.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlStatusBar.ResumeLayout(false);
            this.pnlStatusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbMessages;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.GroupBox grpLog;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlStatusBar;
        private System.Windows.Forms.Label lblStatusText;
        private System.Windows.Forms.Button btnClear;
    }
}
