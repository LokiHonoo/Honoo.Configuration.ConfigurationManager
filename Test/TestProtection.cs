using Honoo.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Test
{
    /// <summary>
    /// 加密读写。
    /// </summary>
    internal static class TestProtection
    {
        internal static void Create()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            XmlWriterSettings writerSettings = new XmlWriterSettings() { Indent = true, Encoding = new UTF8Encoding(false) };
            RSA rsa = RSA.Create();
            rsa.FromXmlString("<RSAKeyValue>" +
                "<Modulus>01KVE1o0XhPBx1RWTKtg2upDlX9BWemOr3smbevwwJP30vu9W7HRTiGnMG5mzn5/c8UxJnxLvHOajgga6qjR5gLKuGTezW2J4vz6Yd62gUp4CHfyUAGlP7Yz03jykeeImlpMe9DcCRNLu6SvxZZtr5y/zfAQS4m59vdb7EQtyNE=</Modulus>" +
                "<Exponent>AQAB</Exponent>" +
                "<P>8QXRPPlNF5S2dr338CawMPJIDT57Bl5y25FN3LRtur67YsjfhBrNnraArTXhtTqIVRaJHAAqj7amExeLXQOEow==</P>" +
                "<Q>4HRLylJdmCCOXB73ImqxnIwj+XeCUhHH3wqwIy6a0eBOD3aM1omarTsx4CwDR1PumwUDP7/yoYI4jsS22Fkf+w==</Q>" +
                "<DP>PYQllnFu2sDrHT15RcMqHyQHfdHfpo1+tUnN3LH0e8BopVrtqZMJviOIjiz9cbsBxu3citgEBPAyTRcqS9A1Dw==</DP>" +
                "<DQ>v6CKVGGdppOk9uE4/Rk6Kf07eXCewpKLodDuMBtJ8oUeH/WGqGMyu1MecdUht3Pg8liFPZgS/fC/eKRZtrvgoQ==</DQ>" +
                "<InverseQ>7urJ29tbe0EqRTjI6NkHQvTE3Sw66incJ4jtnfDHds5+vyBZADRnmnicPyuyG02+ujdcsuojxDg6JpBlog3PAg==</InverseQ>" +
                "<D>T/HI9dZFQ2XkBB2SvFSFQqwnPzIyLeqekSJcqm782E3iDk4wF7VQgmdW0Yqil/HhE5IBAxc4q6VsTdkhHa8aIGLLpMsks5rvCZrTIy+5x4zG/XkVIgxjQ2xOx3KR/xuGisxOzI+jgc6I8DGBw7tSaCVfynNqVe4jgGov7szGMBU=</D>" +
                "</RSAKeyValue>");
            StringBuilder sb = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(sb, writerSettings))
            {
                //
                // 读取加密配置文件。加密配置文件和 .NET 程序的默认配置文件不兼容。
                //
                using (ConfigurationManager manager = new ConfigurationManager())
                {
                    manager.AppSettings.Properties.AddOrUpdate("prop1", new AddProperty("This is \"protection\" test.")).Comment.SetValue("This is \"protection\" test.");
                    //
                    // 加密方式保存到指定的文件。
                    //
                    manager.Save(writer, rsa);
                    writer.Close();
                }
            }
            Console.WriteLine(sb.ToString());
            Console.WriteLine();
            //
            //
            //
            using (XmlReader reader = XmlReader.Create(new StringReader(sb.ToString())))
            {
                using (ConfigurationManager manager = new ConfigurationManager(reader, rsa))
                {
                    Console.WriteLine(manager.GetXmlString());
                }
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            //
            //
            //
            sb = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(sb, writerSettings))
            {
                using (XConfigManager manager = new XConfigManager())
                {
                    manager.Default.Properties.AddOrUpdate("prop1", new XString("This is \"protection\" test.")).Comment.SetValue("This is \"protection\" test.");
                    //
                    // 加密方式保存到指定的文件。
                    //
                    manager.Save(writer, rsa);
                    writer.Close();
                }
            }
            Console.WriteLine(sb.ToString());
            Console.WriteLine();
            //
            // 读取加密配置文件。加密配置文件和 .NET 程序的默认配置文件不兼容。
            //
            using (XmlReader reader = XmlReader.Create(new StringReader(sb.ToString())))
            {
                using (XConfigManager manager = new XConfigManager(reader, rsa))
                {
                    Console.WriteLine(manager.GetXmlString());
                }
            }
        }
    }
}