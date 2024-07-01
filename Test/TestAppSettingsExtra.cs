using Honoo.Configuration;
using System.IO;

namespace Test
{
    /// <summary>
    /// 标准格式的 "appSettings" 节点的 "file" 属性指定的附加文件。
    /// </summary>
    internal static class TestAppSettingsExtra
    {
        internal static void Create()
        {
            string filePath1 = "config.exrea1.xml";
            string filePath2 = "config.exrea2.xml";
            using (AppSettingsManager manager = File.Exists(filePath1) ? new AppSettingsManager(filePath1) : new AppSettingsManager())
            {
                manager.SetFileAttribute(filePath2);
                manager.Properties.AddOrUpdate("extra1_1", "This is \"appSettings\" extra1_1 value.").SetComment("This is \"appSettings\" extra1_1 comment.");
                manager.Properties.AddOrUpdate("extra1_2", "This is \"appSettings\" extra1_2 value.").SetComment("This is \"appSettings\" extra1_2 comment.");
                manager.Save(filePath1);
            }
            using (AppSettingsManager manager = File.Exists(filePath2) ? new AppSettingsManager(filePath2) : new AppSettingsManager())
            {
                manager.Properties.AddOrUpdate("extra2_1", "This is \"appSettings\" extra2_1 value.").SetComment("This is \"appSettings\" extra2_1 comment.");
                manager.Properties.AddOrUpdate("extra2_2", "This is \"appSettings\" extra2_2 value.").SetComment("This is \"appSettings\" extra2_2 comment.");
                manager.Save(filePath2);
            }
        }
    }
}