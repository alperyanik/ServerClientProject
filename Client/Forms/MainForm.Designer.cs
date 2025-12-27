namespace Client.Forms
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
            this.txtIp = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.cmbCipher = new System.Windows.Forms.ComboBox();
            this.cmbMode = new System.Windows.Forms.ComboBox();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.rtbMessages = new System.Windows.Forms.RichTextBox();
            this.lblMode = new System.Windows.Forms.Label();
            this.cmbKeyExchange = new System.Windows.Forms.ComboBox();
            this.lblKeyExchange = new System.Windows.Forms.Label();
            this.grpConnection = new System.Windows.Forms.GroupBox();
            this.grpEncryption = new System.Windows.Forms.GroupBox();
            this.grpMessage = new System.Windows.Forms.GroupBox();
            this.lblIp = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblCipher = new System.Windows.Forms.Label();
            this.lblKey = new System.Windows.Forms.Label();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.picStatus = new System.Windows.Forms.PictureBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.btnSaveLog = new System.Windows.Forms.Button();

            this.grpConnection.SuspendLayout();
            this.grpEncryption.SuspendLayout();
            this.grpMessage.SuspendLayout();
            this.pnlStatus.SuspendLayout();
            this.SuspendLayout();

            // 
            // Form Properties
            // 
            this.BackColor = System.Drawing.Color.FromArgb(240, 240, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);

            // 
            // pnlStatus - Status Panel (Top)
            // 
            this.pnlStatus.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStatus.Height = 35;
            this.pnlStatus.Controls.Add(this.lblStatus);

            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(255, 165, 0);
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Location = new System.Drawing.Point(12, 8);
            this.lblStatus.Text = "Baglanti Yok";

            // 
            // grpConnection - Connection Group
            // 
            this.grpConnection.Text = "Baglanti Ayarlari";
            this.grpConnection.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpConnection.ForeColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.grpConnection.Location = new System.Drawing.Point(12, 45);
            this.grpConnection.Size = new System.Drawing.Size(340, 85);
            this.grpConnection.Controls.Add(this.lblIp);
            this.grpConnection.Controls.Add(this.txtIp);
            this.grpConnection.Controls.Add(this.lblPort);
            this.grpConnection.Controls.Add(this.txtPort);
            this.grpConnection.Controls.Add(this.btnConnect);
            this.grpConnection.Controls.Add(this.btnDisconnect);

            // 
            // lblIp
            // 
            this.lblIp.AutoSize = true;
            this.lblIp.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblIp.Location = new System.Drawing.Point(10, 25);
            this.lblIp.Text = "IP Adresi:";

            // 
            // txtIp
            // 
            this.txtIp.Location = new System.Drawing.Point(10, 45);
            this.txtIp.Size = new System.Drawing.Size(120, 23);
            this.txtIp.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtIp.Text = "127.0.0.1";
            this.txtIp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPort.Location = new System.Drawing.Point(140, 25);
            this.lblPort.Text = "Port:";

            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(140, 45);
            this.txtPort.Size = new System.Drawing.Size(60, 23);
            this.txtPort.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtPort.Text = "5000";
            this.txtPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(210, 43);
            this.btnConnect.Size = new System.Drawing.Size(60, 27);
            this.btnConnect.Text = "Baglan";
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnect.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            this.btnConnect.ForeColor = System.Drawing.Color.White;
            this.btnConnect.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnConnect.FlatAppearance.BorderSize = 0;
            this.btnConnect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);

            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(275, 43);
            this.btnDisconnect.Size = new System.Drawing.Size(55, 27);
            this.btnDisconnect.Text = "Kes";
            this.btnDisconnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisconnect.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.btnDisconnect.ForeColor = System.Drawing.Color.White;
            this.btnDisconnect.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnDisconnect.FlatAppearance.BorderSize = 0;
            this.btnDisconnect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);

            // 
            // grpEncryption - Encryption Group
            // 
            this.grpEncryption.Text = "Sifreleme Ayarlari";
            this.grpEncryption.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpEncryption.ForeColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.grpEncryption.Location = new System.Drawing.Point(358, 45);
            this.grpEncryption.Size = new System.Drawing.Size(320, 120);
            this.grpEncryption.Controls.Add(this.lblCipher);
            this.grpEncryption.Controls.Add(this.cmbCipher);
            this.grpEncryption.Controls.Add(this.lblMode);
            this.grpEncryption.Controls.Add(this.cmbMode);
            this.grpEncryption.Controls.Add(this.lblKeyExchange);
            this.grpEncryption.Controls.Add(this.cmbKeyExchange);

            // 
            // lblCipher
            // 
            this.lblCipher.AutoSize = true;
            this.lblCipher.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCipher.Location = new System.Drawing.Point(10, 25);
            this.lblCipher.Text = "Algoritma:";

            // 
            // cmbCipher
            // 
            this.cmbCipher.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCipher.Location = new System.Drawing.Point(10, 45);
            this.cmbCipher.Size = new System.Drawing.Size(140, 23);
            this.cmbCipher.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbCipher.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbCipher.SelectedIndexChanged += new System.EventHandler(this.cmbCipher_SelectedIndexChanged);

            // 
            // lblMode
            // 
            this.lblMode.AutoSize = true;
            this.lblMode.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblMode.Location = new System.Drawing.Point(10, 70);
            this.lblMode.Text = "Mod:";
            this.lblMode.Visible = false;

            // 
            // cmbMode
            // 
            this.cmbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMode.Location = new System.Drawing.Point(10, 88);
            this.cmbMode.Size = new System.Drawing.Size(140, 23);
            this.cmbMode.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbMode.Visible = false;

            // 
            // lblKeyExchange
            // 
            this.lblKeyExchange.AutoSize = true;
            this.lblKeyExchange.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblKeyExchange.Location = new System.Drawing.Point(160, 70);
            this.lblKeyExchange.Text = "Anahtar Dagitimi:";
            this.lblKeyExchange.Visible = false;

            // 
            // cmbKeyExchange
            // 
            this.cmbKeyExchange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKeyExchange.Location = new System.Drawing.Point(160, 88);
            this.cmbKeyExchange.Size = new System.Drawing.Size(145, 23);
            this.cmbKeyExchange.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbKeyExchange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbKeyExchange.Visible = false;

            // 
            // lblKey
            // 
            this.lblKey.AutoSize = true;
            this.lblKey.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblKey.ForeColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.lblKey.Location = new System.Drawing.Point(12, 170);
            this.lblKey.Text = "Anahtar:";

            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(12, 190);
            this.txtKey.Size = new System.Drawing.Size(666, 25);
            this.txtKey.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtKey.BackColor = System.Drawing.Color.FromArgb(255, 255, 240);

            // 
            // grpMessage - Message Group
            // 
            this.grpMessage.Text = "Mesaj";
            this.grpMessage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpMessage.ForeColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.grpMessage.Location = new System.Drawing.Point(12, 220);
            this.grpMessage.Size = new System.Drawing.Size(666, 75);
            this.grpMessage.Controls.Add(this.txtMessage);
            this.grpMessage.Controls.Add(this.btnLoadFile);
            this.grpMessage.Controls.Add(this.btnSend);

            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(10, 30);
            this.txtMessage.Size = new System.Drawing.Size(440, 30);
            this.txtMessage.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // 
            // btnLoadFile
            // 
            this.btnLoadFile.Location = new System.Drawing.Point(460, 28);
            this.btnLoadFile.Size = new System.Drawing.Size(80, 33);
            this.btnLoadFile.Text = "Dosya";
            this.btnLoadFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadFile.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            this.btnLoadFile.ForeColor = System.Drawing.Color.White;
            this.btnLoadFile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLoadFile.FlatAppearance.BorderSize = 0;
            this.btnLoadFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click);

            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(550, 28);
            this.btnSend.Size = new System.Drawing.Size(105, 33);
            this.btnSend.Text = "Gonder";
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.BackColor = System.Drawing.Color.FromArgb(0, 123, 255);
            this.btnSend.ForeColor = System.Drawing.Color.White;
            this.btnSend.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSend.FlatAppearance.BorderSize = 0;
            this.btnSend.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);

            // 
            // rtbMessages
            // 
            this.rtbMessages.Location = new System.Drawing.Point(12, 305);
            this.rtbMessages.Size = new System.Drawing.Size(666, 230);
            this.rtbMessages.Font = new System.Drawing.Font("Consolas", 9F);
            this.rtbMessages.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            this.rtbMessages.ForeColor = System.Drawing.Color.FromArgb(0, 255, 0);
            this.rtbMessages.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbMessages.ReadOnly = true;

            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(588, 542);
            this.btnClear.Size = new System.Drawing.Size(90, 28);
            this.btnClear.Text = "Temizle";
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);

            // 
            // btnSaveLog
            // 
            this.btnSaveLog.Location = new System.Drawing.Point(490, 542);
            this.btnSaveLog.Size = new System.Drawing.Size(90, 28);
            this.btnSaveLog.Text = "Kaydet";
            this.btnSaveLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveLog.BackColor = System.Drawing.Color.FromArgb(23, 162, 184);
            this.btnSaveLog.ForeColor = System.Drawing.Color.White;
            this.btnSaveLog.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSaveLog.FlatAppearance.BorderSize = 0;
            this.btnSaveLog.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveLog.Click += new System.EventHandler(this.btnSaveLog_Click);

            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(690, 575);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Controls.Add(this.pnlStatus);
            this.Controls.Add(this.grpConnection);
            this.Controls.Add(this.grpEncryption);
            this.Controls.Add(this.lblKey);
            this.Controls.Add(this.txtKey);
            this.Controls.Add(this.grpMessage);
            this.Controls.Add(this.rtbMessages);
            this.Controls.Add(this.btnSaveLog);
            this.Controls.Add(this.btnClear);
            this.Name = "MainForm";
            this.Text = "Sifreli Istemci - AES | DES | RSA";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            this.grpConnection.ResumeLayout(false);
            this.grpConnection.PerformLayout();
            this.grpEncryption.ResumeLayout(false);
            this.grpEncryption.PerformLayout();
            this.grpMessage.ResumeLayout(false);
            this.grpMessage.PerformLayout();
            this.pnlStatus.ResumeLayout(false);
            this.pnlStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.ComboBox cmbCipher;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.RichTextBox rtbMessages;
        private System.Windows.Forms.ComboBox cmbMode;
        private System.Windows.Forms.Label lblMode;
        private System.Windows.Forms.ComboBox cmbKeyExchange;
        private System.Windows.Forms.Label lblKeyExchange;
        private System.Windows.Forms.GroupBox grpConnection;
        private System.Windows.Forms.GroupBox grpEncryption;
        private System.Windows.Forms.GroupBox grpMessage;
        private System.Windows.Forms.Label lblIp;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblCipher;
        private System.Windows.Forms.Label lblKey;
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.PictureBox picStatus;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnLoadFile;
        private System.Windows.Forms.Button btnSaveLog;
    }
}
