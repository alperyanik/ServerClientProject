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
        private Thread listenThread;

        public string SelectedCipher { get; set; } = "Sezar";
        public string CipherKey { get; set; } = "3";
        public string IP { get; set; }
        public int Port { get; set; }

        public string ServerPublicKey { get; set; }
        public byte[] ServerECCPublicKey { get; set; }
        private byte[] sessionKey;
        private string currentAlgo;
        private bool currentKeyExchangeMethod = false;
        public bool UseManualMode { get; set; } = false;
        public bool UseECCKeyExchange { get; set; } = false;

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

                listenThread = new Thread(ListenForMessages);
                listenThread.IsBackground = true;
                listenThread.Start();

                byte[] reqData = Encoding.UTF8.GetBytes("REQ_PUB_KEY|");
                stream.Write(reqData, 0, reqData.Length);

                Thread.Sleep(100);

                byte[] reqEccData = Encoding.UTF8.GetBytes("REQ_ECC_KEY|");
                stream.Write(reqEccData, 0, reqEccData.Length);

                return true;
            }
            catch (Exception ex)
            {
                LogMessage("Bağlantı hatası: " + ex.Message);
                return false;
            }
        }


        private void ListenForMessages()
        {
            try
            {
                byte[] buffer = new byte[8192];
                int bytesRead;

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    if (message.StartsWith("COMMAND|PUBLIC_KEY|"))
                    {
                        string[] parts = message.Split('|');
                        if (parts.Length >= 3)
                        {
                            ServerPublicKey = parts[2];
                            LogMessage("[SİSTEM] Sunucu RSA Anahtarı alındı ve kaydedildi.");
                        }
                        continue;
                    }

                    if (message.StartsWith("COMMAND|ECC_PUBLIC_KEY|"))
                    {
                        string[] parts = message.Split('|');
                        if (parts.Length >= 3)
                        {
                            ServerECCPublicKey = Convert.FromBase64String(parts[2]);
                            LogMessage("[SİSTEM] Sunucu ECC Anahtarı alındı ve kaydedildi.");
                        }
                        continue;
                    }

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
                return;
            }

            string modeStr = UseManualMode ? "MANUAL" : "LIBRARY";
            string packet = $"TEXT|{SelectedCipher}|{CipherKey}|{modeStr}|{encryptedMsg}";

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


        private bool PerformHandshake(string algo, byte[] keyBytes)
        {
            try
            {
                if (UseECCKeyExchange)
                {
                    if (ServerECCPublicKey == null)
                    {
                        LogMessage("Hata: Sunucu ECC Anahtarı henüz gelmedi! Biraz bekleyip tekrar deneyin.");
                        return false;
                    }

                    byte[] encryptedKey = ECCCipher.Encrypt(keyBytes, ServerECCPublicKey);
                    string b64Key = Convert.ToBase64String(encryptedKey);

                    string packet = $"ECC_KEY_EXCHANGE|{algo}|{b64Key}";
                    byte[] data = Encoding.UTF8.GetBytes(packet);
                    stream.Write(data, 0, data.Length);

                    LogMessage($"[SİSTEM] Yeni {algo} anahtarı üretildi ve ECC ile sunucuya gönderildi.");
                    return true;
                }
                else
                {
                    if (string.IsNullOrEmpty(ServerPublicKey))
                    {
                        LogMessage("Hata: Sunucu RSA Anahtarı henüz gelmedi! Biraz bekleyip tekrar deneyin.");
                        return false;
                    }

                    byte[] encryptedKey = RSACipher.Encrypt(keyBytes, ServerPublicKey);
                    string b64Key = Convert.ToBase64String(encryptedKey);

                    string packet = $"KEY_EXCHANGE|{algo}|{b64Key}";
                    byte[] data = Encoding.UTF8.GetBytes(packet);
                    stream.Write(data, 0, data.Length);

                    LogMessage($"[SİSTEM] Yeni {algo} anahtarı üretildi ve RSA ile sunucuya gönderildi.");
                    return true;
                }
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
                        if (sessionKey == null || currentAlgo != "AES" || currentKeyExchangeMethod != UseECCKeyExchange)
                        {
                            sessionKey = UseManualMode ? ManualAESCipher.GenerateRandomKey() : AESCipher.GenerateRandomKey();
                            currentAlgo = "AES";
                            currentKeyExchangeMethod = UseECCKeyExchange;
                            if (!PerformHandshake("AES", sessionKey)) return null;

                            Thread.Sleep(50);
                        }
                        if (UseManualMode)
                            return ManualAESCipher.Encrypt(plainText, sessionKey);
                        else
                            return AESCipher.Encrypt(plainText, sessionKey);

                    case "DES":
                        if (sessionKey == null || currentAlgo != "DES" || currentKeyExchangeMethod != UseECCKeyExchange)
                        {
                            sessionKey = UseManualMode ? ManualDESCipher.GenerateRandomKey() : DESCipher.GenerateRandomKey();
                            currentAlgo = "DES";
                            currentKeyExchangeMethod = UseECCKeyExchange;
                            if (!PerformHandshake("DES", sessionKey)) return null;
                            Thread.Sleep(50);
                        }
                        if (UseManualMode)
                            return ManualDESCipher.Encrypt(plainText, sessionKey);
                        else
                            return DESCipher.Encrypt(plainText, sessionKey);


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