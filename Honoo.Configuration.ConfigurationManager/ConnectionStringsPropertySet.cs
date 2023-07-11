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
    public sealed class ConnectionStringsPropertySet : IEnumerable<KeyValuePair<string, ConnectionStringsValue>>
    {
        #region Class

        /// <summary>
        /// 代表此连接属性集合的名称的集合。
        /// </summary>
        public sealed class NameCollection : IEnumerable<string>
        {
            #region Properties

            private readonly Dictionary<string, ConnectionStringsValue> _properties;

            /// <summary>
            /// 获取连接属性集合的名称的元素数。
            /// </summary>
            public int Count => _properties.Count;

            #endregion Properties

            internal NameCollection(Dictionary<string, ConnectionStringsValue> groups)
            {
                _properties = groups;
            }

            /// <summary>
            /// 从指定数组索引开始将名称元素复制到到指定数组。
            /// </summary>
            /// <param name="array">要复制到的目标数组。</param>
            /// <param name="arrayIndex">目标数组中从零开始的索引，从此处开始复制。</param>
            public void CopyTo(string[] array, int arrayIndex)
            {
                _properties.Keys.CopyTo(array, arrayIndex);
            }

            /// <summary>
            /// 返回循环访问集合的枚举数。
            /// </summary>
            /// <returns></returns>
            public IEnumerator<string> GetEnumerator()
            {
                return _properties.Keys.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _properties.Keys.GetEnumerator();
            }
        }

        /// <summary>
        /// 代表此连接属性集合的值的集合。
        /// </summary>
        public sealed class ValueCollection : IEnumerable<ConnectionStringsValue>
        {
            #region Properties

            private readonly Dictionary<string, ConnectionStringsValue> _properties;

            /// <summary>
            /// 获取连接属性集合的值的元素数。
            /// </summary>
            public int Count => _properties.Count;

            #endregion Properties

            internal ValueCollection(Dictionary<string, ConnectionStringsValue> groups)
            {
                _properties = groups;
            }

            /// <summary>
            /// 从指定数组索引开始将值元素复制到到指定数组。
            /// </summary>
            /// <param name="array">要复制到的目标数组。</param>
            /// <param name="arrayIndex">目标数组中从零开始的索引，从此处开始复制。</param>
            public void CopyTo(ConnectionStringsValue[] array, int arrayIndex)
            {
                _properties.Values.CopyTo(array, arrayIndex);
            }

            /// <summary>
            /// 返回循环访问集合的枚举数。
            /// </summary>
            /// <returns></returns>
            public IEnumerator<ConnectionStringsValue> GetEnumerator()
            {
                return _properties.Values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _properties.Values.GetEnumerator();
            }
        }

        #endregion Class

        #region Properties

        private readonly Dictionary<string, XComment> _comments = new Dictionary<string, XComment>();
        private readonly Dictionary<string, XElement> _contents = new Dictionary<string, XElement>();
        private readonly NameCollection _nameExhibits;
        private readonly Dictionary<string, ConnectionStringsValue> _properties = new Dictionary<string, ConnectionStringsValue>();
        private readonly XElement _superior;
        private readonly ValueCollection _valueExhibits;

        /// <summary>
        /// 获取连接属性集合中包含的元素数。
        /// </summary>
        public int Count => _properties.Count;

        /// <summary>
        /// 获取连接属性集合的名称的集合。
        /// </summary>
        public NameCollection Names => _nameExhibits;

        /// <summary>
        /// 获取连接属性集合的值的集合。
        /// </summary>
        public ValueCollection Values => _valueExhibits;

        /// <summary>
        /// 获取或设置具有指定名称的连接属性的值。直接赋值等同于 AddOrUpdate 方法。
        /// <br/>取值时如果没有找到指定名称，返回 <see langword="null"/>。使用 <see cref="GetValue(string , ConnectionStringsValue)"/> 设置没有找到指定名称时的默认值。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ConnectionStringsValue this[string name]
        {
            get => _properties.TryGetValue(name, out ConnectionStringsValue value) ? value : null;
            set { AddOrUpdate(name, value); }
        }

        #endregion Properties

        #region Construction

        internal ConnectionStringsPropertySet(XElement superior)
        {
            _superior = superior;
            if (superior.HasElements)
            {
                IEnumerator<XNode> enumerator = superior.Nodes().GetEnumerator();
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
                                ConnectionStringsValue value = new ConnectionStringsValue(content);
                                _properties.Add(name, value);
                                _contents.Add(name, content);
                                _comments.Add(name, comment);
                            }
                        }
                        comment = null;
                    }
                }
            }
            _nameExhibits = new NameCollection(_properties);
            _valueExhibits = new ValueCollection(_properties);
        }

        #endregion Construction

        /// <summary>
        /// 添加或更新一个连接属性。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <param name="value">连接属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string name, ConnectionStringsValue value)
        {
            if (value == null)
            {
                AddOrUpdate(name, null, null);
            }
            else
            {
                AddOrUpdate(name, value.ConnectionString, value.ProviderName);
            }
        }

        /// <summary>
        /// 添加或更新一个连接属性。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <param name="connection">数据库连接实例。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string name, DbConnection connection)
        {
            if (connection == null)
            {
                AddOrUpdate(name, null, null);
            }
            else
            {
                AddOrUpdate(name, connection.ConnectionString, connection.GetType().Namespace);
            }
        }

        /// <summary>
        /// 添加或更新一个连接属性。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <param name="connectionString">连接字符串。</param>
        /// <param name="providerName">数据库引擎的文本名称。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string name, string connectionString, string providerName)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"The invalid argument - {nameof(name)}.");
            }
            if (connectionString == null && providerName == null)
            {
                if (_properties.Remove(name))
                {
                    _contents[name].Remove();
                    _contents.Remove(name);
                    _comments[name]?.Remove();
                    _comments.Remove(name);
                }
            }
            else if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }
            else
            {
                if (_properties.TryGetValue(name, out _))
                {
                    XElement content = _contents[name];
                    content.SetAttributeValue("connectionString", connectionString);
                    if (providerName != null)
                    {
                        content.SetAttributeValue("providerName", providerName);
                    }
                    _properties[name] = new ConnectionStringsValue(content);
                }
                else
                {
                    XElement content = new XElement("add");
                    content.SetAttributeValue("name", name);
                    content.SetAttributeValue("connectionString", connectionString);
                    if (providerName != null)
                    {
                        content.SetAttributeValue("providerName", providerName);
                    }
                    ConnectionStringsValue value = new ConnectionStringsValue(content);
                    _properties.Add(name, value);
                    _contents.Add(name, content);
                    _comments.Add(name, null);
                    _superior.Add(content);
                }
            }
        }

        /// <summary>
        /// 从连接属性集合中移除所有连接属性。
        /// </summary>
        public void Clear()
        {
            _properties.Clear();
            _contents.Clear();
            _comments.Clear();
            _superior.RemoveNodes();
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
        public IEnumerator<KeyValuePair<string, ConnectionStringsValue>> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        /// <summary>
        /// 获取与指定名称关联的连接属性的值。
        ///  <br/>如果没有找到指定名称，返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <param name="defaultValue">没有找到指定名称时的连接属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ConnectionStringsValue GetValue(string name, ConnectionStringsValue defaultValue)
        {
            return _properties.TryGetValue(name, out ConnectionStringsValue value) ? value : defaultValue;
        }

        /// <summary>
        /// 从连接属性集合中移除带有指定名称的连接属性。和指定名称关联的连接属性的注释一并移除。
        /// <br/>如果该元素成功移除，返回 <see langword="true"/>。如果没有找到指定名称，则返回 <see langword="false"/>。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool Remove(string name)
        {
            if (_properties.Remove(name))
            {
                _contents[name].Remove();
                _contents.Remove(name);
                _comments[name]?.Remove();
                _comments.Remove(name);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取与指定名称关联的连接属性的注释。
        /// <br/>如果没有找到指定名称，返回 <see langword="false"/>。如果找到了指定名称但没有注释节点，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <param name="comment">连接属性的注释。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetComment(string name, out string comment)
        {
            _comments.TryGetValue(name, out XComment comment_);
            if (comment_ == null)
            {
                comment = null;
                return false;
            }
            else
            {
                comment = comment_.Value;
                return true;
            }
        }

        /// <summary>
        /// 获取与指定名称关联的连接属性的值。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <param name="value">连接属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string name, out ConnectionStringsValue value)
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
            if (_properties.TryGetValue(name, out ConnectionStringsValue value))
            {
                connectionString = value.ConnectionString;
                providerName = value.ProviderName;
                return true;
            }
            else
            {
                connectionString = null;
                providerName = null;
                return false;
            }
        }

        /// <summary>
        /// 添加或更新或删除一个与指定名称关联的连接属性的注释。
        /// </summary>
        /// <param name="name">连接属性的名称。</param>
        /// <param name="comment">连接属性的注释。</param>
        /// <exception cref="Exception"/>
        public bool TrySetComment(string name, string comment)
        {
            if (comment == null)
            {
                if (_comments.TryGetValue(name, out XComment comment_))
                {
                    if (comment_ != null)
                    {
                        comment_.Remove();
                        _comments[name] = null;
                    }
                    return true;
                }
            }
            else
            {
                if (_comments.TryGetValue(name, out XComment comment_))
                {
                    if (comment_ == null)
                    {
                        comment_ = new XComment(comment);
                        _contents[name].AddBeforeSelf(comment_);
                        _comments[name] = comment_;
                    }
                    else
                    {
                        comment_.Value = comment;
                    }
                    return true;
                }
            }
            return false;
        }
    }
}