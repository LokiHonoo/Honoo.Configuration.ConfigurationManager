using System;
using System.IO;

namespace Test
{
    internal class Program
    {
        private static void Main()
        {
            string filePath = "test.config.xml";
            //
            // 清除内容。
            //
            //using (ConfigurationManager manager = new ConfigurationManager(filePath))
            //{
            //    manager.AppSettings.Properties.Clear();
            //    manager.ConnectionStrings.Properties.Clear();
            //    manager.ConfigSections.Groups.Clear();
            //    manager.ConfigSections.Sections.Clear();
            //    manager.Save(filePath);
            //}
            //
            //
            //
            TestAppSettings.Create(filePath);
            TestAppSettings.Load(filePath);
            TestAssemblyBinding.Create(filePath);
            TestAssemblyBinding.Load(filePath);
            TestConnectionStrings.Create(filePath);
            TestConnectionStrings.Load(filePath);
            TestSection.Create(filePath);
            TestSection.Load(filePath);
            TestStream.Write(filePath);
            TestStream.Load(filePath);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(File.ReadAllText(filePath));
            Console.ReadKey(true);
            //
            string filePath2 = "protection.config.xml";
            TestProtection.Create(filePath2);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(File.ReadAllText(filePath2));
            Console.ReadKey(true);
        }
    }
}