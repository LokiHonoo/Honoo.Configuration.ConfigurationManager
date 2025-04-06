using Honoo.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;

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
                using (XConfigManager manager = new XConfigManager())
                {
                    manager.Default.Properties.AddOrUpdate("prop1", new XString("test.")).Comment.SetValue("test.");
                    manager.Default.Properties.AddOrUpdate("prop2", new XString("test.")).Encrypt(rsa);
                    manager.Default.Properties.AddOrUpdate("prop3", new XList()).Encrypt(rsa);
                    manager.Default.Properties.AddOrUpdate("prop4", new XDictionary()).Encrypt(rsa);
                    XSection section1 = manager.Sections.GetOrAdd("section1");
                    section1.Comment.SetValue("test.");
                    section1.Attributes.AddOrUpdate("attr1", new XConfigAttribute("test attr."));
                    section1.Properties.AddOrUpdate("prop1", new XString("test.")).Comment.SetValue("test.");
                    section1.Properties.AddOrUpdate("prop2", new XDictionary()).Properties.AddOrUpdate("prop3", new XString("test."));

                    Console.WriteLine("AAA --------------------------------------------------");
                    foreach (XNode node in manager.GetDocumentClone().DescendantNodes())
                    {
                        if (node.NodeType == System.Xml.XmlNodeType.Element)
                        {
                            Console.WriteLine(node.NodeType + "  " + ((XElement)node).Name.LocalName);
                        }
                        else
                        {
                            Console.WriteLine(node.NodeType);
                        }
                    }

                    section1.Encrypt(rsa);
                    manager.Save(writer);
                    writer.Close();

                    Console.WriteLine("BBB --------------------------------------------------");
                    foreach (XNode node in manager.GetDocumentClone().DescendantNodes())
                    {
                        if (node.NodeType == System.Xml.XmlNodeType.Element)
                        {
                            Console.WriteLine(node.NodeType + "  " + ((XElement)node).Name.LocalName);
                        }
                        else
                        {
                            Console.WriteLine(node.NodeType);
                        }
                    }
                }
            }
            Console.WriteLine(sb.ToString());
            Console.WriteLine();
            Console.WriteLine("CCC --------------------------------------------------");
            foreach (XNode node in XDocument.Parse(sb.ToString()).DescendantNodes())
            {
                if (node.NodeType == System.Xml.XmlNodeType.Element)
                {
                    Console.WriteLine(node.NodeType + "  " + ((XElement)node).Name.LocalName);
                }
                else
                {
                    Console.WriteLine(node.NodeType);
                }
            }
            //
            // 读取加密配置文件。加密方式和 ASP.NET 加密不兼容。
            //
            using (XmlReader reader = XmlReader.Create(new StringReader(sb.ToString())))
            {
                using (XConfigManager manager = new XConfigManager(reader))
                {
                    Console.WriteLine("DDD --------------------------------------------------");
                    foreach (XNode node in manager.GetDocumentClone().DescendantNodes())
                    {
                        if (node.NodeType == System.Xml.XmlNodeType.Element)
                        {
                            Console.WriteLine(node.NodeType + "  " + ((XElement)node).Name.LocalName);
                        }
                        else
                        {
                            Console.WriteLine(node.NodeType);
                        }
                    }

                    Console.WriteLine();
                    manager.Default.Properties.GetValue<XString>("prop2").Decrypt(rsa);
                    manager.Default.Properties.GetValue<XList>("prop3").Decrypt(rsa);
                    manager.Default.Properties.GetValue<XProperty>("prop4").Decrypt(rsa);
                    XSection section1 = manager.Sections.GetValue("section1");
                    section1.Decrypt(rsa);
                    section1.Attributes.GetValue("attr1");
                    section1.Properties.GetValue<XString>("prop1");
                    Console.WriteLine(manager.ToString());
                    Console.WriteLine();

                    Console.WriteLine("EEE --------------------------------------------------");
                    foreach (XNode node in manager.GetDocumentClone().DescendantNodes())
                    {
                        if (node.NodeType == System.Xml.XmlNodeType.Element)
                        {
                            Console.WriteLine(node.NodeType + "  " + ((XElement)node).Name.LocalName);
                        }
                        else
                        {
                            Console.WriteLine(node.NodeType);
                        }
                    }
                }
            }
        }
    }
}