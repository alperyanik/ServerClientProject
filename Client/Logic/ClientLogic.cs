using System;
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

        public string SelectedCipher { get; set; } = "Sezar";
        public string CipherKey { get; set; } = "3";
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
            catch { }
        }

        public void SendMessage(string message)
        {
            if (client == null || !client.Connected)
            {
                LogMessage("Sunucuya bağlı değil.");
                return;
            }

            string encryptedMsg = EncryptMessage(message);

            if (encryptedMsg == null)
            {
                LogMessage("Şifreleme başarısız, mesaj gönderilmedi.");
                return;
            }

            string packet = $"TEXT|{SelectedCipher}|{CipherKey}|{encryptedMsg}";

            try
            {
                byte[] data = Encoding.UTF8.GetBytes(packet);
                stream.Write(data, 0, data.Length);
                LogMessage($"Gönderildi ({SelectedCipher}): {encryptedMsg}");
            }
            catch (Exception ex)
            {
                LogMessage("Gönderme hatası: " + ex.Message);
            }
        }

        private string EncryptMessage(string plainText)
        {
            try
            {
                switch (SelectedCipher)
                {
                    case "Sezar":
                        if (int.TryParse(CipherKey, out int sShift))
                        {
                            return CaesarCipher.Encrypt(plainText, sShift);
                        }
                        else
                        {
                            LogMessage("Hata: Sezar için anahtar bir SAYI olmalıdır.");
                            return null;
                        }

                    case "Vigenere":
                        if (!string.IsNullOrWhiteSpace(CipherKey))
                        {
                            return VigenereCipher.Encrypt(plainText, CipherKey);
                        }
                        else
                        {
                            LogMessage("Hata: Vigenere için anahtar boş olamaz.");
                            return null;
                        }

                    case "Substitution":
                        if (CipherKey.Length == 26)
                        {
                            return SubstitutionCipher.Encrypt(plainText, CipherKey);
                        }
                        else
                        {
                            LogMessage("Hata: Substitution için anahtar 26 harfli olmalıdır (Örn: QWERTY...).");
                            return null;
                        }

                    case "Affine":
                        string[] parts = CipherKey.Split(',');
                        if (parts.Length == 2 && int.TryParse(parts[0], out int a) && int.TryParse(parts[1], out int b))
                        {
                            return AffineCipher.Encrypt(plainText, a, b);
                        }
                        else
                        {
                            LogMessage("Hata: Affine anahtarı 'a,b' formatında olmalı (Örn: 5,8).");
                            return null;
                        }


                    case "Playfair":

                        if (string.IsNullOrWhiteSpace(CipherKey)) { LogMessage("Hata: Anahtar boş olamaz."); return null; }
                        return PlayfairCipher.Cipher(plainText, CipherKey, true);

                    case "RailFence":
                        if (int.TryParse(CipherKey, out int rails) && rails > 1)
                        {
                            return RailFenceCipher.Encrypt(plainText, rails);
                        }
                        else
                        {
                            LogMessage("Hata: RailFence için anahtar 1'den büyük bir SAYI olmalıdır (Örn: 3).");
                            return null;
                        }

                    case "Route":
                        if (int.TryParse(CipherKey, out int rCols) && rCols > 0)
                        {
                            return RouteCipher.Encrypt(plainText, rCols);
                        }
                        else
                        {
                            LogMessage("Hata: Route Cipher için anahtar pozitif bir SAYI olmalıdır.");
                            return null;
                        }

                    case "Columnar":
                        if (!string.IsNullOrWhiteSpace(CipherKey))
                        {
                            return ColumnarTranspositionCipher.Encrypt(plainText, CipherKey);
                        }
                        else
                        {
                            LogMessage("Hata: Columnar Transposition için bir anahtar kelime veya sayı girmelisiniz.");
                            return null;
                        }

                    case "Polybius":
                        
                        if (!string.IsNullOrEmpty(CipherKey))
                        {
                            foreach (char c in CipherKey)
                            {
                                if (char.IsDigit(c))
                                {
                                    LogMessage("Hata: Polybius anahtarı SADECE HARF içermelidir (Rakam giremezsiniz).");
                                    return null;
                                }
                            }
                        }

                        
                        foreach (char c in plainText)
                        {
                            if (char.IsDigit(c))
                            {
                                LogMessage("Hata: Polybius şifrelemesi yapılacak METİN içinde sayı olamaz.");
                                return null;
                            }
                        }

                        return PolybiusCipher.Encrypt(plainText, CipherKey ?? "");



                    default:
                        return plainText;
                }
            }
            catch (Exception ex)
            {
                LogMessage("Şifreleme Hatası: " + ex.Message);
                return null;
            }
        }

        private void LogMessage(string message)
        {
            if (logBox.IsDisposed) return;

            if (logBox.InvokeRequired)
            {
                logBox.Invoke(new Action(() => LogMessage(message)));
            }
            else
            {
                logBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\n");
                logBox.ScrollToCaret();
            }
        }
    }
}