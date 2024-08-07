﻿using System;
using System.IO;

namespace Test
{
    internal class Program
    {
        private static void Main()
        {
            string filePath1 = "config.main.xml";
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
            TestAppSettingsExtra.Create();
            TestAppSettings.Create(filePath1);
            TestAppSettings.Load(filePath1);
            TestAssemblyBinding.Create(filePath1);
            TestAssemblyBinding.Load(filePath1);
            TestConnectionStrings.Create(filePath1);
            TestConnectionStrings.Load(filePath1);
            TestSection.Create(filePath1);
            TestSection.Load(filePath1);
            TestStream.Write(filePath1);
            TestStream.Load(filePath1);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(File.ReadAllText(filePath1));
            Console.ReadKey(true);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            //
            string filePath5 = "honoo-settings.xml";
            TestHonooSettings.Create(filePath5);
            TestHonooSettings.Load(filePath5);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(File.ReadAllText(filePath5));
            Console.ReadKey(true);
            //
            TestProtection.Create();
            Console.ReadKey(true);
        }
    }
}