using Honoo.Configuration;
using System.IO;
using System.Text;

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
                manager.AppSettings.Properties.AddOrUpdate("prop1", Common.Random.NextDouble().ToString());
                manager.AppSettings.Properties["prop2"] = Common.Random.NextDouble().ToString();
                manager.AppSettings.Properties["prop3"] = "等待移除";
                //
                // 移除属性的方法。选择其一。
                //
                manager.AppSettings.Properties.Remove("prop3");
                manager.AppSettings.Properties["prop3"] = null;
                manager.AppSettings.Properties.AddOrUpdate("prop3", null);
                //
                // 保存到指定的文件。
                //
                manager.Save(filePath);
            }
        }

        internal static string Load(string filePath)
        {
            StringBuilder result = new StringBuilder();
            //
            // 使用 .NET 程序的默认配置文件或自定义配置文件。
            //
            using (ConfigurationManager manager = new ConfigurationManager(filePath))
            {
                //
                // 取出属性。
                //
                if (manager.AppSettings.Properties.TryGetValue("prop1", out string value))
                {
                    result.AppendLine(value);
                }
                value = manager.AppSettings.Properties["prop2"];
                result.AppendLine(value);
            }

            return result.ToString();
        }
    }
}