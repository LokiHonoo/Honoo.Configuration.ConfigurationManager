using Honoo.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;

namespace Test
{
    /// <summary>
    /// HoonooSettings。
    /// </summary>
    internal static class TestHonooSettings
    {
        internal static void Create(string filePath)
        {
            //
            // 使用自定义配置文件。
            //
            using (HonooSettingsManager manager = File.Exists(filePath) ? new HonooSettingsManager(filePath) : new HonooSettingsManager())
            {
                //
                // 赋值并设置注释。
                //
                manager.Default.Properties.AddOrUpdate("prop1", "This is \"hoonoo-settings\" prop1 value.").SetComment("This is \"hoonoo-settings\" prop1 value.");
                manager.Default.Properties.AddOrUpdate("prop2", new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
                manager.Default.Properties.AddOrUpdate("prop3", 123456789);
                manager.Default.Properties.AddOrUpdateArray("prop4", new int[] { 1, 2, 3, 4, 5 });
                manager.Default.Properties.AddOrUpdate("prop5", "Remove this.");
                //
                // 移除属性的方法。移除属性时相关注释一并移除。
                //
                manager.Default.Properties.Remove("prop5");
                //
                // 附加配置容器。
                //
                HonooSection section = manager.Sections.GetOrAdd("section1");
                section.SetComment("\"This is \"hoonoo-settings\" section1");
                section.Properties.AddOrUpdate("prop1", 123456789);
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
            using (HonooSettingsManager manager = new HonooSettingsManager(filePath))
            {
                //
                // 取出属性和注释。
                //
                HonooProperty value1 = manager.Default.Properties.GetValue("prop1");
                if (value1.TryGetComment(out string comment1))
                {
                    Console.WriteLine(comment1);
                }
                Console.WriteLine(value1.GetValue(string.Empty));
                //
                HonooProperty value2 = manager.Default.Properties.GetValue("prop2");
                value2.TryGetValue(out byte[] val2);
                Console.WriteLine(BitConverter.ToString(val2));
                //
                Console.WriteLine(manager.Default.Properties.GetValue("prop3").GetValue(0));
                //
                manager.Default.Properties.TryGetArrayValue("prop4", out int[] value4);
                foreach (var val4 in value4)
                {
                    Console.WriteLine(val4);
                }
                //
                HonooSection section = manager.Sections.GetValue("section1");
                Console.WriteLine(section.GetComment());
                Console.WriteLine(section.Properties.GetValue("prop1").Value);
            }
        }
    }
}