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
        private readonly string _connectionString;
        private readonly XElement _content;
        private readonly string _name;
        private readonly string _providerName;

        /// <summary>
        /// 连接属性的注释。
        /// </summary>
        public ConfigComment Comment => _comment;

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

        internal XElement Content => _content;

        #region Construction

        /// <summary>
        /// 创建 ConnectionStringProperty 的新实例。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <param name="connection">数据库连接实例。</param>
        public ConnectionStringProperty(string name, DbConnection connection)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }
            _connectionString = connection.ConnectionString;
            _providerName = connection.GetType().Namespace;
            _content = GetElement(name, connection.ConnectionString, connection.GetType().Namespace);
            _comment = new ConfigComment(null, _content);
        }

        /// <summary>
        /// 创建 ConnectionStringProperty 的新实例。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <param name="connectionString">连接字符串。</param>
        /// <param name="providerName">数据库引擎的文本名称。</param>
        public ConnectionStringProperty(string name, string connectionString, string providerName)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _providerName = providerName;
            _content = GetElement(name, connectionString, providerName);
            _comment = new ConfigComment(null, _content);
        }

        internal ConnectionStringProperty(XElement content, XComment comment)
        {
            _name = content.Attribute("name").Value;
            _connectionString = content.Attribute("connectionString").Value;
            _providerName = content.Attribute("providerName")?.Value;
            _content = content;
            _comment = new ConfigComment(comment, content);
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