﻿using System;
using System.Data.Common;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 连接属性。
    /// </summary>
    public sealed class ConnectionStringsProperty
    {
        private readonly string _connectionString;
        private readonly XElement _content;
        private readonly string _providerName;
        private DbConnection _connection;
        private bool _generated;

        /// <summary>
        /// 获取连接实例。如果连接属性中没有数据库引擎参数，或者工程没有引用相关类库，将引发异常。
        /// </summary>
        public DbConnection Connection
        {
            get
            {
                if (!_generated)
                {
                    _connection = CreateInstance(_providerName, _connectionString);
                    _generated = true;
                }
                return _connection;
            }
        }

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
        /// 创建 ConnectionStringsProperty 的新实例。
        /// </summary>
        /// <param name="connection">数据库连接实例。</param>
        public ConnectionStringsProperty(DbConnection connection)
        {
            if (connection is null)
            {
                throw new ArgumentNullException(nameof(connection));
            }
            _connectionString = connection.ConnectionString;
            _providerName = connection.GetType().Namespace;
            _content = new XElement("add");
            _content.SetAttributeValue("name", "newProperty");
            _content.SetAttributeValue("connectionString", _connectionString);
            _content.SetAttributeValue("providerName", _providerName);
        }

        /// <summary>
        /// 创建 ConnectionStringsProperty 的新实例。
        /// </summary>
        /// <param name="connectionString">连接字符串。</param>
        /// <param name="providerName">数据库引擎的文本名称。</param>
        public ConnectionStringsProperty(string connectionString, string providerName)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException($"The invalid connection string - {nameof(connectionString)}.");
            }
            _connectionString = connectionString;
            _providerName = providerName;
            _content = new XElement("add");
            _content.SetAttributeValue("name", "newProperty");
            _content.SetAttributeValue("connectionString", connectionString);
            _content.SetAttributeValue("providerName", providerName);
        }

        internal ConnectionStringsProperty(XElement content)
        {
            _content = content;
            _connectionString = content.Attribute("connectionString").Value;
            _providerName = content.Attribute("providerName")?.Value;
        }

        #endregion Construction

        /// <summary>
        /// 确定指定的对象是否等于当前对象。
        /// </summary>
        /// <param name="obj">要与当前对象进行比较的对象。</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is ConnectionStringsProperty other && _content.Equals(other._content);
        }

        /// <summary>
        /// 作为默认哈希函数。
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _content.GetHashCode();
        }

        /// <summary>
        /// 方法已重写。返回节点的缩进 XML 文本。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _content.ToString();
        }

        internal void Remove()
        {
            _content.Remove();
        }

        private static DbConnection CreateInstance(string providerName, string connectionString)
        {
            Type type;
            switch (providerName)
            {
                case "System.Data.Odbc":
                case "System.Data.Odbc.OdbcConnection":
                case "System.Data.Odbc.OdbcConnection, System.Data.Odbc":
                    type = Type.GetType("System.Data.Odbc.OdbcConnection, System.Data.Odbc");
                    if (type is null)
                    {
                        type = Type.GetType("System.Data.Odbc.OdbcConnection, System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
                    }
                    break;

                case "System.Data.OleDb":
                case "System.Data.OleDb.OleDbConnection":
                case "System.Data.OleDb.OleDbConnection, System.Data.OleDb":
                    type = Type.GetType("System.Data.OleDb.OleDbConnection, System.Data.OleDb");
                    if (type is null)
                    {
                        type = Type.GetType("System.Data.OleDb.OleDbConnection, System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
                    }
                    break;

                case "System.Data.SqlClient":
                case "System.Data.SqlClient.SqlConnection":
                case "System.Data.SqlClient.SqlConnection, System.Data.SqlClient":
                    type = Type.GetType("System.Data.SqlClient.SqlConnection, System.Data.SqlClient");
                    if (type is null)
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

                default: type = Type.GetType(providerName); break;
            }
            return (DbConnection)Activator.CreateInstance(type, connectionString);
        }
    }
}