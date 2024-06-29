using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 连接属性。
    /// </summary>
    public sealed class ConnectionStringProperty
    {
        private readonly string _connectionString;
        private readonly XElement _content;
        private readonly string _name;
        private readonly string _providerName;
        private XComment _comment;

        /// <summary>
        /// 获取连接字符串。
        /// </summary>
        public string ConnectionString => _connectionString;

        /// <summary>
        /// 获取此连接属性的名称。
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// 获取数据库引擎的文本名称。
        /// </summary>
        public string ProviderName => _providerName;

        internal XComment Comment => _comment;
        internal XElement Content => _content;

        #region Construction

        /// <summary>
        /// 创建 ConnectionStringProperty 的新实例。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <param name="connection">数据库连接实例。</param>
        public ConnectionStringProperty(string name, DbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _connectionString = connection.ConnectionString;
            _providerName = connection.GetType().Namespace;
            _content = GetElement(name, connection.ConnectionString, connection.GetType().Namespace);
            _comment = null;
        }

        /// <summary>
        /// 创建 ConnectionStringProperty 的新实例。
        /// </summary>
        /// <param name="connectionString">连接字符串。</param>
        /// <param name="providerName">数据库引擎的文本名称。</param>
        public ConnectionStringProperty(string name, string connectionString, string providerName)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _providerName = providerName;
            _content = GetElement(name, connectionString, providerName);
            _comment = null;
        }

        internal ConnectionStringProperty(XElement content, XComment comment)
        {
            _name = content.Attribute("name").Value;
            _connectionString = content.Attribute("connectionString").Value;
            _providerName = content.Attribute("providerName")?.Value;
            _content = content;
            _comment = comment;
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

        #region Comment

        /// <summary>
        /// 获取注释。
        /// </summary>
        /// <returns></returns>
        public string GetComment()
        {
            return TryGetComment(out string comment) ? comment : null;
        }

        /// <summary>
        /// 删除注释。
        /// <br/>如果注释成功删除，返回 <see langword="true"/>。如果没有找到注释节点，则返回 <see langword="false"/>。
        /// </summary>
        /// <returns></returns>
        public bool RemoveComment()
        {
            if (_comment != null)
            {
                _comment.Remove();
                _comment = null;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 添加或更新注释。
        /// </summary>
        /// <param name="comment">注释文本。</param>
        /// <exception cref="Exception"/>
        public void SetComment(string comment)
        {
            if (string.IsNullOrEmpty(comment))
            {
                throw new ArgumentException($"The invalid argument - {nameof(comment)}.");
            }
            if (_comment == null)
            {
                _comment = new XComment(comment);
                _content.AddBeforeSelf(_comment);
            }
            else
            {
                _comment.Value = comment;
            }
        }

        /// <summary>
        /// 获取注释。
        /// <br/>如果没有找到注释，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="comment">注释文本。</param>
        /// <returns></returns>
        public bool TryGetComment(out string comment)
        {
            if (_comment != null)
            {
                comment = _comment.Value;
                return true;
            }
            comment = null;
            return false;
        }

        #endregion Comment

        /// <summary>
        /// 方法已重写。返回节点的缩进 XML 文本。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _content.ToString();
        }

        private static XElement GetElement(string name, string connectionString, string providerName)
        {
            XElement content = new XElement("add");
            content.SetAttributeValue("name", name);
            content.SetAttributeValue("connectionString", connectionString);
            content.SetAttributeValue("providerName", providerName);
            return content;
        }
    }
}