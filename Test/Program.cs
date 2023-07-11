using Honoo.Configuration;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;

namespace Test
{
    internal class Program
    {
        private static void Main()
        {
            string filePath = Assembly.GetEntryAssembly().Location + ".config";
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
        }
    }
}