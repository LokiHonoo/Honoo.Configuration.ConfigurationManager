using Honoo.Configuration;
using System;
using System.Xml.Linq;

namespace Test
{
    /// <summary>
    ///
    /// </summary>
    internal static class TestX
    {
        internal static void Create(string filePath)
        {
            //
            // 使用自定义配置文件。
            //
            using (XConfigManager manager = new XConfigManager(filePath, true))
            {
                //
                // 赋值并设置注释。
                //
                manager.Default.Properties.AddOrUpdate("prop1", new XString(StringComparison.Ordinal.ToString())).Comment.SetValue("This is \"XCconfig\" prop1 comment.");
                manager.Default.Properties.AddOrUpdate("prop7", new XString("Update this."));
                var prop2 = manager.Default.Properties.AddOrUpdate("prop2", new XDictionary());
                prop2.Properties.AddOrUpdate("prop4", new XString("Sub this dictionary prop."));
                var prop3 = manager.Default.Properties.AddOrUpdate("prop3", new XList());
                prop3.Properties.Add(new XString("Sub this list prop.")).Comment.SetValue("This is \"XCconfig\" list prop comment.");
                XDictionary prop5 = prop3.Properties.Add(new XDictionary());
                XString prop55 = prop5.Properties.Add("prop5", new XString("F024AC4"));
                //
                // 附加属性。
                //
                prop5.Attributes.AddOrUpdate("attr1", new XConfigAttribute("add attr"));
                prop55.Attributes.AddOrUpdate("attr1", new XConfigAttribute("add attr"));
                //
                //
                //
                manager.Default.Properties.AddOrUpdate("prop6", new XString("Remove this."));
                //
                // manager.Default.Properties.Add("prop1", new XString("Test unique."));
                //
                // 移除属性的方法。移除属性时相关注释一并移除。
                //
                manager.Default.Properties.Remove("prop6");
                //
                // 更新。
                //
                manager.Default.Properties.AddOrUpdate("prop7", new XString("Update this successful."));
                //
                // 附加配置容器。
                //
                XDictionary section = manager.Sections.GetOrAdd("section1");
                section.Comment.SetValue("This is \"XCconfig\" section1");
                section.Properties.AddOrUpdate("prop1", new XString("123456789"));
                //
                // 保存到指定的文件。
                //
                manager.Save(filePath);

                foreach (XNode node in manager.CloneDocument().DescendantNodes())
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

        internal static void Load(string filePath)
        {
            //
            // 使用自定义配置文件。
            //
            using (XConfigManager manager = new XConfigManager(filePath))
            {
                foreach (XNode node in manager.CloneDocument().DescendantNodes())
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
                // 取出属性和注释。
                //
                XString value1 = manager.Default.Properties.GetValue<XString>("prop1");
                if (value1.Comment.TryGetValue(out string comment1))
                {
                    Console.WriteLine(comment1);
                }
                Console.WriteLine(value1.GetEnumValue<StringComparison>());
                //
                XDictionary value2 = manager.Default.Properties.GetValue<XDictionary>("prop2");
                value2.Properties.TryGetValue("prop4", out XString val2);
                Console.WriteLine(val2.GetStringValue());
                //
                XList value3 = manager.Default.Properties.GetValue<XList>("prop3");
                Console.WriteLine(value3.Properties.GetValue<XString>(0).GetStringValue());
                //
                XString value5 = ((XDictionary)value3.Properties[1]).Properties.GetValue<XString>("prop5");
                byte[] val5 = value5.GetBytesValue();
                Console.WriteLine(BitConverter.ToString(val5));
                Console.WriteLine(value5.Attributes.GetValue("attr1").GetStringValue());
                //
                XDictionary section = manager.Sections.GetValue("section1");
                Console.WriteLine(section.Comment.GetValue());
                Console.WriteLine(section.Properties.GetValue<XString>("prop1").GetInt64Value());
            }
        }
    }
}