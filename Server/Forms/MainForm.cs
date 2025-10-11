using System;
using System.Windows.Forms;
using Server.Logic;

namespace Server.Forms
{
    public partial class MainForm : Form
    {
        private ServerLogic server;
        private bool isRunning = false;

        public MainForm()
        {
            InitializeComponent();
            server = new ServerLogic(rtbMessages);
            btnStop.Enabled = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            string ip = txtIP.Text.Trim();
            if (!int.TryParse(txtPort.Text.Trim(), out int port))
            {
                MessageBox.Show("Port numarası geçersiz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                server.StartServer(ip, port);
                isRunning = true;
                btnStart.Enabled = false;
                btnStop.Enabled = true;
                UpdateStatus($"Sunucu başlatıldı ({ip}:{port})");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Server başlatılamadı!\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (!isRunning) return;

            try
            {
                server.StopServer();
                isRunning = false;
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                UpdateStatus("Sunucu durduruldu.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Server durdurulurken hata oluştu!\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatus(string msg)
        {
            lblStatus.Text = "Durum: " + msg;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isRunning)
                server.StopServer();
        }
    }
}
