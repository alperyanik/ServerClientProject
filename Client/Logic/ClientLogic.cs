using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Client.Logic.Ciphers;

namespace Client.Logic
{
    public class ClientLogic
    {
        private TcpClient client;
        private NetworkStream stream;
        private readonly RichTextBox logBox;
        private Thread listenThread; // Mesaj dinleme thread'i

        public string SelectedCipher { get; set; } = "Sezar";
        public string CipherKey { get; set; } = "3";
        public string IP { get; set; }
        public int Port { get; set; }

        // --- HİBRİT SİSTEM DEĞİŞKENLERİ ---
        public string ServerPublicKey { get; set; } // Sunucudan gelecek RSA anahtarı
        private byte[] sessionKey;                  // O anki AES/DES anahtarı
        private string currentAlgo;                 // "AES" veya "DES"
        // ----------------------------------

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

                // 1. Dinleyiciyi Başlat (Gelen mesajları okumak için)
                listenThread = new Thread(ListenForMessages);
                listenThread.IsBackground = true;
                listenThread.Start();

                // 2. Hemen Public Key İste
                byte[] reqData = Encoding.UTF8.GetBytes("REQ_PUB_KEY|");
                stream.Write(reqData, 0, reqData.Length);

                return true;
            }
            catch (Exception ex)
            {
                LogMessage("Bağlantı hatası: " + ex.Message);
                return false;
            }
        }

        // Sunucudan gelen mesajları sürekli dinleyen metod
        private void ListenForMessages()
        {
            try
            {
                byte[] buffer = new byte[8192];
                int bytesRead;

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    // --- ÖZEL KOMUT: RSA PUBLIC KEY ---
                    if (message.StartsWith("COMMAND|PUBLIC_KEY|"))
                    {
                        string[] parts = message.Split('|');
                        if (parts.Length >= 3)
                        {
                            ServerPublicKey = parts[2]; // XML verisini kaydet
                            LogMessage("[SİSTEM] Sunucu RSA Anahtarı alındı ve kaydedildi.");
                        }
                        continue; // Bunu ekrana sohbet mesajı gibi basma
                    }
                    // -----------------------------------

                    // Normal mesajları ekrana bas
                    LogMessage(message);
                }
            }
            catch
            {
                LogMessage("Sunucu bağlantısı koptu.");
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
                // Hata mesajı zaten EncryptMessage içinde loglandı
                return;
            }

            // Paket formatı: TEXT | ALGO | KEY | MSG
            // AES ve DES için KEY kısmını boş gönderiyoruz çünkü sunucuda zaten var.
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

        // El Sıkışma (Handshake): Simetrik anahtarı RSA ile şifreleyip sunucuya atar
        private bool PerformHandshake(string algo, byte[] keyBytes)
        {
            try
            {
                if (string.IsNullOrEmpty(ServerPublicKey))
                {
                    LogMessage("Hata: Sunucu RSA Anahtarı henüz gelmedi! Biraz bekleyip tekrar deneyin.");
                    return false;
                }

                // 1. Anahtarı RSA ile şifrele
                byte[] encryptedKey = RSACipher.Encrypt(keyBytes, ServerPublicKey);

                // 2. Base64 yap
                string b64Key = Convert.ToBase64String(encryptedKey);

                // 3. Gönder: KEY_EXCHANGE | ALGO | ŞİFRELİ_ANAHTAR
                string packet = $"KEY_EXCHANGE|{algo}|{b64Key}";
                byte[] data = Encoding.UTF8.GetBytes(packet);
                stream.Write(data, 0, data.Length);

                LogMessage($"[SİSTEM] Yeni {algo} anahtarı üretildi ve RSA ile sunucuya gönderildi.");
                return true;
            }
            catch (Exception ex)
            {
                LogMessage("Handshake Hatası: " + ex.Message);
                return false;
            }
        }

        private string EncryptMessage(string plainText)
        {
            try
            {
                switch (SelectedCipher)
                {
                    case "AES":
                        // Eğer anahtar yoksa veya algoritma değiştiyse yeni anahtar üretip sunucuyla anlaş
                        if (sessionKey == null || currentAlgo != "AES")
                        {
                            sessionKey = AESCipher.GenerateRandomKey();
                            currentAlgo = "AES";
                            if (!PerformHandshake("AES", sessionKey)) return null;

                            // Sunucunun işlemesi için çok kısa bekle
                            Thread.Sleep(50);
                        }
                        // Mesajı şifrele
                        return AESCipher.Encrypt(plainText, sessionKey);

                    case "DES":
                        if (sessionKey == null || currentAlgo != "DES")
                        {
                            sessionKey = DESCipher.GenerateRandomKey();
                            currentAlgo = "DES";
                            if (!PerformHandshake("DES", sessionKey)) return null;
                            Thread.Sleep(50);
                        }
                        return DESCipher.Encrypt(plainText, sessionKey);

                    // --- DİĞER ALGORİTMALAR ---
                    case "Sezar":
                        if (int.TryParse(CipherKey, out int sShift)) return CaesarCipher.Encrypt(plainText, sShift);
                        else { LogMessage("Hata: Sezar anahtarı sayı olmalı."); return null; }

                    case "Vigenere": return VigenereCipher.Encrypt(plainText, CipherKey);

                    case "Substitution":
                        if (CipherKey.Length == 26) return SubstitutionCipher.Encrypt(plainText, CipherKey);
                        else { LogMessage("Hata: Anahtar 26 karakter olmalı."); return null; }

                    case "Affine":
                        string[] parts = CipherKey.Split(',');
                        if (parts.Length == 2 && int.TryParse(parts[0], out int a) && int.TryParse(parts[1], out int b))
                            return AffineCipher.Encrypt(plainText, a, b);
                        else { LogMessage("Hata: Format 'a,b' olmalı."); return null; }

                    case "Playfair":
                        // Playfair özel kontrolleri (senin kodun)
                        foreach (char c in plainText) if (!char.IsLetter(c)) { LogMessage("Playfair sadece harf kabul eder."); return null; }
                        return PlayfairCipher.Cipher(plainText, CipherKey, true);

                    case "RailFence":
                        if (int.TryParse(CipherKey, out int rails) && rails > 1) return RailFenceCipher.Encrypt(plainText, rails);
                        else { LogMessage("Hata: Ray sayısı > 1 olmalı."); return null; }

                    case "Route":
                        if (int.TryParse(CipherKey, out int rc) && rc > 0) return RouteCipher.Encrypt(plainText, rc);
                        else { LogMessage("Hata: Sütun sayısı pozitif olmalı."); return null; }

                    case "Columnar": return ColumnarTranspositionCipher.Encrypt(plainText, CipherKey);

                    case "Polybius":
                        foreach (char c in CipherKey) if (char.IsDigit(c)) { LogMessage("Anahtarda rakam olamaz."); return null; }
                        foreach (char c in plainText) if (char.IsDigit(c)) { LogMessage("Metinde rakam olamaz."); return null; }
                        return PolybiusCipher.Encrypt(plainText, CipherKey ?? "");

                    case "Hill":
                        if (!HillCipher.IsKeyValid(CipherKey)) { LogMessage("Hill anahtarı geçersiz."); return null; }
                        return HillCipher.Encrypt(plainText, CipherKey);

                    default: return plainText;
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
            if (logBox.InvokeRequired) logBox.Invoke(new Action(() => LogMessage(message)));
            else
            {
                logBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\n");
                logBox.ScrollToCaret();
            }
        }
    }
}