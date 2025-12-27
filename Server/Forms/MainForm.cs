using System;
using System.Drawing;
using System.IO;
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
            
            rtbMessages.AppendText("================================================================\n");
            rtbMessages.AppendText("  Sifreli Iletisim Sunucusu v2.0                              \n");
            rtbMessages.AppendText("  AES-128 | DES | RSA Hibrit Sifreleme                        \n");
            rtbMessages.AppendText("  Kutuphaneli ve Manuel Sifre Cozme Destekli                  \n");
            rtbMessages.AppendText("================================================================\n\n");
            rtbMessages.AppendText("Sunucuyu baslatmak icin yukaridaki butona tiklayin.\n");
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            string ip = txtIP.Text.Trim();
            if (!int.TryParse(txtPort.Text.Trim(), out int port))
            {
                MessageBox.Show("Port numarasi gecersiz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                server.StartServer(ip, port);
                isRunning = true;
                btnStart.Enabled = false;
                btnStop.Enabled = true;
                UpdateStatus($"Calisiyor: {ip}:{port}", true);
                rtbMessages.AppendText($"\nSunucu baslatildi! ({ip}:{port})\n");
                rtbMessages.AppendText("Istemci bekleniyor...\n\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sunucu baslatilamadi!\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                UpdateStatus("Durum: Durduruldu", false);
                rtbMessages.AppendText("\nSunucu durduruldu.\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sunucu durdurulurken hata olustu!\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatus(string msg, bool running)
        {
            lblStatusText.Text = msg;
            if (running)
            {
                lblStatusText.ForeColor = Color.FromArgb(40, 167, 69);
            }
            else
            {
                lblStatusText.ForeColor = Color.FromArgb(255, 165, 0);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            rtbMessages.Clear();
            rtbMessages.AppendText("Log temizlendi.\n");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isRunning)
                server.StopServer();
        }

        private void btnSaveLog_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Metin Dosyalari|*.txt|Tum Dosyalar|*.*";
                sfd.Title = "Server Log Dosyasini Kaydet";
                sfd.FileName = $"server_log_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(sfd.FileName, rtbMessages.Text, System.Text.Encoding.UTF8);
                        rtbMessages.AppendText($"[{DateTime.Now:HH:mm:ss}] Log kaydedildi: {Path.GetFileName(sfd.FileName)}\n");
                        MessageBox.Show("Log basariyla kaydedildi!", "Basarili", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Dosya kaydetme hatasi: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
