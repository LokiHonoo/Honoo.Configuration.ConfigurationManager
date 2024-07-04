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
                manager.Properties.AddOrUpdate("prop1", "This is \"hoonoo-settings\" prop1 value.").SetComment("This is \"hoonoo-settings\" prop1 value.");
                manager.Properties.AddOrUpdate("prop2", new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
                manager.Properties.AddOrUpdate("prop3", 123456789);
                manager.Properties.AddOrUpdateArray("prop4", new int[] { 1, 2, 3, 4, 5 });
                manager.Properties.AddOrUpdate("prop5", "Remove this.");
                //
                // 移除属性的方法。移除属性时相关注释一并移除。
                //
                manager.Properties.Remove("prop5");
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
                HonooProperty value1 = manager.Properties.GetValue("prop1");
                if (value1.TryGetComment(out string comment1))
                {
                    Console.WriteLine(comment1);
                }
                Console.WriteLine(value1.GetValue(string.Empty));
                //
                HonooProperty value2 = manager.Properties.GetValue("prop2");
                value2.TryGetValue(out byte[] val2);
                Console.WriteLine(BitConverter.ToString(val2));
                //
                Console.WriteLine(manager.Properties.GetValue("prop3").GetValue(0));
                //
                manager.Properties.TryGetArrayValue("prop4", out int[] value4);
                foreach (var val4 in value4)
                {
                    Console.WriteLine(val4);
                }
            }
        }
    }
}