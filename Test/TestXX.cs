using Honoo.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;

namespace Test
{
    internal static class TestXX
    {
        private static bool _isFirst = true;
        private static RSA _rsa = RSA.Create();

        internal static void Create()
        {
            if (_isFirst)
            {
                File.Delete("config.x.xml");
                _isFirst = false;
            }
            _rsa.FromXmlString("<RSAKeyValue>" +
                "<Modulus>01KVE1o0XhPBx1RWTKtg2upDlX9BWemOr3smbevwwJP30vu9W7HRTiGnMG5mzn5/c8UxJnxLvHOajgga6qjR5gLKuGTezW2J4vz6Yd62gUp4CHfyUAGlP7Yz03jykeeImlpMe9DcCRNLu6SvxZZtr5y/zfAQS4m59vdb7EQtyNE=</Modulus>" +
                "<Exponent>AQAB</Exponent>" +
                "<P>8QXRPPlNF5S2dr338CawMPJIDT57Bl5y25FN3LRtur67YsjfhBrNnraArTXhtTqIVRaJHAAqj7amExeLXQOEow==</P>" +
                "<Q>4HRLylJdmCCOXB73ImqxnIwj+XeCUhHH3wqwIy6a0eBOD3aM1omarTsx4CwDR1PumwUDP7/yoYI4jsS22Fkf+w==</Q>" +
                "<DP>PYQllnFu2sDrHT15RcMqHyQHfdHfpo1+tUnN3LH0e8BopVrtqZMJviOIjiz9cbsBxu3citgEBPAyTRcqS9A1Dw==</DP>" +
                "<DQ>v6CKVGGdppOk9uE4/Rk6Kf07eXCewpKLodDuMBtJ8oUeH/WGqGMyu1MecdUht3Pg8liFPZgS/fC/eKRZtrvgoQ==</DQ>" +
                "<InverseQ>7urJ29tbe0EqRTjI6NkHQvTE3Sw66incJ4jtnfDHds5+vyBZADRnmnicPyuyG02+ujdcsuojxDg6JpBlog3PAg==</InverseQ>" +
                "<D>T/HI9dZFQ2XkBB2SvFSFQqwnPzIyLeqekSJcqm782E3iDk4wF7VQgmdW0Yqil/HhE5IBAxc4q6VsTdkhHa8aIGLLpMsks5rvCZrTIy+5x4zG/XkVIgxjQ2xOx3KR/xuGisxOzI+jgc6I8DGBw7tSaCVfynNqVe4jgGov7szGMBU=</D>" +
                "</RSAKeyValue>");
            //
            // 使用自定义配置文件。
            //
            using (XConfigManager manager = new XConfigManager("config.x.xml", true))
            {
                #region Default

                //
                // 添加或更新属性，并设置注释。
                //
                manager.Default.Properties.AddOrUpdate("prop1", new XString("prop1")).Attributes.AddOrUpdateString("attr1", "attr1");
                XDictionary dict1 = manager.Default.Properties.AddOrUpdate("dictionary1", new XDictionary());
                dict1.Comment.SetValue("This is dictionary1 comment.");
                dict1.Properties.AddOrUpdateString("dict-prop1", "dict-prop1").Comment.SetValue("This is dictionary1 prop1 comment.");
                dict1.Properties.AddOrUpdate("dict-prop2", new XString("dict-prop2"));
                XList list1 = dict1.Properties.AddOrUpdate("dict-prop3-list1", new XList());
                list1.Properties.Add(new XString("dict-prop3-list1-prop1"));
                manager.Default.Properties.AddOrUpdate("prop5", new XString("Update this.")).Comment.SetValue("Will be update prop5.");
                manager.Default.Properties.AddOrUpdate("prop6", new XString("Remove this.")).Comment.SetValue("Will be remove prop6.");
                //
                // 更新。
                //
                manager.Default.Properties.AddOrUpdateString("prop5", "Update this successful.").Comment.SetValue("Update prop5 successful.");
                //
                // 移除属性的方法。移除属性时相关注释一并移除。
                //
                manager.Default.Properties.Remove("prop6");
                //
                // 添加同名属性异常。
                //
                try
                {
                    manager.Default.Properties.Add("prop1", new XString("Test unique.")); // Allways error.
                }
                catch (Exception)
                {
                    manager.Default.Properties.AddOrUpdateString("prop99", "Add prop1 exception OK.");
                }

                #endregion Default

                #region Sections

                //
                // 添加或更新容器组，并设置注释。
                //
                manager.Sections.AddOrUpdate("section1");
                manager.Sections.AddOrUpdate("section2").Comment.SetValue("This is section2 comment."); ;
                manager.Sections.AddOrUpdate("section3");
                XSection section1 = manager.Sections.GetOrAdd("section1");
                section1.Comment.SetValue("This is section1 comment.");
                //
                // 添加或更新属性，并设置注释。
                //
                section1.Properties.AddOrUpdate("section1-dict1", new XDictionary()).Comment.SetValue("This is section1-dict1 comment.");
                section1.Properties.AddOrUpdate("section1-list1", new XList()).Comment.SetValue("This is section1-list1 comment.");
                section1.Properties.AddOrUpdate("section1-string1", new XString("section1-string1")).Comment.SetValue("This is section1-string1 comment.");
                //
                // 加密。
                //
                section1.Encrypt(_rsa);

                #endregion Sections

                //
                // 保存到指定的文件。
                //
                manager.Save("config.x.xml");
            }
        }

        internal static void Load()
        {
            using (XConfigManager manager = new XConfigManager("config.x.xml"))
            {
                Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine(manager.ToString());
                Console.WriteLine();
                Console.WriteLine();
                manager.Sections.GetValue("section1").Decrypt(_rsa);
                Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine(manager.ToString());
                manager.Save("config.x.xml");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                Console.WriteLine(File.ReadAllText("config.x.xml"));
            }
        }
    }
}