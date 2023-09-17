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
                // 赋值。
                //
                while (manager.AssemblyBinding.Properties.Count < 3)
                {
                    manager.AssemblyBinding.Properties.Add("file://c:\\aa.tt");
                }
                //
                // 设置注释。
                //
                manager.AssemblyBinding.Properties.TrySetComment(0, "Comment");
                manager.AssemblyBinding.Properties.TrySetComment(1, "Comment");
                manager.AssemblyBinding.Properties.TrySetComment(2, "Comment");
                //
                // 移除属性的方法。
                //
                manager.AssemblyBinding.Properties.Remove(0);
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
                string value1 = manager.AssemblyBinding.Properties.GetValue(0, "This is default value when prop1 not fount.");
                Console.WriteLine(value1);
                string value2 = manager.AssemblyBinding.Properties.GetValue(1, "This is default value when prop2 not fount.");
                Console.WriteLine(value2);
                //
                // 取出注释。
                //
                if (manager.AssemblyBinding.Properties.TryGetComment(0, out string comment))
                {
                    Console.WriteLine(comment);
                }
                if (manager.AssemblyBinding.Properties.TryGetComment(1, out comment))
                {
                    Console.WriteLine(comment);
                }
            }
        }
    }
}