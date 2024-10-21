using System;
using System.Data.Common;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 连接属性。
    /// </summary>
    public sealed class ConnectionStringProperty
    {
        private readonly ConfigComment _comment;
        private readonly XElement _content;
        private string _connectionString;
        private string _providerName;

        /// <summary>
        /// 连接属性的注释。
        /// </summary>
        public ConfigComment Comment => _comment;

        /// <summary>
        /// 获取连接字符串。
        /// </summary>
        public string ConnectionString => _connectionString;

        /// <summary>
        /// 获取数据库引擎的文本名称。
        /// </summary>
        public string ProviderName => _providerName;

        internal XElement Content => _content;

        #region Construction

        /// <summary>
        /// 创建 ConnectionStringProperty 的新实例。
        /// </summary>
        /// <param name="connection">数据库连接实例。</param>
        public ConnectionStringProperty(DbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }
            _connectionString = connection.ConnectionString;
            _providerName = connection.GetType().Namespace;
            _content = GetElement(connection.ConnectionString, connection.GetType().Namespace);
            _comment = new ConfigComment(null, _content);
        }

        /// <summary>
        /// 创建 ConnectionStringProperty 的新实例。
        /// </summary>
        /// <param name="connectionString">连接字符串。</param>
        /// <param name="providerName">数据库引擎的文本名称。</param>
        public ConnectionStringProperty(string connectionString, string providerName)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _providerName = providerName;
            _content = GetElement(connectionString, providerName);
            _comment = new ConfigComment(null, _content);
        }

        internal ConnectionStringProperty(XElement content, XComment comment)
        {
            _connectionString = content.Attribute("connectionString").Value;
            _providerName = content.Attribute("providerName")?.Value;
            _content = content;
            _comment = new ConfigComment(comment, content);
        }

        #endregion Construction

        #region SetValue

        /// <summary>
        /// 设置值。
        /// </summary>
        /// <param name="connection">数据库连接实例。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ConnectionStringProperty SetValue(DbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }
            return SetValue(connection.ConnectionString, connection.GetType().Namespace);
        }

        /// <summary>
        /// 设置值。
        /// </summary>
        /// <param name="connectionString">连接字符串。</param>
        /// <param name="providerName">数据库引擎的文本名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ConnectionStringProperty SetValue(string connectionString, string providerName)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }
            connectionString = connectionString.Trim();
            providerName = providerName?.Trim();
            _content.SetAttributeValue("connectionString", connectionString);
            _content.SetAttributeValue("providerName", providerName);
            _connectionString = connectionString;
            _providerName = providerName;
            return this;
        }

        #endregion SetValue

        /// <summary>
        /// 指定具体的数据库连接类型，创建连接实例。此方法忽略数据库引擎参数 ProviderName。
        /// </summary>
        /// <typeparam name="T">从 DbConnection 继承的具体的数据库连接类型，如 System.Data.SqlClient.SqlConnection 等。</typeparam>
        /// <returns></returns>
        public T CreateInstance<T>() where T : DbConnection
        {
            return (T)Activator.CreateInstance(typeof(T), _connectionString);
        }

        /// <summary>
        /// 方法已重写。返回节点的缩进 XML 文本。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _content.ToString();
        }

        private static XElement GetElement(string connectionString, string providerName)
        {
            XElement element = new XElement("add");
            element.SetAttributeValue("name", "connection_string_property");
            element.SetAttributeValue("connectionString", connectionString);
            element.SetAttributeValue("providerName", providerName);
            return element;
        }
    }
}