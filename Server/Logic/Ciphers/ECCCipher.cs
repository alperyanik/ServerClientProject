using System;
using System.Security.Cryptography;

namespace Server.Logic.Ciphers
{
    public class ECCCipher
    {
        private ECDiffieHellman _ecdh;

        public ECCCipher()
        {
            _ecdh = ECDiffieHellman.Create(ECCurve.NamedCurves.nistP256);
        }

        public byte[] GetPublicKey()
        {
            return _ecdh.PublicKey.ExportSubjectPublicKeyInfo();
        }

        public byte[] Decrypt(byte[] encryptedPackage)
        {
            try
            {
                int offset = 0;

                int clientPubKeyLen = BitConverter.ToInt32(encryptedPackage, offset);
                offset += 4;

                byte[] clientPublicKey = new byte[clientPubKeyLen];
                Array.Copy(encryptedPackage, offset, clientPublicKey, 0, clientPubKeyLen);
                offset += clientPubKeyLen;

                byte[] iv = new byte[16];
                Array.Copy(encryptedPackage, offset, iv, 0, 16);
                offset += 16;

                byte[] encrypted = new byte[encryptedPackage.Length - offset];
                Array.Copy(encryptedPackage, offset, encrypted, 0, encrypted.Length);

                using (ECDiffieHellman clientEcdh = ECDiffieHellman.Create())
                {
                    clientEcdh.ImportSubjectPublicKeyInfo(clientPublicKey, out _);

                    byte[] sharedSecret = _ecdh.DeriveKeyMaterial(clientEcdh.PublicKey);

                    byte[] aesKey = new byte[16];
                    Array.Copy(sharedSecret, aesKey, 16);

                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = aesKey;
                        aes.IV = iv;
                        aes.Mode = CipherMode.CBC;
                        aes.Padding = PaddingMode.PKCS7;

                        using (var decryptor = aes.CreateDecryptor())
                        {
                            return decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);
                        }
                    }
                }
            }
            catch (CryptographicException)
            {
                return null;
            }
        }
    }
}
