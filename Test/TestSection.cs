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
                // SingleTagSection 属性操作与 appSettings 节点相同。不支持属性值注释。
                //
                section1.Properties["prop1"] = 0.6789d.ToString();
                section1.SetComment("This is \"SingleTagSection\" comment.");
                //section1.SetComment("prop1", "This is \"SingleTagSection\" prop1  comment.");
                //
                // DictionarySection 属性操作和注释操作与 appSettings 节点相同。
                //
                section3.Properties.AddOrUpdate("prop1", new byte[] { 0x01, 0x02, 0x03, 0xAA, 0xBB, 0xCC, });
                section3.Properties.TrySetComment("prop1", "This is \"DictionarySection\" prop1 comment.");
                section3.SetComment("This is \"DictionarySection\" comment.");
                //
                // NameValueSection 属性以数组操作，注释需指定索引。
                //
                section2.Properties.AddOrUpdate("prop1", new double[] { 155.66d, 7.9992d });
                section2.Properties.TrySetComment("prop1", 0, "This is \"NameValueSection\" prop1 sub 0 comment.");
                section2.Properties.TrySetComment("prop1", 1, "This is \"NameValueSection\" prop1 sub 1 comment.");
                section2.SetComment("This is \"NameValueSection\" comment.");
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
                // 取出属性。
                //
                Console.WriteLine(section1.Properties.GetValue("prop1", string.Empty));
                Console.WriteLine(section3.Properties.GetValue("prop1", string.Empty));
                double[] value2 = section2.Properties.GetValues("prop1", new double[] { 11, 22, 33 });
                foreach (double v in value2)
                {
                    Console.WriteLine(v);
                }
                //
                // 取出注释。
                //
                if (section1.TryGetComment(out string comment))
                {
                    Console.WriteLine(comment);
                }
                if (section2.Properties.TryGetComment("prop1", 0, out comment))
                {
                    Console.WriteLine(comment);
                }
                if (section3.Properties.TryGetComment("prop1", out comment))
                {
                    Console.WriteLine(comment);
                }
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