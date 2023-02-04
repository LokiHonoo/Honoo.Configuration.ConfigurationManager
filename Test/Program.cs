using Honoo.Configuration;
using System;

namespace Test
{
    internal class Program
    {
        private static void Main()
        {
            Localization.EX_0X0001_InvalidKey = "无效的键。";
            Localization.EX_0X0002_InvalidType = "无效的类型。";
            Localization.EX_0X0003_DuplicateKey = "指定的键已存在。";
            //
            //
            //
            TestAppSettings.Create();
            Console.WriteLine(TestAppSettings.Load());
            TestConnectionStrings.Create();
            Console.WriteLine(TestConnectionStrings.Load());
            TestSection.Create();
            Console.WriteLine(TestSection.Load());
            Console.ReadKey(true);
        }
    }
}