using System;
using System.Data.SqlClient;
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
            TestAppSettings.Load(filePath);
            TestConnectionStrings.Create(filePath);
            TestConnectionStrings.Load(filePath);
            TestSection.Create(filePath);
            TestSection.Load(filePath);
            TestStream.Create(filePath);
            TestStream.Load(filePath);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(File.ReadAllText(filePath));
            Console.ReadKey(true);
        }
    }
}