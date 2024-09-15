using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    internal static class ProtectionHelper
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA5350:不要使用弱加密算法", Justification = "<挂起>")]
        internal static XElement Decrypt(XElement root, RSACryptoServiceProvider rsa)
        {
            XName name = root.Name;
            XNamespace ns = name.Namespace;
            XElement keyElement = root.Element(ns + "EncryptedKey");
            string keyAlgorithm = keyElement.Attribute("Algorithm").Value;
            byte[] keyEncrypted = Convert.FromBase64String(keyElement.Element(ns + "CipherData").Value.Trim());
            XElement dataElement = root.Element(ns + "EncryptedData");
            string dataAlgorithm = dataElement.Attribute("Algorithm").Value;
            byte[] dataEncrypted = Convert.FromBase64String(dataElement.Element(ns + "CipherData").Value.Trim());
            byte[] pms = null;
            byte[] data = null;
            switch (keyAlgorithm)
            {
                case "http://www.w3.org/2001/04/xmlenc#rsa-1_5": pms = rsa.Decrypt(keyEncrypted, false); break;
                case "http://www.w3.org/2001/04/xmlenc#rsa-oaep-mgf1p": pms = rsa.Decrypt(keyEncrypted, true); break;
                default: break;
            }
            switch (dataAlgorithm)
            {
                case "http://www.w3.org/2001/04/xmlenc#aes128-cbc":
                    using (AesCryptoServiceProvider algorithm = new AesCryptoServiceProvider() { KeySize = 128 })
                    {
                        byte[] key = new byte[16];
                        Buffer.BlockCopy(pms, 0, key, 0, 16);
                        byte[] iv = new byte[16];
                        Buffer.BlockCopy(pms, 16, iv, 0, 16);
                        data = Decrypt(algorithm, key, iv, dataEncrypted);
                    }
                    break;

                case "http://www.w3.org/2001/04/xmlenc#aes192-cbc":
                    using (AesCryptoServiceProvider algorithm = new AesCryptoServiceProvider() { KeySize = 192 })
                    {
                        byte[] key = new byte[24];
                        Buffer.BlockCopy(pms, 0, key, 0, 24);
                        byte[] iv = new byte[16];
                        Buffer.BlockCopy(pms, 24, iv, 0, 16);
                        data = Decrypt(algorithm, key, iv, dataEncrypted);
                    }
                    break;

                case "http://www.w3.org/2001/04/xmlenc#aes256-cbc":
                    using (AesCryptoServiceProvider algorithm = new AesCryptoServiceProvider() { KeySize = 256 })
                    {
                        byte[] key = new byte[32];
                        Buffer.BlockCopy(pms, 0, key, 0, 32);
                        byte[] iv = new byte[16];
                        Buffer.BlockCopy(pms, 32, iv, 0, 16);
                        data = Decrypt(algorithm, key, iv, dataEncrypted);
                    }
                    break;

                case "http://www.w3.org/2001/04/xmlenc#tripledes-cbc":
                    using (TripleDESCryptoServiceProvider algorithm = new TripleDESCryptoServiceProvider() { KeySize = 192 })
                    {
                        byte[] key = new byte[24];
                        Buffer.BlockCopy(pms, 0, key, 0, 24);
                        byte[] iv = new byte[8];
                        Buffer.BlockCopy(pms, 24, iv, 0, 8);
                        data = Decrypt(algorithm, key, iv, dataEncrypted);
                    }
                    break;

                default: break;
            }
            return XElement.Parse(Encoding.UTF8.GetString(data));
        }

        internal static XElement Encrypt(XElement root, RSACryptoServiceProvider rsa)
        {
            XName name = root.Name;
            XNamespace ns = name.Namespace;
            byte[] data = Encoding.UTF8.GetBytes(root.ToString());
            byte[] dataEncrypted;
            byte[] keyEncrypted;
            string dataAlgorithm = "http://www.w3.org/2001/04/xmlenc#aes128-cbc";
            string keyAlgorithm = "http://www.w3.org/2001/04/xmlenc#rsa-1_5";
            using (AesCryptoServiceProvider algorithm = new AesCryptoServiceProvider() { KeySize = 128 })
            {
                byte[] pms = new byte[16 + 16];
                Buffer.BlockCopy(algorithm.Key, 0, pms, 0, 16);
                Buffer.BlockCopy(algorithm.IV, 0, pms, 16, 16);
                keyEncrypted = rsa.Encrypt(pms, false);
                dataEncrypted = Encrypt(algorithm, data);
            }
            //
            XElement keyElement = new XElement(ns + "EncryptedKey");
            keyElement.Add(new XAttribute("Algorithm", keyAlgorithm));
            keyElement.Add(new XElement(ns + "CipherData", Convert.ToBase64String(keyEncrypted)));

            XElement dataElement = new XElement(ns + "EncryptedData");
            dataElement.Add(new XAttribute("Algorithm", dataAlgorithm));
            dataElement.Add(new XElement(ns + "CipherData", Convert.ToBase64String(dataEncrypted)));

            XElement result = new XElement(name);
            result.Add(new XAttribute("protected", true));
            result.Add(keyElement);
            result.Add(dataElement);
            return result;
        }

        private static byte[] Decrypt(SymmetricAlgorithm algorithm, byte[] key, byte[] iv, byte[] data)
        {
            using (var decryptor = algorithm.CreateDecryptor(key, iv))
            {
                return decryptor.TransformFinalBlock(data, 0, data.Length);
            }
        }

        private static byte[] Encrypt(AesCryptoServiceProvider algorithm, byte[] data)
        {
            using (var encryptor = algorithm.CreateEncryptor())
            {
                return encryptor.TransformFinalBlock(data, 0, data.Length);
            }
        }
    }
}