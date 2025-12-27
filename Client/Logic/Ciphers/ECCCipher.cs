using System;
using System.Security.Cryptography;

namespace Client.Logic.Ciphers
{
    public static class ECCCipher
    {
        public static byte[] Encrypt(byte[] data, byte[] serverPublicKey)
        {
            using (ECDiffieHellman clientEcdh = ECDiffieHellman.Create(ECCurve.NamedCurves.nistP256))
            {
                byte[] clientPublicKey = clientEcdh.PublicKey.ExportSubjectPublicKeyInfo();

                using (ECDiffieHellman serverEcdh = ECDiffieHellman.Create())
                {
                    serverEcdh.ImportSubjectPublicKeyInfo(serverPublicKey, out _);

                    byte[] sharedSecret = clientEcdh.DeriveKeyMaterial(serverEcdh.PublicKey);

                    byte[] aesKey = new byte[16];
                    Array.Copy(sharedSecret, aesKey, 16);

                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = aesKey;
                        aes.GenerateIV();
                        aes.Mode = CipherMode.CBC;
                        aes.Padding = PaddingMode.PKCS7;

                        byte[] encrypted;
                        using (var encryptor = aes.CreateEncryptor())
                        {
                            encrypted = encryptor.TransformFinalBlock(data, 0, data.Length);
                        }

                        byte[] result = new byte[clientPublicKey.Length + 4 + aes.IV.Length + encrypted.Length];
                        int offset = 0;

                        byte[] pubKeyLen = BitConverter.GetBytes(clientPublicKey.Length);
                        Array.Copy(pubKeyLen, 0, result, offset, 4);
                        offset += 4;

                        Array.Copy(clientPublicKey, 0, result, offset, clientPublicKey.Length);
                        offset += clientPublicKey.Length;

                        Array.Copy(aes.IV, 0, result, offset, aes.IV.Length);
                        offset += aes.IV.Length;

                        Array.Copy(encrypted, 0, result, offset, encrypted.Length);

                        return result;
                    }
                }
            }
        }
    }
}
