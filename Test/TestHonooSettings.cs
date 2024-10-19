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
                manager.Default.AddOrUpdate("prop1", new HonooString("This is \"hoonoo-settings\" prop1 value.")).Comment.SetValue("This is \"hoonoo-settings\" prop1 comment.");
                manager.Default.AddOrUpdate("prop7", new HonooString("Update this."));
                var prop2 = manager.Default.AddOrUpdate("prop2", new HonooDictionary());
                prop2.AddOrUpdate("prop4", new HonooString("Sub this."));
                var prop3 = manager.Default.AddOrUpdate("prop3", new HonooList());
                prop3.Add(new HonooString("Sub this.")).Comment.SetValue("This is \"hoonoo-settings\" list prop comment."); ;
                prop3.Add(new HonooDictionary() { { "prop5", new HonooString("F024AC4") } });
                manager.Default.AddOrUpdate("prop6", new HonooString("Remove this."));
                //manager.Default.Add("prop1", new HonooString("Test unique."));
                //
                // 移除属性的方法。移除属性时相关注释一并移除。
                //
                manager.Default.Remove("prop6");
                //
                // 更新。
                //
                manager.Default.AddOrUpdate("prop7", new HonooString("Update this successful."));
                //
                // 附加配置容器。
                //
                HonooDictionary section = manager.Sections.GetOrAdd("section1");
                section.Comment.SetValue("\"This is \"hoonoo-settings\" section1");
                section.AddOrUpdate("prop1", new HonooString("123456789"));
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
                HonooString value1 = manager.Default.GetValue<HonooString>("prop1");
                if (value1.Comment.TryGetValue(out string comment1))
                {
                    Console.WriteLine(comment1);
                }
                Console.WriteLine(value1.GetStringValue());
                //
                HonooDictionary value2 = manager.Default.GetValue<HonooDictionary>("prop2");
                value2.TryGetValue("prop4", out HonooString val2);
                Console.WriteLine(val2.GetStringValue());
                //
                HonooList value3 = manager.Default.GetValue<HonooList>("prop3");
                Console.WriteLine(value3[0]);
                //
                HonooString value5 = ((HonooDictionary)value3[1]).GetValue<HonooString>("prop5");
                byte[] val5 = value5.GetBytesValue();
                Console.WriteLine(BitConverter.ToString(val5));
                //
                HonooDictionary section = manager.Sections.GetValue("section1");
                Console.WriteLine(section.Comment.GetValue());
                Console.WriteLine(section.GetValue<HonooString>("prop1").GetInt64Value());
            }
        }
    }
}