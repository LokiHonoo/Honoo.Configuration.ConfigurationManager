using Honoo.Configuration;
using System;

namespace Test
{
    internal class Program
    {
        private static void Main()
        {
            Localization.InvalidKey = "无效的键。";
            Localization.DuplicateKey = "指定的键已存在。";
            Localization.InvalidType = "无效的类型。";
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