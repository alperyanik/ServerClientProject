using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Server.Logic.Ciphers;

namespace Server.Logic
{
    public class ServerLogic
    {
        private TcpListener listener;
        private readonly List<TcpClient> clients = new List<TcpClient>();
        private readonly object clientLock = new object();
        private readonly RichTextBox rtb;

        private RSACipher rsaCipher;
        private ECCCipher eccCipher;

        public ServerLogic(RichTextBox rtbMessages)
        {
            rtb = rtbMessages;
            rsaCipher = new RSACipher();
            eccCipher = new ECCCipher();
        }

        public void StartServer(string ip, int port)
        {
            Thread serverThread = new Thread(() =>
            {
                try
                {
                    listener = new TcpListener(IPAddress.Parse(ip), port);
                    listener.Start();
                    AppendMessage($"[+] Server başlatıldı: {ip}:{port}");
                    AppendMessage($"[+] RSA Anahtarları (2048-bit) hazır.");
                    AppendMessage($"[+] ECC Anahtarları (NIST P-256) hazır.");

                    while (true)
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        lock (clientLock) clients.Add(client);
                        AppendMessage("[+] Yeni kullanıcı bağlandı!");

                        Thread clientThread = new Thread(() => HandleClient(client));
                        clientThread.IsBackground = true;
                        clientThread.Start();
                    }
                }
                catch (Exception ex)
                {
                    AppendMessage("[Hata] Server başlatılamadı: " + ex.Message);
                }
            });
            serverThread.IsBackground = true;
            serverThread.Start();
        }

