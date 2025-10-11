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

            cmbCipher.Items.AddRange(new string[] { "Sezar", "Vigenere" });
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
            
            if (clientLogic == null)
                return;

            string selected = cmbCipher.SelectedItem.ToString();

            if (selected == "Sezar")
            {
                txtKey.Text = "";
                txtKey.Text = "Anahtar (ör: 3)";
            }
            else if (selected == "Vigenere")
            {
                txtKey.Text = "";
                txtKey.Text = "Anahtar (ör: KEY)";
            }

            clientLogic.SelectedCipher = selected;
        }

    }
}
