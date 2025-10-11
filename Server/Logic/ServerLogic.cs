using System;
using System.Collections.Generic;
using System.IO;
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

        public string SelectedCipher { get; set; } = "Sezar";
        public string CipherKey { get; set; } = "3";

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
                    AppendMessage("[+] Client bekleniyor...\n");

                    while (true)
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        lock (clientLock) clients.Add(client);
                        AppendMessage("[+] Yeni client bağlandı!");

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
                        string encryptedMsg = data.Substring(5);
                        string decrypted = Decrypt(encryptedMsg);
                        AppendMessage($"[Client] (Şifreli: {encryptedMsg}) → Çözülmüş: {decrypted}");
                    }
                    else if (data.StartsWith("IMAGE|") || data.StartsWith("AUDIO|") || data.StartsWith("VIDEO|"))
                    {
                        string[] parts = data.Split('|');
                        if (parts.Length < 3) continue;

                        string type = parts[0];
                        string fileName = parts[1];
                        string encryptedBase64 = data.Substring(type.Length + fileName.Length + 2);

                        string decryptedBase64 = Decrypt(encryptedBase64);

                        try
                        {
                            byte[] fileBytes = Convert.FromBase64String(decryptedBase64);
                            string savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Received_" + fileName);
                            File.WriteAllBytes(savePath, fileBytes);
                            AppendMessage($"[{type}] {fileName} alındı ve kaydedildi: {savePath}");
                        }
                        catch
                        {
                            AppendMessage($"[{type}] {fileName} alındı fakat çözümlenemedi (yanlış anahtar olabilir).");
                        }
                    }
                    else
                    {
                        AppendMessage("[!] Bilinmeyen veri alındı.");
                    }
                }
            }
            catch (Exception ex)
            {
                AppendMessage("[!] Client bağlantısı koptu: " + ex.Message);
            }
            finally
            {
                client.Close();
                lock (clientLock) clients.Remove(client);
                AppendMessage("[i] Client ayrıldı.");
            }
        }

        private string Decrypt(string input)
        {
            try
            {
                switch (SelectedCipher)
                {
                    case "Sezar":
                        if (int.TryParse(CipherKey, out int shift))
                            return CaesarCipher.Decrypt(input, shift);
                        AppendMessage("Sezar için geçerli bir sayı anahtarı girin.");
                        break;

                    case "Vigenere":
                        if (!string.IsNullOrWhiteSpace(CipherKey))
                            return VigenereCipher.Decrypt(input, CipherKey);
                        AppendMessage("Vigenere için anahtar boş olamaz.");
                        break;
                }
            }
            catch (Exception ex)
            {
                AppendMessage("[Hata] Deşifreleme hatası: " + ex.Message);
            }

            return input;
        }

        private void AppendMessage(string msg)
        {
            if (rtb.InvokeRequired)
                rtb.Invoke(new Action(() => AppendMessage(msg)));
            else
                rtb.AppendText(msg + Environment.NewLine);
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
            catch (Exception ex)
            {
                AppendMessage("[Hata] Server durdurulamadı: " + ex.Message);
            }
        }
    }
}
