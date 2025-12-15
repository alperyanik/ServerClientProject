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

        public ServerLogic(RichTextBox rtbMessages)
        {
            rtb = rtbMessages;
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
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[8192];
                int bytesRead;

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    if (data.StartsWith("TEXT|"))
                    {
                        string[] parts = data.Split('|');

                        if (parts.Length >= 4)
                        {
                            string cipherType = parts[1];
                            string key = parts[2];
                            string encryptedMsg = string.Join("|", parts, 3, parts.Length - 3);

                            string decrypted = DecryptMessage(encryptedMsg, cipherType, key);

                            AppendMessage($"--------------------------------------------------");
                            AppendMessage($"[Gelen] Algoritma: {cipherType} | Anahtar: {key}");
                            AppendMessage($"🔒 Şifreli: {encryptedMsg}");
                            AppendMessage($"🔓 Çözülmüş: {decrypted}");
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

        private string DecryptMessage(string input, string cipherType, string key)
        {
            try
            {
                switch (cipherType)
                {
                    case "Sezar":
                        if (int.TryParse(key, out int shift))
                            return CaesarCipher.Decrypt(input, shift);
                        return "[Hata: Sezar anahtarı sayı olmalı]";

                    case "Vigenere":
                        return VigenereCipher.Decrypt(input, key);


                    case "Substitution":
                        return SubstitutionCipher.Decrypt(input, key);

                    case "Affine":
                        string[] affParts = key.Split(',');
                        if (affParts.Length == 2 && int.TryParse(affParts[0], out int affA) && int.TryParse(affParts[1], out int affB))
                        {
                            return AffineCipher.Decrypt(input, affA, affB);
                        }
                        return "[Hata: Affine anahtarı bozuk]";

                    case "Playfair":
                        
                        return PlayfairCipher.Cipher(input, key, false);

                    case "RailFence":
                        if (int.TryParse(key, out int rKey))
                            return RailFenceCipher.Decrypt(input, rKey);
                        return "[Hata: RailFence anahtarı sayı olmalı]";

                    case "Route":
                        if (int.TryParse(key, out int routeKey))
                            return RouteCipher.Decrypt(input, routeKey);
                        return "[Hata: Route anahtarı sayı olmalı]";

                    case "Columnar":
                        return ColumnarTranspositionCipher.Decrypt(input, key);

                    case "Polybius":
                        return PolybiusCipher.Decrypt(input, key ?? "");

                    case "Hill":
                        return HillCipher.Decrypt(input, key);

                    default:
                        return input + " (Bilinmeyen Algoritma)";
                }
            }
            catch (Exception ex)
            {
                return $"[Çözme Hatası: {ex.Message}]";
            }
        }

        private void AppendMessage(string msg)
        {
            if (rtb.InvokeRequired)
            {
                rtb.Invoke(new Action(() => AppendMessage(msg)));
            }
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