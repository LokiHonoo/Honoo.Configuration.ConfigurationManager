//using System;
//using System.Data.Common;
//using System.Xml.Linq;

//namespace Honoo.Configuration
//{
//    /// <summary>
//    /// 配置属性。
//    /// </summary>
//    public sealed class AppSettingsProperty
//    {
//        private readonly string _comment;
//        private readonly XElement _content;
//        private readonly string _value;

//        /// <summary>
//        /// 获取连接字符串。
//        /// </summary>
//        public string Comment => _comment;

//        /// <summary>
//        /// 获取数据库引擎的文本名称。如果连接属性没有数据库引擎参数，值是 null。
//        /// </summary>
//        public string Value => _value;

//        #region Construction

//        /// <summary>
//        /// 创建 AppSettingsProperty 的新实例。
//        /// </summary>
//        /// <param name="connectionString">连接字符串。</param>
//        /// <param name="providerName">数据库引擎的文本名称。</param>
//        public AppSettingsProperty(string connectionString, string providerName)
//        {
//            if (string.IsNullOrWhiteSpace(connectionString))
//            {
//                throw new ArgumentException($"The invalid connection string - {nameof(connectionString)}.");
//            }
//            _connectionString = connectionString;
//            _providerName = providerName;
//            _content = new XElement("add");
//            _content.SetAttributeValue("name", "newConnectionStringsValue");
//            _content.SetAttributeValue("connectionString", connectionString);
//            _content.SetAttributeValue("providerName", providerName);
//        }

//        internal AppSettingsProperty(XElement content)
//        {
//            _content = content;
//            _connectionString = content.Attribute("connectionString").Value;
//            _providerName = content.Attribute("providerName")?.Value;
//        }

//        #endregion Construction

//        /// <summary>
//        /// 确定指定的对象是否等于当前对象。
//        /// </summary>
//        /// <param name="obj">要与当前对象进行比较的对象。</param>
//        /// <returns></returns>
//        public override bool Equals(object obj)
//        {
//            return obj is AppSettingsProperty other && _content.Equals(other._content);
//        }

//        /// <summary>
//        /// 作为默认哈希函数。
//        /// </summary>
//        /// <returns></returns>
//        public override int GetHashCode()
//        {
//            return _content.GetHashCode();
//        }

//        /// <summary>
//        /// 方法已重写。返回节点的缩进 XML 文本。
//        /// </summary>
//        /// <returns></returns>
//        public override string ToString()
//        {
//            return _content.ToString();
//        }

//        internal void Remove()
//        {
//            _content.Remove();
//        }

//        private static DbConnection CreateInstance(string providerName, string connectionString)
//        {
//            Type type;
//            switch (providerName)
//            {
//                case "System.Data.Odbc":
//                    type = Type.GetType("System.Data.Odbc.OdbcConnection, System.Data.Odbc");
//                    if (type is null)
//                    {
//                        type = Type.GetType("System.Data.Odbc.OdbcConnection, System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
//                    }
//                    break;

//                case "System.Data.OleDb":
//                    type = Type.GetType("System.Data.OleDb.OleDbConnection, System.Data.OleDb");
//                    if (type is null)
//                    {
//                        type = Type.GetType("System.Data.OleDb.OleDbConnection, System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
//                    }
//                    break;

//                case "System.Data.SqlClient":
//                    type = Type.GetType("System.Data.SqlClient.SqlConnection, System.Data.SqlClient");
//                    if (type is null)
//                    {
//                        type = Type.GetType("System.Data.SqlClient.SqlConnection, System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
//                    }
//                    break;

//                case "System.Data.SqlServerCe": type = Type.GetType("System.Data.SqlServerCe.SqlCeConnection, System.Data.SqlServerCe"); break;
//                case "System.Data.EntityClient": type = Type.GetType("System.Data.EntityClient.EntityConnection, System.Data.EntityClient"); break;
//                case "System.Data.OracleClient": type = Type.GetType("System.Data.OracleClient.OracleConnection, System.Data.OracleClient"); break;
//                case "System.Data.SQLite": type = Type.GetType("System.Data.SQLite.SQLiteConnection, System.Data.SQLite"); break;
//                case "Microsoft.Data.SqlClient": type = Type.GetType("Microsoft.Data.SqlClient.SqlConnection, Microsoft.Data.SqlClient"); break;
//                case "Microsoft.SqlServerCe.Client": type = Type.GetType("Microsoft.SqlServerCe.Client.SqlCeConnection, Microsoft.SqlServerCe.Client"); break;
//                case "Microsoft.Data.Sqlite": type = Type.GetType("Microsoft.Data.Sqlite.SqliteConnection, Microsoft.Data.Sqlite"); break;
//                case "Oracle.DataAccess.Client": type = Type.GetType("Oracle.DataAccess.Client.OracleConnection, Oracle.DataAccess.Client"); break;
//                case "MySql.Data.MySqlClient": type = Type.GetType("MySql.Data.MySqlClient.MySqlConnection, MySql.Data"); break;
//                case "MySqlConnector": type = Type.GetType("MySqlConnector.MySqlConnection, MySqlConnector"); break;
//                default: type = Type.GetType(providerName); break;
//            }
//            return (DbConnection)Activator.CreateInstance(type, connectionString);
//        }
//    }
//}