using Honoo.Configuration;
using System;
using System.IO;

namespace Test
{
    /// <summary>
    /// 标准格式的 "appSettings" 节点。
    /// </summary>
    internal static class TestAppSettings
    {
        internal static void Create(string filePath)
        {
            //
            // 使用 .NET 程序的默认配置文件或自定义配置文件。
            //
            using (ConfigurationManager manager = File.Exists(filePath) ? new ConfigurationManager(filePath) : new ConfigurationManager())
            {
                manager.AppSettings.SetFileAttribute("config.exrea1.xml");
                //
                // 赋值并设置注释。
                //
                manager.AppSettings.Properties.AddOrUpdate("prop1", "It's will remove this.").SetComment("It's will remove this.");
                manager.AppSettings.Properties.AddOrUpdate("prop2", "This is \"appSettings\" prop2 value.").SetComment("This is \"appSettings\" prop2 comment.");
                manager.AppSettings.Properties.AddOrUpdate("prop3", 1234567);
                manager.AppSettings.Properties.AddOrUpdate("prop4", LoaderOptimization.SingleDomain);
                //
                // 移除属性的方法。移除属性时相关注释一并移除。
                //
                manager.AppSettings.Properties.Remove("prop1");
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
                // 取出属性和注释。
                //
                AddProperty value2 = manager.AppSettings.Properties.GetValue("prop2");
                if (value2.TryGetComment(out string comment2))
                {
                    Console.WriteLine(comment2);
                }
                Console.WriteLine(value2.Value);
                //
                int value3 = manager.AppSettings.Properties.GetValue("prop3", 55555);
                Console.WriteLine(value3);
                //
                if (manager.AppSettings.Properties.TryGetValue("prop4", out LoaderOptimization value4))
                {
                    Console.WriteLine(value4);
                }
                // 取出应用控制标签后的属性。
                DictionaryPropertySetControlled propertySetControlled = manager.AppSettings.GetPropertySetControlled();
                foreach (AddProperty property in propertySetControlled)
                {
                    Console.WriteLine(property.Value);
                }
            }
        }
    }
}