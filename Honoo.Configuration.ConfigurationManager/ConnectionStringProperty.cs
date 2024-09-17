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
            _name = name ?? throw new ArgumentNullException(nameof(name));
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }
            _connectionString = connection.ConnectionString;
            _providerName = connection.GetType().Namespace;
            _content = GetElement(name, connection.ConnectionString, connection.GetType().Namespace);
            _comment = null;
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
        /// 指定具体的数据库连接类型，创建连接实例。此方法忽略数据库引擎参数 ProviderName。
        /// </summary>
        /// <typeparam name="T">从 DbConnection 继承的具体的数据库连接类型，如 System.Data.SqlClient.SqlConnection 等。</typeparam>
        /// <returns></returns>
        public T CreateInstance<T>() where T : DbConnection
        {
            return (T)Activator.CreateInstance(typeof(T), _connectionString);
        }

        #region Comment

        /// <summary>
        /// 获取注释。如果没有找到注释，返回 <see langword="null"/>。
        /// </summary>
        /// <returns></returns>
        public string GetComment()
        {
            return TryGetComment(out string comment) ? comment : null;
        }

        /// <summary>
        /// 删除注释。如果注释成功删除，返回 <see langword="true"/>。如果没有找到注释节点，则返回 <see langword="false"/>。
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
            if (comment == null)
            {
                if (_comment != null)
                {
                    _comment.Remove();
                    _comment = null;
                }
            }
            else if (_comment == null)
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
        /// 获取注释。如果没有找到注释，返回 <see langword="false"/>。
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