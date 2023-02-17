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
            bool exists = File.Exists(filePath);
            using (ConfigurationManager manager = exists ? new ConfigurationManager(filePath) : new ConfigurationManager())
            {
                //
                // 直接赋值等同于 AddOrUpdate 方法。
                //
                manager.AppSettings.Properties["prop1"] = Common.Random.NextDouble().ToString();
                manager.AppSettings.Properties.AddOrUpdate("prop2", Common.Random.NextDouble().ToString());
                manager.AppSettings.Properties.AddOrUpdate("prop3", Common.Random.NextDouble().ToString());
                //
                // 设置注释。
                //
                manager.AppSettings.Properties.TrySetComment("prop1", string.Empty);
                manager.AppSettings.Properties.TrySetComment("prop2", "This is \"appSettings\" prop2 comment");
                manager.AppSettings.Properties.TrySetComment("prop3", null);
                //
                // 移除属性的方法。选择其一。移除属性时相关注释一并移除。
                //
                manager.AppSettings.Properties.Remove("prop1");
                manager.AppSettings.Properties["prop1"] = null;
                manager.AppSettings.Properties.AddOrUpdate("prop1", null);
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
                if (manager.AppSettings.Properties.TryGetValue("prop2", out string value))
                {
                    Console.WriteLine(value);
                }
                value = manager.AppSettings.Properties["prop3"];
                Console.WriteLine(value);
                //
                // 取出注释。
                //
                if (manager.AppSettings.Properties.TryGetComment("prop2", out string comment))
                {
                    Console.WriteLine(comment);
                }
            }
        }
    }
}