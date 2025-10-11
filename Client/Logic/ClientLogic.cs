using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using Client.Logic.Ciphers;

namespace Client.Logic
{
    public class ClientLogic
    {
        private TcpClient client;
        private NetworkStream stream;
        private readonly RichTextBox logBox;

        public string SelectedCipher { get; set; }
        public string CipherKey { get; set; } 
        public string IP { get; set; }
        public int Port { get; set; }

        public ClientLogic(RichTextBox logBox)
        {
            this.logBox = logBox;
        }

        public bool ConnectToServer(string ip, int port)
        {
            try
            {
                client = new TcpClient();
                client.Connect(ip, port);
                stream = client.GetStream();
                LogMessage($"Sunucuya bağlanıldı ({ip}:{port})");
                return true;
            }
            catch (Exception ex)
            {
                LogMessage("Bağlantı hatası: " + ex.Message);
                return false;
            }
        }

        public void Disconnect()
        {
            try
            {
                stream?.Close();
                client?.Close();
                LogMessage("Bağlantı sonlandırıldı.");
            }
            catch (Exception ex)
            {
                LogMessage("Bağlantı sonlandırma hatası: " + ex.Message);
            }
        }

        public void SendMessage(string message)
        {
            if (client == null || !client.Connected)
            {
                LogMessage("Sunucuya bağlı değil.");
                return;
            }

            string encrypted = EncryptMessage(message);
            if (encrypted == null)
            {
                LogMessage("Şifreleme başarısız.");
                return;
            }

            byte[] data = Encoding.UTF8.GetBytes(encrypted);
            stream.Write(data, 0, data.Length);
            LogMessage($"Mesaj gönderildi ({SelectedCipher}): {encrypted}");
        }

        private string EncryptMessage(string plainText)
        {
            try
            {
                switch (SelectedCipher)
                {
                    case "Sezar":
                        if (int.TryParse(CipherKey, out int shift))
                            return CaesarCipher.Encrypt(plainText, shift);
                        else
                            LogMessage("Sezar için geçerli bir sayı anahtarı girin.");
                        break;

                    case "Vigenere":
                        if (!string.IsNullOrWhiteSpace(CipherKey))
                            return VigenereCipher.Encrypt(plainText, CipherKey);
                        else
                            LogMessage("Vigenere için anahtar boş olamaz.");
                        break;
                }
            }
            catch (Exception ex)
            {
                LogMessage("Şifreleme hatası: " + ex.Message);
            }

            return null;
        }

        private void LogMessage(string message)
        {
            logBox.Invoke(new Action(() =>
            {
                logBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\n");
            }));
        }
    }
}
