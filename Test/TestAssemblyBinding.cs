using Honoo.Configuration;
using System;

namespace Test
{
    /// <summary>
    /// 标准格式的 "assemblyBinding" 节点。
    /// </summary>
    internal static class TestAssemblyBinding
    {
        internal static void Create(string filePath)
        {
            //
            // 使用 .NET 程序的默认配置文件或自定义配置文件。
            //
            using (ConfigurationManager manager = new ConfigurationManager(filePath))
            {
                //
                // 赋值并设置注释。
                //
                while (manager.AssemblyBinding.Properties.Count < 3)
                {
                    manager.AssemblyBinding.Properties.Add("file://c:\\aa.tt").Comment.SetValue("This is \"linkedConfiguration\" comment.");
                }
                //
                // 移除属性的方法。
                //
                manager.AssemblyBinding.Properties.RemoveAt(0);
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
                LinkedConfigurationProperty value2 = manager.AssemblyBinding.Properties.GetValue(0);
                if (value2.Comment.TryGetValue(out string comment2))
                {
                    Console.WriteLine(comment2);
                }
                Console.WriteLine(value2.Href);
                //
                var a = manager.AssemblyBinding.Properties.GetValue(1);
                if (a.Comment.TryGetValue(out string comment3))
                {
                    Console.WriteLine(comment3);
                }
                Console.WriteLine(a.Href);
            }
        }
    }
}