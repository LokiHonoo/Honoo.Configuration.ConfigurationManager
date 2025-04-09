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
        private readonly XConfigComment _comment;
        private readonly string _connectionString;
        private readonly XElement _content;
        private readonly string _providerName;

        /// <summary>
        /// 连接属性的注释。
        /// </summary>
        public XConfigComment Comment => _comment;

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
            _content = GetElement(connection.ConnectionString, connection.GetType().Namespace);
            _comment = new XConfigComment(null, _content);
            _connectionString = _content.Attribute("connectionString").Value;
            _providerName = _content.Attribute("providerName")?.Value;
        }

        /// <summary>
        /// 创建 ConnectionStringProperty 的新实例。
        /// </summary>
        /// <param name="connectionString">连接字符串。</param>
        /// <param name="providerName">数据库引擎的文本名称。</param>
        public ConnectionStringProperty(string connectionString, string providerName)
        {
            _content = GetElement(connectionString, providerName);
            _comment = new XConfigComment(null, _content);
            _connectionString = _content.Attribute("connectionString").Value;
            _providerName = _content.Attribute("providerName")?.Value;
        }

        internal ConnectionStringProperty(XElement content, XComment comment)
        {
            _content = content;
            _comment = new XConfigComment(comment, content);
            _connectionString = content.Attribute("connectionString").Value;
            _providerName = content.Attribute("providerName")?.Value;
        }

        #endregion Construction

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
            if (connectionString is null)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }
            XElement element = new XElement("add");
            element.SetAttributeValue("name", "connection_string_property");
            element.SetAttributeValue("connectionString", connectionString.Trim());
            element.SetAttributeValue("providerName", providerName);
            return element;
        }
    }
}