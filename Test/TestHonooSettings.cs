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
                manager.Default.Properties.AddOrUpdate("prop1", StringComparison.Ordinal.ToString()).Comment.SetValue("This is \"hoonoo-settings\" prop1 comment.");
                manager.Default.Properties.AddOrUpdate("prop7", "Update this.");
                var prop2 = manager.Default.Properties.AddOrUpdate("prop2", new HonooDictionary());
                prop2.Properties.AddOrUpdate("prop4", "Sub this dictionary prop.");
                var prop3 = manager.Default.Properties.AddOrUpdate("prop3", new HonooList());
                prop3.Properties.Add(new HonooString("Sub this list prop.")).Comment.SetValue("This is \"hoonoo-settings\" list prop comment."); ;
                HonooDictionary prop5 = prop3.Properties.Add(new HonooDictionary());
                prop5.Properties.Add("prop5", new HonooString("F024AC4"));
                manager.Default.Properties.AddOrUpdate("prop6", new HonooString("Remove this."));
                // manager.Default.Properties.Add("prop1", new HonooString("Test unique."));
                //
                // 移除属性的方法。移除属性时相关注释一并移除。
                //
                manager.Default.Properties.Remove("prop6");
                //
                // 更新。
                //
                manager.Default.Properties.AddOrUpdate("prop7", new HonooString("Update this successful."));
                //
                // 附加配置容器。
                //
                HonooDictionary section = manager.Sections.GetOrAdd("section1");
                section.Comment.SetValue("This is \"hoonoo-settings\" section1");
                section.Properties.AddOrUpdate("prop1", new HonooString("123456789"));
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
                HonooString value1 = manager.Default.Properties.GetValue<HonooString>("prop1");
                if (value1.Comment.TryGetValue(out string comment1))
                {
                    Console.WriteLine(comment1);
                }
                Console.WriteLine(value1.GetEnumValue<StringComparison>());
                //
                HonooDictionary value2 = manager.Default.Properties.GetValue<HonooDictionary>("prop2");
                value2.Properties.TryGetValue("prop4", out HonooString val2);
                Console.WriteLine(val2.GetStringValue());
                //
                HonooList value3 = manager.Default.Properties.GetValue<HonooList>("prop3");
                Console.WriteLine(value3.Properties.GetValue(0).GetStringValue());
                //
                HonooString value5 = ((HonooDictionary)value3.Properties[1]).Properties.GetValue<HonooString>("prop5");
                byte[] val5 = value5.GetBytesValue();
                Console.WriteLine(BitConverter.ToString(val5));
                //
                HonooDictionary section = manager.Sections.GetValue("section1");
                Console.WriteLine(section.Comment.GetValue());
                Console.WriteLine(section.Properties.GetValue<HonooString>("prop1").GetInt64Value());
            }
        }
    }
}