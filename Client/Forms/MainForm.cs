using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Client.Logic;

namespace Client.Forms
{
    public partial class MainForm : Form
    {
        private ClientLogic clientLogic;
        private string keyPlaceholder = "Anahtar (Sayi)";
        private string messagePlaceholder = "Mesajinizi buraya yazin...";

        public MainForm()
        {
            InitializeComponent();

            cmbCipher.Items.AddRange(new string[] { "Sezar", "Vigenere", "Substitution", "Affine","Playfair", "RailFence" , "Route" , "Columnar" , "Polybius" , "Hill" ,"AES" , "DES"});
            cmbCipher.SelectedIndex = 0;

            cmbMode.Items.AddRange(new string[] { "Kutuphane", "Manuel" });
            cmbMode.SelectedIndex = 0;

            cmbKeyExchange.Items.AddRange(new string[] { "RSA", "ECC" });
            cmbKeyExchange.SelectedIndex = 0;
            cmbKeyExchange.SelectedIndexChanged += CmbKeyExchange_SelectedIndexChanged;

            clientLogic = new ClientLogic(rtbMessages);
            
            txtKey.Enter += TxtKey_Enter;
            txtKey.Leave += TxtKey_Leave;
            txtMessage.Enter += TxtMessage_Enter;
            txtMessage.Leave += TxtMessage_Leave;
            
            SetKeyPlaceholder(keyPlaceholder);
            SetMessagePlaceholder();
            
            rtbMessages.AppendText("================================================================\n");
            rtbMessages.AppendText("  Sifreli Iletisim Istemcisi v2.0                             \n");
            rtbMessages.AppendText("  AES-128 | DES | RSA | ECC Hibrit Sifreleme                  \n");
            rtbMessages.AppendText("================================================================\n\n");
        }

        #region Placeholder Methods

        private void SetKeyPlaceholder(string placeholder)
        {
            keyPlaceholder = placeholder;
            txtKey.Text = placeholder;
            txtKey.ForeColor = Color.Gray;
        }

        private void TxtKey_Enter(object sender, EventArgs e)
        {
            if (txtKey.ForeColor == Color.Gray)
            {
                txtKey.Text = "";
                txtKey.ForeColor = Color.Black;
            }
        }

