﻿using Honoo.Configuration;
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
                manager.AppSettings.Properties.AddOrUpdate("prop1", "This is \"appSettings\" prop1 value.").Comment.SetValue("This is \"appSettings\" prop1 comment.");
                manager.AppSettings.Properties.AddOrUpdate("prop6", "Update this.");
                manager.AppSettings.Properties.AddOrUpdate("prop2", 123456789);
                manager.AppSettings.Properties.AddOrUpdate("prop3", new Binaries(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }));
                manager.AppSettings.Properties.AddOrUpdate("prop4", LoaderOptimization.SingleDomain);
                manager.AppSettings.Properties.AddOrUpdate("prop5", "Remove this.");
                //manager.AppSettings.Properties.Add("prop1", "Test unique.");
                //
                // 移除属性的方法。移除属性时相关注释一并移除。
                //
                manager.AppSettings.Properties.Remove("prop5");
                //
                // 更新。
                //
                manager.AppSettings.Properties.AddOrUpdate("prop6", "Update this successful.");
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
                AddProperty value1 = manager.AppSettings.Properties.GetValue("prop1");
                if (value1.Comment.TryGetValue(out string comment1))
                {
                    Console.WriteLine(comment1);
                }
                Console.WriteLine(value1.Value);
                //
                int value2 = manager.AppSettings.Properties.GetValue("prop2", 55555);
                Console.WriteLine(value2);
                //
                AddProperty value3 = manager.AppSettings.Properties["prop3"];
                Console.WriteLine(value3.GetValue(Binaries.Empty));
                //
                if (manager.AppSettings.Properties.TryGetValue("prop4", out LoaderOptimization value4))
                {
                    Console.WriteLine(value4);
                }
                // 取出应用控制标签后的属性。
                foreach (AddProperty property in manager.AppSettings.GetPropertySetControlled())
                {
                    Console.WriteLine(property.Value);
                }
            }
        }
    }
}