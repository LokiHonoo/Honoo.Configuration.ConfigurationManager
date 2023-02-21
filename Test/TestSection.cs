using Honoo.Configuration;
using System;
using System.Collections.Generic;

namespace Test
{
    /// <summary>
    /// 标准格式的 "configSections" 节点。
    /// </summary>
    internal static class TestSection
    {
        internal static void Create(string filePath)
        {
            //
            // 使用 .NET 程序的默认配置文件或自定义配置文件。
            //
            using (ConfigurationManager manager = new ConfigurationManager(filePath))
            {
                //
                // 支持三种标准类型的创建。
                // System.Configuration.DictionarySectionHandler
                // System.Configuration.NameValueSectionHandler
                // System.Configuration.SingleTagSectionHandler
                //
                // 直接赋值等同于 AddOrUpdate 方法。
                //
                SingleTagSection section1 = (SingleTagSection)manager.ConfigSections.Sections.GetOrAdd("section1", ConfigSectionType.SingleTagSection);
                section1.Properties.AddOrUpdate("prop1", Common.Random.NextDouble().ToString());
                section1.Properties["prop2"] = Common.Random.NextDouble().ToString();
                NameValueSection section2 = (NameValueSection)manager.ConfigSections.Sections.GetOrAdd("section2", ConfigSectionType.NameValueSection);
                section2.Properties.AddOrUpdate("prop1", Common.Random.NextDouble().ToString());

                section2.Properties.AddOrUpdate (filePath, new string[3] );
                section2.Properties["prop2"] = Common.Random.NextDouble().ToString();
                section2.Properties.TrySetComment("prop1", "This is a name value section child");
                section2.Properties.TrySetComment("prop2", "This is a name value section child");
                //
                // 配置组和注释。
                //
                ConfigSectionGroup group = manager.ConfigSections.Groups.GetOrAdd("sectionGroup1");
                manager.ConfigSections.Groups.TrySetComment("sectionGroup1", "This is a section group");
                //
                // 配置容器和注释。
                //
                DictionarySection section3 = (DictionarySection)group.Sections.GetOrAdd("section3", ConfigSectionType.DictionarySection);
                group.Sections.TrySetComment("section3", "This is a dictionary section");
                //
                section3.Properties.AddOrUpdate("prop1", true);
                section3.Properties.AddOrUpdate("prop2", sbyte.MaxValue);
                section3.Properties.AddOrUpdate("prop3", byte.MaxValue);
                section3.Properties.AddOrUpdate("prop4", short.MaxValue);
                section3.Properties.AddOrUpdate("prop5", ushort.MaxValue);
                section3.Properties.AddOrUpdate("prop6", int.MaxValue);
                section3.Properties.AddOrUpdate("prop7", uint.MaxValue);
                section3.Properties["prop8"] = long.MaxValue;
                section3.Properties["prop9"] = ulong.MaxValue;
                section3.Properties["prop10"] = float.MaxValue / 2;
                section3.Properties["prop11"] = double.MaxValue / 2;
                section3.Properties["prop12"] = decimal.MaxValue;
                section3.Properties["prop13"] = (char)Common.Random.Next(65, 91);
                section3.Properties["prop14"] = new byte[] { 0x01, 0x02, 0x03, 0x0A, 0x0B, 0x0C };
                section3.Properties["prop15"] = "支持 15 种可序列化类型";
                section3.Properties.TrySetComment("prop15", "This is a dictionary section child");
                //
                // 以文本方式创建。
                //
                TextSection section4 = (TextSection)manager.ConfigSections.Sections.GetOrAdd("section4", ConfigSectionType.TextSection);
                section4.SetAttribute("attr1", "属性值");
                section4.SetValue("<arbitrarily>任意内容</arbitrarily>");
                manager.ConfigSections.Sections.TrySetComment("section4", "This is a text section");
                //
                // 保存到指定的文件。
                //
                manager.Save(filePath);
            }
        }

        internal static void Load(string filePath)
        {
            //
            // 使用 .NET 程序的默认配置文件或自定义配置文件。
            //
            using (ConfigurationManager manager = new ConfigurationManager(filePath))
            {
                //
                // 取出属性。
                //
                if (manager.ConfigSections.Sections.TryGetValue("section1", out SingleTagSection section1))
                {
                    foreach (KeyValuePair<string, string> prop in section1.Properties)
                    {
                        Console.WriteLine(prop.Value);
                    }
                }
                if (manager.ConfigSections.Sections.TryGetValue("section2", out NameValueSection section2))
                {
                    foreach (KeyValuePair<string, string> prop in section2.Properties)
                    {
                        Console.WriteLine(prop.Value);
                    }
                }
                if (manager.ConfigSections.Groups.TryGetValue("sectionGroup1", out ConfigSectionGroup group))
                {
                    if (group.Sections.TryGetValue("section3", out DictionarySection section3))
                    {
                        // 根据 type 参数返回强类型值。如果没有 type 参数，以 string 类型处理。
                        foreach (KeyValuePair<string, object> prop in section3.Properties)
                        {
                            Console.WriteLine($"{prop.Value.GetType().Name,-10}{prop.Value}");
                        }
                    }
                }
                if (manager.ConfigSections.Sections.TryGetValue("section4", out TextSection section4))
                {
                    Console.WriteLine(section4.GetXmlString());
                }
            }
        }
    }
}