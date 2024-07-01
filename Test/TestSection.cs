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
                group.SetComment("This is \"ConfigSectionGroup\" comment.");
                //
                // 配置容器。
                //
                SingleTagSection section1 = (SingleTagSection)manager.ConfigSections.Sections.GetOrAdd("section1", ConfigSectionKind.SingleTagSection);
                NameValueSection section2 = (NameValueSection)manager.ConfigSections.Sections.GetOrAdd("section2", ConfigSectionKind.NameValueSection);
                DictionarySection section3 = (DictionarySection)group.Sections.GetOrAdd("section3", ConfigSectionKind.DictionarySection);
                //
                // SingleTagSection 使用唯一键值。不支持属性值注释。
                //
                section1.Properties.AddOrUpdate("prop1", 0.6789d);
                section1.SetComment("This is \"SingleTagSection\" comment.");
                section1.Properties.AddOrUpdate("prop2", "abc");
                //
                // NameValueSection 允许同名键值。
                //
                section2.Properties.Clear();
                section2.Properties.Add("prop1", 155.66d).SetComment("This is \"NameValueSection\" prop1 comment.");
                section2.Properties.Add("prop1", 7.9992d).SetComment("This is \"NameValueSection\" prop1 comment.");
                section2.SetComment("This is \"NameValueSection\" comment.");
                //
                // DictionarySection 使用唯一键值。
                //
                section3.Properties.AddOrUpdate("prop1", "DictionarySection prop.").SetComment("This is \"DictionarySection\" prop1 comment.");
                section3.SetComment("This is \"DictionarySection\" comment.");

                //
                // 以文本方式创建。
                //
                TextSection section4 = (TextSection)manager.ConfigSections.Sections.GetOrAdd("section4", ConfigSectionKind.TextSection);
                section4.SetAttribute("attr1", "attr1value");
                section4.SetValue("<!-- Comment --><arbitrarily>abc</arbitrarily><arbitrarily>def</arbitrarily>");
                section4.SetComment("This is \"TextSection\" comment.");

                TextSection section5 = (TextSection)manager.ConfigSections.Sections.GetOrAdd("section5", ConfigSectionKind.TextSection);
                section5.SetValue("<![CDATA[<arbitrarily>abc</arbitrarily><arbitrarily>def</arbitrarily>]]>");

                TextSection section6 = (TextSection)manager.ConfigSections.Sections.GetOrAdd("section6", ConfigSectionKind.TextSection);
                section6.SetValue("abcdefg");
                //
                //
                //
                NameValueSection section7 = (NameValueSection)manager.ConfigSections.Sections.GetOrAdd("section7", ConfigSectionKind.NameValueSection);
                section7.SetComment("It's will remove this.");
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
                SingleTagSection section1 = (SingleTagSection)manager.ConfigSections.Sections.GetOrAdd("section1", ConfigSectionKind.SingleTagSection);
                NameValueSection section2 = (NameValueSection)manager.ConfigSections.Sections.GetOrAdd("section2", ConfigSectionKind.NameValueSection);
                DictionarySection section3 = (DictionarySection)group.Sections.GetOrAdd("section3", ConfigSectionKind.DictionarySection);
                TextSection section4 = (TextSection)manager.ConfigSections.Sections.GetOrAdd("section4", ConfigSectionKind.TextSection);
                TextSection section5 = (TextSection)manager.ConfigSections.Sections.GetOrAdd("section5", ConfigSectionKind.TextSection);
                TextSection section6 = (TextSection)manager.ConfigSections.Sections.GetOrAdd("section6", ConfigSectionKind.TextSection);
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