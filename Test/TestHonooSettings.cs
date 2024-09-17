using Honoo.Configuration;
using System;
using System.IO;

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
                manager.Default.Properties.AddOrUpdate("prop1", "This is \"hoonoo-settings\" prop1 value.").SetComment("This is \"hoonoo-settings\" prop1 comment.");
                manager.Default.Properties.Remove("prop7");
                manager.Default.Properties.Add("prop7", "Update this.");
                manager.Default.Properties.AddOrUpdate("prop2", new Binaries(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }));
                manager.Default.Properties.AddOrUpdate("prop3", 123456789);
                manager.Default.Properties.AddOrUpdate("prop4", new int[] { 1, 2, 3, 4, 5 });
                manager.Default.Properties.AddOrUpdate("prop5", "Remove this.");
                var md = new long[][][] {
                    new long[][] {
                        new long[] { 1, 2, 3 },
                    },
                    new long[][] {
                        new long[] { 4, 5 },
                    },
                    new long[][] {
                        new long[] { 6, 7, 8, 9 },
                    },
                };
                manager.Default.Properties.AddOrUpdate("prop6", md);
                //manager.Default.Properties.Add("prop1", "Test unique.");
                //
                // 移除属性的方法。移除属性时相关注释一并移除。
                //
                manager.Default.Properties.Remove("prop5");
                //
                // 更新。
                //
                manager.Default.Properties.AddOrUpdate("prop7", "Update this successful.");
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
            // 使用自定义配置文件。
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
                value2.TryGetValue(out Binaries val2);
                Console.WriteLine(val2.Hex);
                //
                Console.WriteLine(manager.Default.Properties.GetValue("prop3").GetValue(0));
                //
                manager.Default.Properties.TryGetValue("prop4", out int[] value4);
                foreach (var val4 in value4)
                {
                    Console.WriteLine(val4);
                }
                //
                manager.Default.Properties.TryGetValue("prop6", out string[][][] _);
                //
                HonooSection section = manager.Sections.GetValue("section1");
                Console.WriteLine(section.GetComment());
                Console.WriteLine(section.Properties.GetValue("prop1").Value);
            }
        }
    }
}