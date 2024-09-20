using Honoo.Configuration;
using System;

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
                // 配置组。
                //
                ConfigSectionGroup group = manager.ConfigSections.Groups.GetOrAdd("sectionGroup1");
                group.Comment.SetValue("This is \"ConfigSectionGroup\" comment.");
                //
                // 配置容器。
                //
                SingleTagSection section1 = manager.ConfigSections.Sections.GetOrAdd<SingleTagSection>("section1");
                NameValueSection section2 = manager.ConfigSections.Sections.GetOrAdd<NameValueSection>("section2");
                DictionarySection section3 = group.Sections.GetOrAdd<DictionarySection>("section3");
                //
                // SingleTagSection 使用唯一键值。不支持属性值注释。
                //
                section1.Properties.AddOrUpdate("prop1", 0.6789d);
                section1.Comment.SetValue("This is \"SingleTagSection\" comment.");
                section1.Properties.Remove("prop3");
                section1.Properties.Add("prop3", "Update this.");
                section1.Properties.AddOrUpdate("prop2", "abc");
                //section1.Properties.Add("prop1", "Test unique.");
                //
                // 更新。
                //
                section1.Properties.AddOrUpdate("prop3", "Update this successful.");
                //
                // NameValueSection 允许同名键值。
                //
                section2.Properties.Clear();
                section2.Properties.Add("prop1", 155.66d).Comment.SetValue("This is \"NameValueSection\" prop1 comment.");
                section2.Properties.Add("prop1", 7.9992d).Comment.SetValue("This is \"NameValueSection\" prop1 comment.");
                section2.Comment.SetValue("This is \"NameValueSection\" comment.");
                //
                // DictionarySection 使用唯一键值。
                //
                section3.Properties.AddOrUpdate("prop1", "DictionarySection prop.").Comment.SetValue("This is \"DictionarySection\" prop1 comment.");
                section3.Comment.SetValue("This is \"DictionarySection\" comment.");

                //
                // 以文本方式创建。
                //
                TextSection section4 = manager.ConfigSections.Sections.GetOrAdd<TextSection>("section4");
                section4.SetAttribute("attr1", "attr1value");
                section4.SetValue("<!-- Comment --><arbitrarily>abc</arbitrarily><arbitrarily>def</arbitrarily>");
                section4.Comment.SetValue("This is \"TextSection\" comment.");

                TextSection section5 = manager.ConfigSections.Sections.GetOrAdd<TextSection>("section5");
                section5.SetValue("<![CDATA[<arbitrarily>abc</arbitrarily><arbitrarily>def</arbitrarily>]]>");

                TextSection section6 = manager.ConfigSections.Sections.GetOrAdd<TextSection>("section6");
                section6.SetValue("abcdefg");
                //
                //
                //
                NameValueSection section7 = manager.ConfigSections.Sections.GetOrAdd<NameValueSection>("section7");
                section7.Comment.SetValue("It's will remove this.");
                manager.ConfigSections.Sections.Remove("section7");
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
                // 取出容器。
                //
                ConfigSectionGroup group = manager.ConfigSections.Groups.GetOrAdd("sectionGroup1");
                SingleTagSection section1 = manager.ConfigSections.Sections.GetOrAdd<SingleTagSection>("section1");
                NameValueSection section2 = manager.ConfigSections.Sections.GetOrAdd<NameValueSection>("section2");
                DictionarySection section3 = group.Sections.GetOrAdd<DictionarySection>("section3");
                TextSection section4 = manager.ConfigSections.Sections.GetOrAdd<TextSection>("section4");
                TextSection section5 = manager.ConfigSections.Sections.GetOrAdd<TextSection>("section5");
                TextSection section6 = manager.ConfigSections.Sections.GetOrAdd<TextSection>("section6");
                //
                // 取出属性和注释。
                //
                Console.WriteLine(section1.Properties.GetValue("prop1", 0d));
                //
                AddProperty[] value2 = section2.Properties.GetValue("prop1");
                foreach (AddProperty val in value2)
                {
                    Console.WriteLine(val.Value);
                }
                Console.WriteLine(section3.Properties.GetValue("prop1", string.Empty));
                //
                // 以文本方式取出节点内容。
                //
                Console.WriteLine(section4.GetValue());
                Console.WriteLine(section5.GetValue());
                Console.WriteLine(section6.GetValue());
            }
        }
    }
}