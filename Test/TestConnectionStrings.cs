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
                SqlConnectionStringBuilder builder1 = new SqlConnectionStringBuilder()
                {
                    DataSource = "127.0.0.1",
                    InitialCatalog = "DemoCatalog",
                    UserID = "sa",
                    Password = "12345"
                };
                SqlConnection conn1 = new SqlConnection(builder1.ConnectionString);
                MySqlConnectionStringBuilder builder2 = new MySqlConnectionStringBuilder()
                {
                    Server = "127.0.0.1",
                    Database = "DemoDB",
                    UserID = "root",
                    Password = "12345"
                };
                MySqlConnection conn2 = new MySqlConnection(builder2.ConnectionString);
                //
                // 直接赋值等同于 AddOrUpdate 方法。不设置引擎参数，读取时不能访问连接实例。
                //
                manager.ConnectionStrings.Properties["prop1"] = new ConnectionStringsValue(conn1.ConnectionString, null);
                manager.ConnectionStrings.Properties["prop2"] = new ConnectionStringsValue(conn1);
                manager.ConnectionStrings.Properties.AddOrUpdate("prop3", conn1);
                manager.ConnectionStrings.Properties.AddOrUpdate("prop4", conn2.ConnectionString, conn2.GetType().Namespace);
                manager.ConnectionStrings.Properties.AddOrUpdate("prop5", conn2.ConnectionString, typeof(MySqlConnection).AssemblyQualifiedName);
                //
                // 设置注释。
                //
                manager.ConnectionStrings.Properties.TrySetComment("prop1", string.Empty);
                manager.ConnectionStrings.Properties.TrySetComment("prop2", "This is \"sql server connection\" comment");
                manager.ConnectionStrings.Properties.TrySetComment("prop3", null);
                manager.ConnectionStrings.Properties.TrySetComment("prop4", "This is \"mysql connection\" comment ");
                manager.ConnectionStrings.Properties.TrySetComment("prop5", "This is \"mysql connection\" comment used assembly qualified name");
                //
                // 移除属性的方法。选择其一。移除属性时相关注释一并移除。
                //
                manager.ConnectionStrings.Properties.Remove("prop1");
                manager.ConnectionStrings.Properties["prop1"] = null;
                manager.ConnectionStrings.Properties.AddOrUpdate("prop1", (DbConnection)null);
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
                // 取出属性。
                //
                if (manager.ConnectionStrings.Properties.TryGetValue("prop2", out ConnectionStringsValue value))
                {
                    Console.WriteLine(value.ConnectionString);
                }
                string connectionString = manager.ConnectionStrings.Properties["prop3"].ConnectionString;
                Console.WriteLine(connectionString);
                //
                // 访问实例。
                //
                DbConnection connection = manager.ConnectionStrings.Properties["prop4"].CreateInstance();
                Console.WriteLine(connection.ConnectionString);

                MySqlConnection mysql = (MySqlConnection)manager.ConnectionStrings.Properties["prop5"].CreateInstance();
                Console.WriteLine(mysql.ConnectionString);
            }
        }
    }
}