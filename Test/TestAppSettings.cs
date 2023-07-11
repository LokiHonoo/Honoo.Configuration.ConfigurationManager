using Honoo.Configuration;
using System;

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
            using (ConfigurationManager manager = new ConfigurationManager(filePath))
            {
                //
                // 直接赋值必须转换为 string 类型。
                //
                manager.AppSettings.Properties["prop1"] = "It's will remove this.";
                manager.AppSettings.Properties.AddOrUpdate("prop2", "This is \"appSettings\" prop2 value.");
                manager.AppSettings.Properties.AddOrUpdate("prop3", 1234567);
                manager.AppSettings.Properties.AddOrUpdate("prop4", LoaderOptimization.SingleDomain);
                //
                // 设置注释。
                //
                manager.AppSettings.Properties.TrySetComment("prop1", "It's will remove this.");
                manager.AppSettings.Properties.TrySetComment("prop2", "This is \"appSettings\" prop2 comment.");
                manager.AppSettings.Properties.TrySetComment("prop3", "This is \"appSettings\" prop3 comment.");
                manager.AppSettings.Properties.TrySetComment("prop4", "This is \"appSettings\" prop4 comment.");
                //
                // 移除属性的方法。选择其一。移除属性时相关注释一并移除。
                //
                manager.AppSettings.Properties.Remove("prop1");
                manager.AppSettings.Properties["prop1"] = null;
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
                string value1 = manager.AppSettings.Properties.GetValue("prop1", "This is default value when prop1 not fount.");
                Console.WriteLine(value1);
                string value2 = manager.AppSettings.Properties.GetValue("prop2", "This is default value when prop2 not fount.");
                Console.WriteLine(value2);
                double value3 = manager.AppSettings.Properties.GetValue("prop3", 0.77777777d);
                Console.WriteLine(value3);
                LoaderOptimization value4 = manager.AppSettings.Properties.GetValue("prop4", LoaderOptimization.MultiDomainHost);
                Console.WriteLine(value4);
                //
                // 取出注释。
                //
                if (manager.AppSettings.Properties.TryGetComment("prop1", out string comment))
                {
                    Console.WriteLine(comment);
                }
                if (manager.AppSettings.Properties.TryGetComment("prop2", out comment))
                {
                    Console.WriteLine(comment);
                }
            }
        }
    }
}