using Honoo.Configuration;
using System.IO;
using System.Text;

namespace Test
{
    /// <summary>
    /// 流的读取保存。
    /// </summary>
    internal static class TestStream
    {
        internal static void Create(string filePath)
        {
            //
            // 使用配置文件流。
            //
            using (FileStream input = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (ConfigurationManager manager = new ConfigurationManager(input, true))
                {
                  
                    manager.AppSettings.Properties.AddOrUpdate("prop4", Common.Random.NextDouble().ToString());
                    manager.AppSettings.Properties["prop5"] = Common.Random.NextDouble().ToString();
                    //
                    // 保存到指定的流。
                    //
                    using (FileStream output = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
                    {
                        manager.Save(output);
                    }
                }
            }
        }

        internal static string Load(string filePath)
        {
            StringBuilder result = new StringBuilder();
            //
            // 使用配置文件流。
            //
            using (FileStream input = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (ConfigurationManager manager = new ConfigurationManager(input))
                {
                    //
                    // 取出属性。
                    //
                    if (manager.AppSettings.Properties.TryGetValue("prop4", out string value))
                    {
                        result.AppendLine(value);
                    }
                    value = manager.AppSettings.Properties["prop5"];
                    result.AppendLine(value);
                }
            }

            return result.ToString();
        }
    }
}