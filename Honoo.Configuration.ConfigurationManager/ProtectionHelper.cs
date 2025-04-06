using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    internal static class ProtectionHelper
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA5350:不要使用弱加密算法", Justification = "<挂起>")]
        internal static XElement Decrypt(XElement element, RSA rsa)
        {
            XName name = element.Name;
            XNamespace ns = name.Namespace;
            XElement keyElement = element.Element(ns + "EncryptedKey");
            string keyAlgorithm = keyElement.Attribute("Algorithm").Value;
            byte[] keyEncrypted = Convert.FromBase64String(keyElement.Element(ns + "CipherData").Value.Trim());
            XElement dataElement = element.Element(ns + "EncryptedData");
            string dataAlgorithm = dataElement.Attribute("Algorithm").Value;
            byte[] dataEncrypted = Convert.FromBase64String(dataElement.Element(ns + "CipherData").Value.Trim());
            byte[] pms;
            byte[] data;
            switch (keyAlgorithm)
            {
                case "http://www.w3.org/2001/04/xmlenc#rsa-1_5":
#if NET40
                    pms = rsa.DecryptValue(keyEncrypted);
#else
                    pms = rsa.Decrypt(keyEncrypted, RSAEncryptionPadding.Pkcs1);
#endif
                    break;

                case "http://www.w3.org/2001/04/xmlenc#rsa-oaep-mgf1p":
                default: throw new CryptographicException($"Unknown encryption identifier -\"{keyAlgorithm}\".");
            }
            switch (dataAlgorithm)
            {
                case "http://www.w3.org/2001/04/xmlenc#aes128-cbc":
                    using (Aes algorithm = Aes.Create())
                    {
                        algorithm.KeySize = 128;
                        byte[] key = new byte[16];
                        Buffer.BlockCopy(pms, 0, key, 0, 16);
                        byte[] iv = new byte[16];
                        Buffer.BlockCopy(pms, 16, iv, 0, 16);
                        data = Decrypt(algorithm, key, iv, dataEncrypted);
                    }
                    break;

                case "http://www.w3.org/2001/04/xmlenc#aes192-cbc":
                    using (Aes algorithm = Aes.Create())
                    {
                        algorithm.KeySize = 192;
                        byte[] key = new byte[24];
                        Buffer.BlockCopy(pms, 0, key, 0, 24);
                        byte[] iv = new byte[16];
                        Buffer.BlockCopy(pms, 24, iv, 0, 16);
                        data = Decrypt(algorithm, key, iv, dataEncrypted);
                    }
                    break;

                case "http://www.w3.org/2001/04/xmlenc#aes256-cbc":
                    using (Aes algorithm = Aes.Create())
                    {
                        algorithm.KeySize = 256;
                        byte[] key = new byte[32];
                        Buffer.BlockCopy(pms, 0, key, 0, 32);
                        byte[] iv = new byte[16];
                        Buffer.BlockCopy(pms, 32, iv, 0, 16);
                        data = Decrypt(algorithm, key, iv, dataEncrypted);
                    }
                    break;

                case "http://www.w3.org/2001/04/xmlenc#tripledes-cbc":
                    using (TripleDES algorithm = TripleDES.Create())
                    {
                        algorithm.KeySize = 192;
                        byte[] key = new byte[24];
                        Buffer.BlockCopy(pms, 0, key, 0, 24);
                        byte[] iv = new byte[8];
                        Buffer.BlockCopy(pms, 24, iv, 0, 8);
                        data = Decrypt(algorithm, key, iv, dataEncrypted);
                    }
                    break;

                default: throw new CryptographicException($"Unknown encryption identifier -\"{dataAlgorithm}\".");
            }
            XElement result = XElement.Parse(Encoding.UTF8.GetString(data));
            result.Name = ns + result.Name.LocalName;
            foreach (XElement ele in result.Descendants())
            {
                ele.Name = ns + ele.Name.LocalName;
            }
            return result;
        }

        internal static XElement Encrypt(XElement element, RSA rsa)
        {
            XName name = element.Name;
            XAttribute idAttribute;
            if (element.Name == XConfigManager.Namespace + "default")
            {
                idAttribute = null;
            }
            else if (element.Name == XConfigManager.Namespace + "section")
            {
                idAttribute = element.Attribute("name");
            }
            else
            {
                idAttribute = element.Attribute("key");
            }
            XNamespace ns = name.Namespace;
            XElement elementR = new XElement(element);
            elementR.Name = elementR.Name.LocalName;
            foreach (XElement ele in elementR.Descendants())
            {
                ele.Name = ele.Name.LocalName;
            }
            byte[] data = Encoding.UTF8.GetBytes(elementR.ToString());
            byte[] dataEncrypted;
            byte[] keyEncrypted;
            string dataAlgorithm = "http://www.w3.org/2001/04/xmlenc#aes128-cbc";
            string keyAlgorithm = "http://www.w3.org/2001/04/xmlenc#rsa-1_5";
            using (Aes algorithm = Aes.Create())
            {
                algorithm.KeySize = 128;
                byte[] pms = new byte[16 + 16];
                Buffer.BlockCopy(algorithm.Key, 0, pms, 0, 16);
                Buffer.BlockCopy(algorithm.IV, 0, pms, 16, 16);
#if NET40
                keyEncrypted = rsa.EncryptValue(pms);
#else
                keyEncrypted = rsa.Encrypt(pms, RSAEncryptionPadding.Pkcs1);
#endif
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
            if (idAttribute != null)
            {
                result.Add(new XAttribute(idAttribute.Name.LocalName, idAttribute.Value));
            }
            result.Add(new XAttribute("protected", true));
            result.Add(keyElement);
            result.Add(dataElement);
            return result;
        }

        internal static bool QueryProtected(XElement content)
        {
            if (content.Attribute("protected") is XAttribute attribute)
            {
                if (bool.TryParse(attribute.Value, out bool isProtected))
                {
                    return isProtected;
                }
                else
                {
                    throw new CryptographicException($"Attribute \"protected\" is not a boolean value.");
                }
            }
            return false;
        }

        private static byte[] Decrypt(SymmetricAlgorithm algorithm, byte[] key, byte[] iv, byte[] data)
        {
            using (var decryptor = algorithm.CreateDecryptor(key, iv))
            {
                return decryptor.TransformFinalBlock(data, 0, data.Length);
            }
        }

        private static byte[] Encrypt(Aes algorithm, byte[] data)
        {
            using (var encryptor = algorithm.CreateEncryptor())
            {
                return encryptor.TransformFinalBlock(data, 0, data.Length);
            }
        }
    }
}