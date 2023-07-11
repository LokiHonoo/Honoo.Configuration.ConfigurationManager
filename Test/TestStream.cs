using Honoo.Configuration;
using System.IO;

namespace Test
{
    /// <summary>
    /// 流的读取保存。
    /// </summary>
    internal static class TestStream
    {
        internal static void Load(string filePath)
        {
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
                    if (manager.AppSettings.Properties.TryGetValue("prop2", out string value))
                    {
                    }
                }
            }
        }

        internal static void Write(string filePath)
        {
            //
            // 使用配置文件流。
            //
            using (FileStream input = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (ConfigurationManager manager = new ConfigurationManager(input, true))
                {
                    //
                    // 保存到指定的流。
                    //
                    using (FileStream output = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
                    {
                        manager.Save(output);
                    }
                }
            }
        }
    }
}