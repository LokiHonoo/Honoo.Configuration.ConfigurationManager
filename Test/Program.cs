﻿using System;
using System.IO;

namespace Test
{
    internal class Program
    {
        private static void Main()
        {
            string filePath1 = "config.main.xml";
            string filePath5 = "honoo-settings.xml";
            bool clear = false;
            //
            // 清除内容。
            //
            if (clear)
            {
                using (Honoo.Configuration.ConfigurationManager manager = new Honoo.Configuration.ConfigurationManager(filePath1))
                {
                    manager.Clear();
                    manager.Save(filePath1);
                }
                using (Honoo.Configuration.HonooSettingsManager manager = new Honoo.Configuration.HonooSettingsManager(filePath5))
                {
                    manager.Clear();
                    manager.Save(filePath5);
                }
            }
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