﻿using System;

namespace Test
{
    internal class Program
    {
        private static void Main()
        {
            while (true)
            {
                TestC.Create();
                TestC.Load();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Press any key to test X.");
                Console.ReadKey(true);
                TestXX.Create();
                TestXX.Load();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Press any key to retest.");
                Console.ReadKey(true);
            }
        }

        //private static void Main()
        //{
        //    string filePath1 = "config.main.xml";
        //    string filePath5 = "honoo-settings.xml";
        //    bool clear = true;
        //    //
        //    // 清除内容。
        //    //
        //    if (clear)
        //    {
        //        using (Honoo.Configuration.ConfigurationManager manager = new Honoo.Configuration.ConfigurationManager(filePath1))
        //        {
        //            manager.Clear();
        //            manager.Save(filePath1);
        //        }
        //        using (Honoo.Configuration.XConfigManager manager = new Honoo.Configuration.XConfigManager(filePath5))
        //        {
        //            manager.Clear();
        //            manager.Save(filePath5);
        //        }
        //    }
        //    //
        //    //
        //    //
        //    TestAppSettingsExtra.Create();
        //    TestAppSettings.Create(filePath1);
        //    TestAppSettings.Load(filePath1);
        //    TestAssemblyBinding.Create(filePath1);
        //    TestAssemblyBinding.Load(filePath1);
        //    TestConnectionStrings.Create(filePath1);
        //    TestConnectionStrings.Load(filePath1);
        //    TestSection.Create(filePath1);
        //    TestSection.Load(filePath1);
        //    TestStream.Write(filePath1);
        //    TestStream.Load(filePath1);
        //    Console.WriteLine();
        //    Console.WriteLine();
        //    Console.WriteLine(File.ReadAllText(filePath1));
        //    Console.ReadKey(true);
        //    //
        //    Console.WriteLine();
        //    Console.WriteLine();
        //    Console.WriteLine();
        //    TestX.Create(filePath5);
        //    TestX.Load(filePath5);
        //    Console.WriteLine();
        //    Console.WriteLine();
        //    Console.WriteLine(File.ReadAllText(filePath5));
        //    Console.ReadKey(true);
        //    //
        //    Console.WriteLine();
        //    Console.WriteLine();
        //    Console.WriteLine();
        //    TestComment.Test();
        //    Console.ReadKey(true);
        //    //
        //    Console.WriteLine();
        //    Console.WriteLine();
        //    Console.WriteLine();
        //    TestProtection.Create();
        //    Console.ReadKey(true);
        //}
    }
}