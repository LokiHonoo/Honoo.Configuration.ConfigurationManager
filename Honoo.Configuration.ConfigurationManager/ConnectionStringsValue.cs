using System;
using System.Data.Common;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 连接属性。
    /// </summary>
    public sealed class ConnectionStringsValue
    {
        private readonly string _connectionString;
        private readonly string _providerName;

        /// <summary>
        /// 获取连接字符串。
        /// </summary>
        public string ConnectionString => _connectionString;

        /// <summary>
        /// 获取数据库引擎的文本名称。如果连接属性没有数据库引擎参数，值是 null。
        /// </summary>
        public string ProviderName => _providerName;

        #region Construction

        /// <summary>
        /// 创建 ConnectionStringsValue 的新实例。
        /// </summary>
        /// <param name="connection">数据库连接实例。</param>
        public ConnectionStringsValue(DbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }
            _connectionString = connection.ConnectionString;
            _providerName = connection.GetType().Namespace;
        }

        /// <summary>
        /// 创建 ConnectionStringsValue 的新实例。
        /// </summary>
        /// <param name="connectionString">连接字符串。</param>
        /// <param name="providerName">数据库引擎的文本名称。</param>
        public ConnectionStringsValue(string connectionString, string providerName)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _providerName = providerName;
        }

        internal ConnectionStringsValue(XElement content)
        {
            _connectionString = content.Attribute("connectionString").Value;
            _providerName = content.Attribute("providerName")?.Value;
        }

        #endregion Construction

        /// <summary>
        /// 创建连接实例。如果连接属性中没有数据库引擎参数，或者工程没有引用相关类库，将引发异常。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public DbConnection CreateInstance()
        {
            Type type;
            switch (_providerName)
            {
                case "System.Data.Odbc":
                case "System.Data.Odbc.OdbcConnection":
                case "System.Data.Odbc.OdbcConnection, System.Data.Odbc":
                    type = Type.GetType("System.Data.Odbc.OdbcConnection, System.Data.Odbc");
                    if (type == null)
                    {
                        type = Type.GetType("System.Data.Odbc.OdbcConnection, System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
                    }
                    break;

                case "System.Data.OleDb":
                case "System.Data.OleDb.OleDbConnection":
                case "System.Data.OleDb.OleDbConnection, System.Data.OleDb":
                    type = Type.GetType("System.Data.OleDb.OleDbConnection, System.Data.OleDb");
                    if (type == null)
                    {
                        type = Type.GetType("System.Data.OleDb.OleDbConnection, System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
                    }
                    break;

                case "System.Data.SqlClient":
                case "System.Data.SqlClient.SqlConnection":
                case "System.Data.SqlClient.SqlConnection, System.Data.SqlClient":
                    type = Type.GetType("System.Data.SqlClient.SqlConnection, System.Data.SqlClient");
                    if (type == null)
                    {
                        type = Type.GetType("System.Data.SqlClient.SqlConnection, System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
                    }
                    break;

                case "System.Data.SqlServerCe":
                case "System.Data.SqlServerCe.SqlCeConnection":
                case "System.Data.SqlServerCe.SqlCeConnection, System.Data.SqlServerCe":
                    type = Type.GetType("System.Data.SqlServerCe.SqlCeConnection, System.Data.SqlServerCe");
                    break;

                case "System.Data.EntityClient":
                case "System.Data.EntityClient.EntityConnection":
                case "System.Data.EntityClient.EntityConnection, System.Data.EntityClient":
                    type = Type.GetType("System.Data.EntityClient.EntityConnection, System.Data.EntityClient");
                    break;

                case "System.Data.OracleClient":
                case "System.Data.OracleClient.OracleConnection":
                case "System.Data.OracleClient.OracleConnection, System.Data.OracleClient":
                    type = Type.GetType("System.Data.OracleClient.OracleConnection, System.Data.OracleClient");
                    break;

                case "System.Data.SQLite":
                case "System.Data.SQLite.SQLiteConnection":
                case "System.Data.SQLite.SQLiteConnection, System.Data.SQLite":
                    type = Type.GetType("System.Data.SQLite.SQLiteConnection, System.Data.SQLite");
                    break;

                case "Microsoft.Data.SqlClient":
                case "Microsoft.Data.SqlClient.SqlConnection":
                case "Microsoft.Data.SqlClient.SqlConnection, Microsoft.Data.SqlClient":
                    type = Type.GetType("Microsoft.Data.SqlClient.SqlConnection, Microsoft.Data.SqlClient");
                    break;

                case "Microsoft.SqlServerCe.Client":
                case "Microsoft.SqlServerCe.Client.SqlCeConnection":
                case "Microsoft.SqlServerCe.Client.SqlCeConnection, Microsoft.SqlServerCe.Client":
                    type = Type.GetType("Microsoft.SqlServerCe.Client.SqlCeConnection, Microsoft.SqlServerCe.Client");
                    break;

                case "Microsoft.Data.Sqlite":
                case "Microsoft.Data.Sqlite.SqliteConnection":
                case "Microsoft.Data.Sqlite.SqliteConnection, Microsoft.Data.Sqlite":
                    type = Type.GetType("Microsoft.Data.Sqlite.SqliteConnection, Microsoft.Data.Sqlite");
                    break;

                case "Oracle.DataAccess.Client":
                case "Oracle.DataAccess.Client.OracleConnection":
                case "Oracle.DataAccess.Client.OracleConnection, Oracle.DataAccess.Client":
                    type = Type.GetType("Oracle.DataAccess.Client.OracleConnection, Oracle.DataAccess.Client");
                    break;

                case "MySql.Data.MySqlClient":
                case "MySql.Data.MySqlClient.MySqlConnection":
                case "MySql.Data.MySqlClient.MySqlConnection, MySql.Data":
                    type = Type.GetType("MySql.Data.MySqlClient.MySqlConnection, MySql.Data");
                    break;

                case "MySqlConnector":
                case "MySqlConnector.MySqlConnection":
                case "MySqlConnector.MySqlConnection, MySqlConnector":
                    type = Type.GetType("MySqlConnector.MySqlConnection, MySqlConnector");
                    break;

                default: type = Type.GetType(_providerName); break;
            }
            return (DbConnection)Activator.CreateInstance(type, _connectionString);
        }
    }
}