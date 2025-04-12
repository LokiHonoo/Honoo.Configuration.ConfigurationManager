using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Xml;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 连接属性集合。
    /// </summary>
    public sealed class ConnectionStringsPropertySet : IEnumerable<KeyValuePair<string, ConnectionStringProperty>>
    {
        #region Members

        private readonly XElement _container;
        private readonly Dictionary<string, ConnectionStringProperty> _properties = new Dictionary<string, ConnectionStringProperty>();

        /// <summary>
        /// 获取连接属性集合中包含的元素数。
        /// </summary>
        public int Count => _properties.Count;

        /// <summary>
        /// 获取连接属性集合的键的集合。
        /// </summary>
        public Dictionary<string, ConnectionStringProperty>.KeyCollection Keys => _properties.Keys;

        /// <summary>
        /// 获取连接属性集合的值的集合。
        /// </summary>
        public Dictionary<string, ConnectionStringProperty>.ValueCollection Values => _properties.Values;

        /// <summary>
        /// 获取或设置具有指定名称的连接属性的值。直接赋值等同于 AddOrUpdate 方法。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ConnectionStringProperty this[string name]
        {
            get { return GetValue(name); }
            set { AddOrUpdate(name, value); }
        }

        #endregion Members

        #region Construction

        internal ConnectionStringsPropertySet(XElement container)
        {
            _container = container;
            if (_container.HasElements)
            {
                IEnumerator<XNode> enumerator = _container.Nodes().GetEnumerator();
                XComment comment = null;
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.NodeType == XmlNodeType.Comment)
                    {
                        comment = (XComment)enumerator.Current;
                    }
                    else
                    {
                        if (enumerator.Current.NodeType == XmlNodeType.Element)
                        {
                            XElement content = (XElement)enumerator.Current;
                            if (content.Name == "add")
                            {
                                string name = content.Attribute("name").Value;
                                ConnectionStringProperty value = new ConnectionStringProperty(content, comment);
                                _properties.Remove(name);
                                _properties.Add(name, value);
                            }
                        }
                        comment = null;
                    }
                }
            }
        }

        #endregion Construction

        #region Add

        /// <summary>
        /// 添加一个连接属性。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <param name="value">连接属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ConnectionStringProperty Add(string name, ConnectionStringProperty value)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            _properties.Add(name, value);
            if (value.Comment.HasValue)
            {
                _container.Add(value.Comment.Comment);
            }
            value.Content.SetAttributeValue("name", name);
            _container.Add(value.Content);
            return value;
        }

        /// <summary>
        /// 添加一个连接属性。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <param name="connection">数据库连接实例。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ConnectionStringProperty Add(string name, DbConnection connection)
        {
            return Add(name, new ConnectionStringProperty(connection));
        }

        /// <summary>
        /// 添加一个连接属性。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <param name="connectionString">连接字符串。</param>
        /// <param name="providerName">数据库引擎的文本名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ConnectionStringProperty Add(string name, string connectionString, string providerName)
        {
            return Add(name, new ConnectionStringProperty(connectionString, providerName));
        }

        #endregion Add

        #region AddOrUpdate

        /// <summary>
        /// 添加或更新一个连接属性。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <param name="value">连接属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ConnectionStringProperty AddOrUpdate(string name, ConnectionStringProperty value)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (_properties.TryGetValue(name, out ConnectionStringProperty val))
            {
                value.Content.SetAttributeValue("name", name);
                if (value.Comment.HasValue)
                {
                    val.Content.AddBeforeSelf(value.Comment.Comment);
                }
                val.Content.AddBeforeSelf(value.Content);
                val.Comment.Remove();
                val.Content.Remove();
                _properties[name] = value;
                return value;
            }
            else
            {
                return Add(name, value);
            }
        }

        /// <summary>
        /// 添加或更新一个连接属性。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <param name="connection">数据库连接实例。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ConnectionStringProperty AddOrUpdate(string name, DbConnection connection)
        {
            return AddOrUpdate(name, new ConnectionStringProperty(connection));
        }

        /// <summary>
        /// 添加或更新一个连接属性。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <param name="connectionString">连接字符串。</param>
        /// <param name="providerName">数据库引擎的文本名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ConnectionStringProperty AddOrUpdate(string name, string connectionString, string providerName)
        {
            return AddOrUpdate(name, new ConnectionStringProperty(connectionString, providerName));
        }

        #endregion AddOrUpdate

        #region GetOrAdd

        /// <summary>
        /// 获取与指定键关联的连接属性。如果不存在，添加一个连接属性并返回值。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <param name="valueIfNotExists">指定名称关联的连接属性不存在时添加此连接属性。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ConnectionStringProperty GetOrAdd(string name, ConnectionStringProperty valueIfNotExists)
        {
            return TryGetValue(name, out ConnectionStringProperty value) ? value : Add(name, valueIfNotExists);
        }

        /// <summary>
        /// 获取与指定键关联的连接属性。如果不存在，添加一个连接属性并返回值。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <param name="connection">指定名称关联的连接属性不存在时添加此数据库连接实例。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ConnectionStringProperty GetOrAdd(string name, DbConnection connection)
        {
            return TryGetValue(name, out ConnectionStringProperty value) ? value : Add(name, connection);
        }

        /// <summary>
        /// 获取与指定键关联的连接属性。如果不存在，添加一个连接属性并返回值。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <param name="connectionString">指定名称关联的连接属性不存在时添加此连接字符串。</param>
        /// <param name="providerName">指定名称关联的连接属性不存在时添加此数据库引擎的文本名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ConnectionStringProperty GetOrAdd(string name, string connectionString, string providerName)
        {
            return TryGetValue(name, out ConnectionStringProperty value) ? value : Add(name, connectionString, providerName);
        }

        #endregion GetOrAdd

        #region TryGetValue

        /// <summary>
        /// 获取与指定名称关联的连接属性的值。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <param name="value">连接属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string name, out ConnectionStringProperty value)
        {
            return _properties.TryGetValue(name, out value);
        }

        /// <summary>
        /// 获取与指定名称关联的连接属性的值。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <param name="connectionString">连接字符串。</param>
        /// <param name="providerName">数据库引擎的文本名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string name, out string connectionString, out string providerName)
        {
            if (_properties.TryGetValue(name, out ConnectionStringProperty value))
            {
                connectionString = value.ConnectionString;
                providerName = value.ProviderName;
                return true;
            }
            connectionString = null;
            providerName = null;
            return false;
        }

        #endregion TryGetValue

        #region GetValue

        /// <summary>
        /// 获取与指定名称关联的连接属性的值。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ConnectionStringProperty GetValue(string name)
        {
            return TryGetValue(name, out ConnectionStringProperty value) ? value : null;
        }

        #endregion GetValue

        #region GetValueOrDefault

        /// <summary>
        /// 获取与指定名称关联的连接属性的值。如果没有找到指定名称，返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <param name="defaultValue">没有找到指定名称时的连接属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ConnectionStringProperty GetValue(string name, ConnectionStringProperty defaultValue)
        {
            return TryGetValue(name, out ConnectionStringProperty value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取与指定名称关联的连接属性的值。如果没有找到指定名称，返回新建的连接属性的值。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <param name="connection">指定名称关联的连接属性不存在时添加此数据库连接实例。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ConnectionStringProperty GetValue(string name, DbConnection connection)
        {
            return TryGetValue(name, out ConnectionStringProperty value) ? value : new ConnectionStringProperty(connection);
        }

        /// <summary>
        /// 获取与指定名称关联的连接属性的值。如果没有找到指定名称，返回新建的连接属性的值。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <param name="connectionString">指定名称关联的连接属性不存在时添加此连接字符串。</param>
        /// <param name="providerName">指定名称关联的连接属性不存在时添加此数据库引擎的文本名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ConnectionStringProperty GetValue(string name, string connectionString, string providerName)
        {
            return TryGetValue(name, out ConnectionStringProperty value) ? value : new ConnectionStringProperty(connectionString, providerName);
        }

        #endregion GetValueOrDefault

        /// <summary>
        /// 从连接属性集合中移除所有连接属性。
        /// </summary>
        public void Clear()
        {
            _container.RemoveNodes();
            _properties.Clear();
        }

        /// <summary>
        /// 确定连接属性集合是否包含带有指定名称的连接属性。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool ContainsName(string name)
        {
            return _properties.ContainsKey(name);
        }

        /// <summary>
        /// 返回循环访问集合的枚举数。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, ConnectionStringProperty>> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        /// <summary>
        /// 从连接属性集合中移除带有指定名称的连接属性。和指定名称关联的连接属性的注释一并移除。
        /// <br/>如果该元素成功移除，返回 <see langword="true"/>。如果没有找到指定元素，则返回 <see langword="false"/>。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool Remove(string name)
        {
            if (_properties.TryGetValue(name, out ConnectionStringProperty value))
            {
                value.Comment.Remove();
                value.Content.Remove();
                _properties.Remove(name);
                return true;
            }
            return false;
        }
    }
}