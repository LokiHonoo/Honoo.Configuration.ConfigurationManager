using Honoo.Configuration;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

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
                // 直接赋值等同于 AddOrUpdate 方法。
                //
                manager.ConnectionStrings.Properties.AddOrUpdate("prop1", conn1);
                manager.ConnectionStrings.Properties.AddOrUpdate("prop2", conn2.ConnectionString, null);
                manager.ConnectionStrings.Properties.AddOrUpdate("prop3", conn2.ConnectionString, typeof(MySqlConnection).Namespace);
                manager.ConnectionStrings.Properties.AddOrUpdate("prop4", conn2.ConnectionString, typeof(MySqlConnection).AssemblyQualifiedName);
                //
                // 不设置引擎参数，读取时不能直接创建连接实例。
                //
                manager.ConnectionStrings.Properties["prop5"] = new ConnectionStringsValue(conn2.ConnectionString, string.Empty);
                //
                // 移除属性的方法。选择其一。
                //
                manager.ConnectionStrings.Properties.Remove("prop5");
                manager.ConnectionStrings.Properties["prop5"] = null;
                manager.ConnectionStrings.Properties.AddOrUpdate("prop5", (DbConnection)null);
                //
                // 保存到创建实例时指定的文件。
                //
                manager.Save();
            }
        }

        internal static string Load(string filePath)
        {
            StringBuilder result = new StringBuilder();
            //
            // 使用 .NET 程序的默认配置文件或自定义配置文件。
            //
            using (ConfigurationManager manager = new ConfigurationManager(filePath))
            {
                //
                // 取出属性。
                //
                if (manager.ConnectionStrings.Properties.TryGetValue("prop1", out ConnectionStringsValue value))
                {
                    result.AppendLine(value.Connection.ConnectionString);
                }
                //
                // 不访问 Connection，属性内部没有实例化 Connection。项目没有引用相关数据库引擎时使用。
                //
                string connectionString = manager.ConnectionStrings.Properties["prop2"].ConnectionString;
                result.AppendLine(connectionString);
                //
                DbConnection connection = manager.ConnectionStrings.Properties["prop3"].Connection;
                result.AppendLine(connection.ConnectionString);

                MySqlConnection mysql = (MySqlConnection)manager.ConnectionStrings.Properties["prop4"].Connection;
                result.AppendLine(mysql.ConnectionString);
            }
            return result.ToString();
        }
    }
}