using Honoo.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace Test
{
    /// <summary>
    /// 标准格式的 "connectionStrings" 节点。
    /// </summary>
    internal static class TestConnectionStrings
    {
        internal static void Create(string filePath)
        {
            //
            // 使用 .NET 程序的默认配置文件或自定义配置文件。
            //
            using (ConfigurationManager manager = new ConfigurationManager(filePath))
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder()
                {
                    DataSource = "127.0.0.1",
                    InitialCatalog = "DemoCatalog",
                    UserID = "sa",
                    Password = "12345"
                };
                SqlConnection connection = new SqlConnection(builder.ConnectionString);
                //
                // 赋值并设置注释。如果不设置引擎参数，读取时不能访问连接实例。
                //
                manager.ConnectionStrings.Properties.AddOrUpdate("prop1", connection).SetComment("This is \"connectionStrings\" prop1 comment.");
                manager.ConnectionStrings.Properties.AddOrUpdate("prop2", connection.ConnectionString, connection.GetType().Namespace);
                manager.ConnectionStrings.Properties.AddOrUpdate("prop3", connection.ConnectionString, typeof(SqlConnection).AssemblyQualifiedName);
                manager.ConnectionStrings.Properties.AddOrUpdate("prop4", connection).SetComment("It's will remove this.");
                //
                // 移除属性的方法。
                //
                manager.ConnectionStrings.Properties.Remove("prop4");
                //
                // 保存到指定的文件。
                //
                manager.Save(filePath);
            }
        }

        internal static void Load(string filePath)
        {
            //
            // 使用 .NET 程序的默认配置文件或自定义配置文件。
            //
            using (ConfigurationManager manager = new ConfigurationManager(filePath))
            {
                //
                // 取出属性和注释。
                //
                ConnectionStringProperty value1 = manager.ConnectionStrings.Properties.GetValue("prop1");
                if (value1.TryGetComment(out string comment1))
                {
                    Console.WriteLine(comment1);
                }
                Console.WriteLine(value1.ConnectionString);
                //
                if (manager.ConnectionStrings.Properties.TryGetValue("prop2", out ConnectionStringProperty value2))
                {
                    Console.WriteLine(value2.ConnectionString);
                    //
                    // 访问实例。
                    //
                    SqlConnection connection = (SqlConnection)value2.CreateInstance();
                    Console.WriteLine(connection.ConnectionString);
                }
            }
        }
    }
}