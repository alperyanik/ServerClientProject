using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Client.Logic;

namespace Client.Forms
{
    public partial class MainForm : Form
    {
        private ClientLogic clientLogic;

        public MainForm()
        {
            InitializeComponent();

            cmbCipher.Items.AddRange(new string[] { "Sezar", "Vigenere", "Substitution", "Affine","Playfair", "RailFence" , "Route" , "Columnar" , "Polybius" , "Hill" ,"AES" , "DES"});
            cmbCipher.SelectedIndex = 0;

            clientLogic = new ClientLogic(rtbMessages);
        }


        private void btnConnect_Click(object sender, EventArgs e)
        {
            clientLogic.IP = txtIp.Text;
            clientLogic.Port = int.Parse(txtPort.Text);
            clientLogic.SelectedCipher = cmbCipher.SelectedItem.ToString();
            clientLogic.CipherKey = txtKey.Text;

            if (clientLogic.ConnectToServer(clientLogic.IP, clientLogic.Port))
            {
                MessageBox.Show("Sunucuya bağlanıldı!");
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            clientLogic.Disconnect();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string cipher = cmbCipher.SelectedItem.ToString();
            string key = txtKey.Text;


            if (cipher == "Sezar" && !int.TryParse(key, out _))
            {
                MessageBox.Show("⚠️ Sezar şifrelemesi için sadece SAYISAL anahtar giriniz!",
                    "Anahtar Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cipher == "Vigenere" && !Regex.IsMatch(key, @"^[a-zA-Z]+$"))
            {
                MessageBox.Show("⚠️ Vigenere şifrelemesi için sadece HARF anahtar giriniz!",
                    "Anahtar Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            clientLogic.SelectedCipher = cipher;
            clientLogic.CipherKey = key;

            if (string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                MessageBox.Show("Gönderilecek mesaj boş olamaz!");
                return;
            }

            clientLogic.SendMessage(txtMessage.Text);
        }


        private void cmbCipher_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clientLogic == null) return;

            string selected = cmbCipher.SelectedItem.ToString();

            // 1. AES ve DES için Özel Kontrol (Kilitli Kutu)
            if (selected == "AES" || selected == "DES")
            {
                txtKey.Text = "Otomatik (RSA Korumalı)";
                txtKey.Enabled = false; // Kullanıcı müdahale edemez
            }
            else
            {
                // 2. Diğer Algoritmalar (Kilidi Aç ve İpucunu Göster)
                txtKey.Enabled = true;

                if (selected == "Sezar")
                    txtKey.Text = "Anahtar (Sayı)";
                else if (selected == "Vigenere")
                    txtKey.Text = "Anahtar (Kelime)";
                else if (selected == "Substitution")
                    txtKey.Text = "26 Harfli Alfabe Sırası";
                else if (selected == "Affine")
                    txtKey.Text = "a,b (Örn: 5,8)";
                else if (selected == "Playfair")
                    txtKey.Text = "Anahtar Kelime (Örn: SECRET)";
                else if (selected == "RailFence")
                    txtKey.Text = "Ray Sayısı (Örn: 3)";
                else if (selected == "Route")
                    txtKey.Text = "Sütun Sayısı (Örn: 4)";
                else if (selected == "Columnar")
                    txtKey.Text = "Anahtar Kelime veya Sayı (Örn: KALEM veya 123)";
                else if (selected == "Polybius")
                    txtKey.Text = "Anahtar Kelime (İsteğe Bağlı)";
                else if (selected == "Hill")
                    txtKey.Text = "Tam Kare Uzunlukta Kelime (Örn: DORT, KALEMLER)";
            }

            // Seçimi Logic katmanına bildir
            clientLogic.SelectedCipher = selected;
        }

    }
}
