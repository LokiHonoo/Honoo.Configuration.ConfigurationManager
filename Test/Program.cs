using System;
using System.IO;
using System.Reflection;

namespace Test
{
    internal class Program
    {
        private static void Main()
        {
            string filePath = Assembly.GetEntryAssembly().Location + ".config";
            TestAppSettings.Create(filePath);
            Console.WriteLine(TestAppSettings.Load(filePath));
            TestConnectionStrings.Create(filePath);
            Console.WriteLine(TestConnectionStrings.Load(filePath));
            TestSection.Create(filePath);
            Console.WriteLine(TestSection.Load(filePath));
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(File.ReadAllText(filePath));
            Console.ReadKey(true);
        }
    }
}