        private void HandleClient(TcpClient client)
        {
            byte[] sessionKey = null;
            string sessionAlgo = "";

            try
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[8192];
                int bytesRead;

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    if (data.StartsWith("REQ_PUB_KEY|"))
                    {
                        string pubKeyXml = rsaCipher.GetPublicKey();
                        string response = "COMMAND|PUBLIC_KEY|" + pubKeyXml;

                        byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                        stream.Write(responseBytes, 0, responseBytes.Length);

                        AppendMessage("[Sistem] İstemciye RSA Public Key gönderildi.");
                        continue;
                    }

                    if (data.StartsWith("REQ_ECC_KEY|"))
                    {
                        byte[] eccPubKey = eccCipher.GetPublicKey();
                        string response = "COMMAND|ECC_PUBLIC_KEY|" + Convert.ToBase64String(eccPubKey);

                        byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                        stream.Write(responseBytes, 0, responseBytes.Length);

                        AppendMessage("[Sistem] İstemciye ECC Public Key gönderildi.");
                        continue;
                    }

                    if (data.StartsWith("KEY_EXCHANGE|"))
                    {
                        try
                        {
                            string[] parts = data.Split('|');
                            string algo = parts[1];
                            string encryptedKeyBase64 = parts[2];
                            byte[] encryptedBytes = Convert.FromBase64String(encryptedKeyBase64);

                            byte[] decryptedKey = rsaCipher.Decrypt(encryptedBytes);

                            if (decryptedKey != null)
                            {
                                sessionKey = decryptedKey;
                                sessionAlgo = algo;
                                AppendMessage($"[Sistem] RSA Handshake Başarılı! İstemci {algo} kullanacak.");
                            }
                            else
                            {
                                AppendMessage($"[Hata] RSA şifre çözme başarısız oldu.");
                            }
                        }
                        catch (Exception ex)
                        {
                            AppendMessage($"[Hata] Handshake paketi bozuk: {ex.Message}");
                        }
                        continue;
                    }

                    if (data.StartsWith("ECC_KEY_EXCHANGE|"))
                    {
                        try
                        {
                            string[] parts = data.Split('|');
                            string algo = parts[1];
                            string encryptedKeyBase64 = parts[2];
                            byte[] encryptedBytes = Convert.FromBase64String(encryptedKeyBase64);

                            byte[] decryptedKey = eccCipher.Decrypt(encryptedBytes);

                            if (decryptedKey != null)
                            {
                                sessionKey = decryptedKey;
                                sessionAlgo = algo;
                                AppendMessage($"[Sistem] ECC Handshake Başarılı! İstemci {algo} kullanacak.");
                            }
                            else
                            {
                                AppendMessage($"[Hata] ECC şifre çözme başarısız oldu.");
                            }
                        }
                        catch (Exception ex)
                        {
                            AppendMessage($"[Hata] ECC Handshake paketi bozuk: {ex.Message}");
                        }
                        continue;
                    }

                    if (data.StartsWith("TEXT|"))
                    {
                        string[] parts = data.Split('|');
                        if (parts.Length >= 5)
                        {
                            string cipherType = parts[1];
                            string key = parts[2];
                            string mode = parts[3];
                            string encryptedMsg = string.Join("|", parts, 4, parts.Length - 4);
                            bool isManual = (mode == "MANUAL");

                            string decrypted = DecryptMessage(encryptedMsg, cipherType, key, sessionKey, sessionAlgo, isManual);

                            string modeLabel = isManual ? "Manuel" : "Kütüphane";
                            AppendMessage($"--------------------------------------------------");
                            AppendMessage($"[Gelen] {cipherType} ({modeLabel})");
                            AppendMessage($"🔒 {encryptedMsg}");
                            AppendMessage($"🔓 {decrypted}");
                            AppendMessage($"--------------------------------------------------");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppendMessage("[!] Kullanıcı düştü: " + ex.Message);
            }
            finally
            {
                client.Close();
                lock (clientLock) clients.Remove(client);
            }
        }

        private string DecryptMessage(string input, string cipherType, string key, byte[] sessionKey, string sessionAlgo, bool isManual = false)
        {
            try
            {
                switch (cipherType)
                {
                    case "Sezar":
                        if (int.TryParse(key, out int shift)) return CaesarCipher.Decrypt(input, shift);
                        return "[Hata: Sezar anahtarı sayı olmalı]";
                    case "Vigenere": return VigenereCipher.Decrypt(input, key);
                    case "Substitution": return SubstitutionCipher.Decrypt(input, key);
                    case "Affine":
                        string[] affParts = key.Split(',');
                        if (affParts.Length == 2 && int.TryParse(affParts[0], out int a) && int.TryParse(affParts[1], out int b))
                            return AffineCipher.Decrypt(input, a, b);
                        return "[Hata]";
                    case "Playfair": return PlayfairCipher.Cipher(input, key, false);
                    case "RailFence":
                        if (int.TryParse(key, out int r)) return RailFenceCipher.Decrypt(input, r);
                        return "[Hata]";
                    case "Route":
                        if (int.TryParse(key, out int rk)) return RouteCipher.Decrypt(input, rk);
                        return "[Hata]";
                    case "Columnar": return ColumnarTranspositionCipher.Decrypt(input, key);
                    case "Polybius": return PolybiusCipher.Decrypt(input, key ?? "");
                    case "Hill": return HillCipher.Decrypt(input, key);

                    case "AES":
                        if (sessionKey == null || sessionAlgo != "AES")
                            return "[Hata: Sunucuda AES anahtarı yok. El sıkışma yapılmadı.]";
                        if (isManual)
                            return ManualAESCipher.Decrypt(input, sessionKey);
                        else
                            return AESCipher.Decrypt(input, sessionKey);

                    case "DES":
                        if (sessionKey == null || sessionAlgo != "DES")
                            return "[Hata: Sunucuda DES anahtarı yok. El sıkışma yapılmadı.]";
                        if (isManual)
                            return ManualDESCipher.Decrypt(input, sessionKey);
                        else
                            return DESCipher.Decrypt(input, sessionKey);

                    default: return input + " (Bilinmeyen Algoritma)";
                }
            }
            catch (Exception ex) { return $"[Çözme Hatası: {ex.Message}]"; }
        }

        private void AppendMessage(string msg)
        {
            if (rtb.IsDisposed) return;
            if (rtb.InvokeRequired) rtb.Invoke(new Action(() => AppendMessage(msg)));
            else
            {
                rtb.AppendText(msg + Environment.NewLine);
                rtb.ScrollToCaret();
            }
        }

        public void StopServer()
        {
            try
            {
                listener?.Stop();
                lock (clientLock)
                {
                    foreach (var c in clients) c.Close();
                    clients.Clear();
                }
                AppendMessage("[!] Server durduruldu.");
            }
            catch { }
        }
    }
}