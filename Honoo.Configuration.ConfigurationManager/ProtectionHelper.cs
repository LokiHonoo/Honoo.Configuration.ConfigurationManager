using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    internal static class ProtectionHelper
    {
        internal static XElement Decrypt(XElement root, RSACryptoServiceProvider rsa)
        {
            XElement keyElement = root.Element("EncryptedKey");
            string keyAlgorithm = keyElement.Attribute("Algorithm").Value;
            byte[] keyEncrypted = Convert.FromBase64String(keyElement.Element("CipherData").Value.Trim());
            XElement dataElement = root.Element("EncryptedData");
            string dataAlgorithm = dataElement.Attribute("Algorithm").Value;
            byte[] dataEncrypted = Convert.FromBase64String(dataElement.Element("CipherData").Value.Trim());
            byte[] key = null;
            byte[] data = null;
            switch (keyAlgorithm)
            {
                case "http://www.w3.org/2001/04/xmlenc#rsa-1_5": key = rsa.Decrypt(keyEncrypted, false); break;
                case "http://www.w3.org/2001/04/xmlenc#rsa-oaep-mgf1p": key = rsa.Decrypt(keyEncrypted, true); break;
                default: break;
            }
            switch (dataAlgorithm)
            {
                case "http://www.w3.org/2001/04/xmlenc#aes128-cbc":
                    using (AesCryptoServiceProvider algorithm = new AesCryptoServiceProvider() { KeySize = 128 })
                    {
                        byte[] iv = key;
                        data = Decrypt(algorithm, key, iv, dataEncrypted);
                    }
                    break;

                case "http://www.w3.org/2001/04/xmlenc#aes192-cbc":
                    using (AesCryptoServiceProvider algorithm = new AesCryptoServiceProvider() { KeySize = 192 })
                    {
                        byte[] iv = new byte[16];
                        Buffer.BlockCopy(key, 0, iv, 0, 16);
                        data = Decrypt(algorithm, key, iv, dataEncrypted);
                    }
                    break;

                case "http://www.w3.org/2001/04/xmlenc#aes256-cbc":
                    using (AesCryptoServiceProvider algorithm = new AesCryptoServiceProvider() { KeySize = 256 })
                    {
                        byte[] iv = new byte[16];
                        Buffer.BlockCopy(key, 0, iv, 0, 16);
                        data = Decrypt(algorithm, key, iv, dataEncrypted);
                    }
                    break;

                case "http://www.w3.org/2001/04/xmlenc#tripledes-cbc":
                    using (TripleDESCryptoServiceProvider algorithm = new TripleDESCryptoServiceProvider())
                    {
                        byte[] iv = new byte[8];
                        Buffer.BlockCopy(key, 0, iv, 0, 8);
                        data = Decrypt(algorithm, key, iv, dataEncrypted);
                    }
                    break;

                default: break;
            }
            return XElement.Parse(Encoding.UTF8.GetString(data));
        }

        internal static XElement Encrypt(XElement root, RSACryptoServiceProvider rsa)
        {
            byte[] data = Encoding.UTF8.GetBytes(root.ToString());
            byte[] dataEncrypted;
            byte[] key;
            byte[] keyEncrypted;
            string dataAlgorithm = "http://www.w3.org/2001/04/xmlenc#aes128-cbc";
            string keyAlgorithm = "http://www.w3.org/2001/04/xmlenc#rsa-1_5";
            using (AesCryptoServiceProvider algorithm = new AesCryptoServiceProvider() { KeySize = 128 })
            {
                key = algorithm.Key;
                byte[] iv = key;
                dataEncrypted = Encrypt(algorithm, key, iv, data);
            }
            keyEncrypted = rsa.Encrypt(key, false);
            //
            XElement keyElement = new XElement("EncryptedKey");
            keyElement.Add(new XAttribute("Algorithm", keyAlgorithm));
            keyElement.Add(new XElement("CipherData", Convert.ToBase64String(keyEncrypted)));

            XElement dataElement = new XElement("EncryptedData");
            dataElement.Add(new XAttribute("Algorithm", dataAlgorithm));
            dataElement.Add(new XElement("CipherData", Convert.ToBase64String(dataEncrypted)));

            XElement result = new XElement("configuration");
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

        private static byte[] Encrypt(SymmetricAlgorithm algorithm, byte[] key, byte[] iv, byte[] data)
        {
            using (var encryptor = algorithm.CreateEncryptor(key, iv))
            {
                return encryptor.TransformFinalBlock(data, 0, data.Length);
            }
        }
    }
}