        private void TxtKey_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKey.Text))
            {
                txtKey.Text = keyPlaceholder;
                txtKey.ForeColor = Color.Gray;
            }
        }

        private void SetMessagePlaceholder()
        {
            txtMessage.Text = messagePlaceholder;
            txtMessage.ForeColor = Color.Gray;
        }

        private void TxtMessage_Enter(object sender, EventArgs e)
        {
            if (txtMessage.ForeColor == Color.Gray)
            {
                txtMessage.Text = "";
                txtMessage.ForeColor = Color.Black;
            }
        }

        private void TxtMessage_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                SetMessagePlaceholder();
            }
        }

        #endregion

        private void UpdateConnectionStatus(bool connected)
        {
            if (connected)
            {
                lblStatus.Text = "Bagli";
                lblStatus.ForeColor = Color.FromArgb(40, 167, 69);
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
            }
            else
            {
                lblStatus.Text = "Baglanti Yok";
                lblStatus.ForeColor = Color.FromArgb(255, 165, 0);
                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            clientLogic.IP = txtIp.Text;
            clientLogic.Port = int.Parse(txtPort.Text);
            clientLogic.SelectedCipher = cmbCipher.SelectedItem.ToString();
            
            // Placeholder degilse anahtari al
            clientLogic.CipherKey = (txtKey.ForeColor == Color.Gray) ? "" : txtKey.Text;

            if (clientLogic.ConnectToServer(clientLogic.IP, clientLogic.Port))
            {
                UpdateConnectionStatus(true);
                MessageBox.Show("Sunucuya baglandi!", "Basarili", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            clientLogic.Disconnect();
            UpdateConnectionStatus(false);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string cipher = cmbCipher.SelectedItem.ToString();
            
            // Placeholder degilse anahtari al
            string key = (txtKey.ForeColor == Color.Gray) ? "" : txtKey.Text;
            
            string message = (txtMessage.ForeColor == Color.Gray) ? "" : txtMessage.Text;

            if (cipher == "Sezar" && !int.TryParse(key, out _))
            {
                MessageBox.Show("Sezar sifrelemesi icin sadece SAYISAL anahtar giriniz!",
                    "Anahtar Hatasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cipher == "Vigenere" && !Regex.IsMatch(key, @"^[a-zA-Z]+$"))
            {
                MessageBox.Show("Vigenere sifrelemesi icin sadece HARF anahtar giriniz!",
                    "Anahtar Hatasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            clientLogic.SelectedCipher = cipher;
            clientLogic.CipherKey = key;

            if (cipher == "AES" || cipher == "DES")
            {
                clientLogic.UseManualMode = cmbMode.SelectedItem.ToString().Contains("Manuel");
                clientLogic.UseECCKeyExchange = cmbKeyExchange.SelectedItem.ToString().Contains("ECC");
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show("Gonderilecek mesaj bos olamaz!", "Uyari", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            clientLogic.SendMessage(message);
            txtMessage.Text = "";
            SetMessagePlaceholder();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            rtbMessages.Clear();
            rtbMessages.AppendText("Log temizlendi.\n");
        }

        private void cmbCipher_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clientLogic == null) return;

            string selected = cmbCipher.SelectedItem.ToString();

            if (selected == "AES" || selected == "DES")
            {
                string keyMethod = cmbKeyExchange.SelectedIndex == 0 ? "RSA" : "ECC";
                txtKey.Text = $"Otomatik ({keyMethod} Korumali)";
                txtKey.ForeColor = Color.Gray;
                txtKey.Enabled = false;
                txtKey.BackColor = Color.FromArgb(230, 255, 230);
                
                cmbMode.Visible = true;
                lblMode.Visible = true;
                cmbKeyExchange.Visible = true;
                lblKeyExchange.Visible = true;
            }
            else
            {
                txtKey.Enabled = true;
                txtKey.BackColor = Color.FromArgb(255, 255, 240);
                cmbMode.Visible = false;
                lblMode.Visible = false;
                cmbKeyExchange.Visible = false;
                lblKeyExchange.Visible = false;

                if (selected == "Sezar")
                    SetKeyPlaceholder("Anahtar (Sayi)");
                else if (selected == "Vigenere")
                    SetKeyPlaceholder("Anahtar (Kelime)");
                else if (selected == "Substitution")
                    SetKeyPlaceholder("26 Harfli Alfabe Sirasi");
                else if (selected == "Affine")
                    SetKeyPlaceholder("a,b (Orn: 5,8)");
                else if (selected == "Playfair")
                    SetKeyPlaceholder("Anahtar Kelime (Orn: SECRET)");
                else if (selected == "RailFence")
                    SetKeyPlaceholder("Ray Sayisi (Orn: 3)");
                else if (selected == "Route")
                    SetKeyPlaceholder("Sutun Sayisi (Orn: 4)");
                else if (selected == "Columnar")
                    SetKeyPlaceholder("Anahtar Kelime veya Sayi");
                else if (selected == "Polybius")
                    SetKeyPlaceholder("Anahtar Kelime (Istege Bagli)");
                else if (selected == "Hill")
                    SetKeyPlaceholder("Tam Kare Uzunlukta Kelime");
            }

            clientLogic.SelectedCipher = selected;
        }

        private void CmbKeyExchange_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = cmbCipher.SelectedItem?.ToString();
            if (selected == "AES" || selected == "DES")
            {
                string keyMethod = cmbKeyExchange.SelectedIndex == 0 ? "RSA" : "ECC";
                txtKey.Text = $"Otomatik ({keyMethod} Korumali)";
            }
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Metin Dosyalari|*.txt|Tum Dosyalar|*.*";
                ofd.Title = "Sifrelenecek Dosyayi Secin";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string content = File.ReadAllText(ofd.FileName, System.Text.Encoding.UTF8);
                        txtMessage.Text = content;
                        txtMessage.ForeColor = Color.Black;
                        rtbMessages.AppendText($"[{DateTime.Now:HH:mm:ss}] Dosya yuklendi: {Path.GetFileName(ofd.FileName)}\n");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Dosya okuma hatasi: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnSaveLog_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Metin Dosyalari|*.txt|Tum Dosyalar|*.*";
                sfd.Title = "Log Dosyasini Kaydet";
                sfd.FileName = $"log_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

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
