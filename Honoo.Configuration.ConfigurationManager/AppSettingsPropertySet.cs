using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置属性集合。
    /// </summary>
    public sealed class AppSettingsPropertySet : IEnumerable<KeyValuePair<string, string>>
    {
        #region Class

        /// <summary>
        /// 代表此配置属性集合的键的集合。
        /// </summary>
        public sealed class KeyCollection : IEnumerable<string>
        {
            #region Properties

            private readonly Dictionary<string, string> _properties;

            /// <summary>
            /// 获取配置属性集合的键的元素数。
            /// </summary>
            public int Count => _properties.Count;

            #endregion Properties

            internal KeyCollection(Dictionary<string, string> properties)
            {
                _properties = properties;
            }

            /// <summary>
            /// 从指定数组索引开始将键元素复制到到指定数组。
            /// </summary>
            /// <param name="array">要复制到的目标数组。</param>
            /// <param name="arrayIndex">目标数组中从零开始的索引，从此处开始复制。</param>
            public void CopyTo(string[] array, int arrayIndex)
            {
                _properties.Keys.CopyTo(array, arrayIndex);
            }

            /// <summary>
            /// 支持在泛型集合上进行简单迭代。
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
        /// 代表此配置属性集合的值的集合。
        /// </summary>
        public sealed class ValueCollection : IEnumerable<string>
        {
            #region Properties

            private readonly Dictionary<string, string> _properties;

            /// <summary>
            /// 获取配置属性集合的值的元素数。
            /// </summary>
            public int Count => _properties.Count;

            #endregion Properties

            internal ValueCollection(Dictionary<string, string> properties)
            {
                _properties = properties;
            }

            /// <summary>
            /// 从指定数组索引开始将值元素复制到到指定数组。
            /// </summary>
            /// <param name="array">要复制到的目标数组。</param>
            /// <param name="arrayIndex">目标数组中从零开始的索引，从此处开始复制。</param>
            public void CopyTo(string[] array, int arrayIndex)
            {
                _properties.Values.CopyTo(array, arrayIndex);
            }

            /// <summary>
            /// 支持在泛型集合上进行简单迭代。
            /// </summary>
            /// <returns></returns>
            public IEnumerator<string> GetEnumerator()
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
        private readonly KeyCollection _keyExhibits;
        private readonly Dictionary<string, string> _properties = new Dictionary<string, string>();
        private readonly XElement _superior;
        private readonly ValueCollection _valueExhibits;

        /// <summary>
        /// 获取配置属性集合中包含的元素数。
        /// </summary>
        public int Count => _properties.Count;

        /// <summary>
        /// 获取配置属性集合的键的集合。
        /// </summary>
        public KeyCollection Keys => _keyExhibits;

        /// <summary>
        /// 获取配置属性集合的值的集合。
        /// </summary>
        public ValueCollection Values => _valueExhibits;

        /// <summary>
        /// 获取或设置具有指定键的配置属性的值。直接赋值等同于 AddOrUpdate 方法。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public string this[string key]
        {
            get => _properties.TryGetValue(key, out string value) ? value : null;
            set { AddOrUpdate(key, value); }
        }

        #endregion Properties

        #region Construction

        internal AppSettingsPropertySet(XElement superior)
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
                                string key = content.Attribute("key").Value;
                                string value = content.Attribute("value").Value;
                                _properties.Add(key, value);
                                _contents.Add(key, content);
                                _comments.Add(key, comment);
                            }
                        }
                        comment = null;
                    }
                }
            }
            _keyExhibits = new KeyCollection(_properties);
            _valueExhibits = new ValueCollection(_properties);
        }

        #endregion Construction

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public void AddOrUpdate(string key, string value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                if (_properties.Remove(key))
                {
                    _contents[key].Remove();
                    _contents.Remove(key);
                    _comments[key]?.Remove();
                    _comments.Remove(key);
                }
            }
            else
            {
                if (_properties.TryGetValue(key, out _))
                {
                    _contents[key].SetAttributeValue("value", value);
                    _properties[key] = value;
                }
                else
                {
                    _properties.Add(key, value);
                    XElement content = new XElement("add");
                    content.SetAttributeValue("key", key);
                    content.SetAttributeValue("value", value);
                    _contents.Add(key, content);
                    _comments.Add(key, null);
                    _superior.Add(content);
                }
            }
        }

        /// <summary>
        /// 从配置属性集合中移除所有配置属性。
        /// </summary>
        public void Clear()
        {
            _properties.Clear();
            _contents.Clear();
            _comments.Clear();
            _superior.RemoveNodes();
        }

        /// <summary>
        /// 确定配置属性集合是否包含带有指定键的配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool ContainsKey(string key)
        {
            return _properties.ContainsKey(key);
        }

        /// <summary>
        /// 支持在泛型集合上进行简单迭代。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        /// <summary>
        /// 从配置属性集合中移除带有指定键的配置属性。和指定键关联的配置属性的注释一并移除。
        /// <para/>如果该元素成功移除，返回 true。如果没有找到指定键，则仍返回 false。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool Remove(string key)
        {
            if (_properties.Remove(key))
            {
                _contents[key].Remove();
                _contents.Remove(key);
                _comments[key]?.Remove();
                _comments.Remove(key);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的注释。
        /// <para/>如果没有找到指定键，返回 false。如果找到了指定键但没有注释节点，则仍返回 false。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="comment">配置属性的注释。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetComment(string key, out string comment)
        {
            _comments.TryGetValue(key, out XComment comment_);
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
        /// 获取与指定键关联的配置属性的值。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out string value)
        {
            return _properties.TryGetValue(key, out value);
        }

        /// <summary>
        /// 添加或更新或删除一个与指定键关联的配置属性的注释。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="comment">配置属性的注释。</param>
        /// <exception cref="Exception"/>
        public bool TrySetComment(string key, string comment)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (comment == null)
            {
                if (_comments.TryGetValue(key, out XComment comment_))
                {
                    if (comment_ != null)
                    {
                        comment_.Remove();
                        _comments[key] = null;
                    }
                    return true;
                }
            }
            else
            {
                if (_comments.TryGetValue(key, out XComment comment_))
                {
                    if (comment_ == null)
                    {
                        comment_ = new XComment(comment);
                        _contents[key].AddBeforeSelf(comment_);
                        _comments[key] = comment_;
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