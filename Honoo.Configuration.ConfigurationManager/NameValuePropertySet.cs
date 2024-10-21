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
    public sealed class NameValuePropertySet : IEnumerable<KeyValuePair<string, AddProperty[]>>
    {
        #region Properties

        private readonly XElement _container;
        private readonly Dictionary<string, AddProperty[]> _properties = new Dictionary<string, AddProperty[]>();

        /// <summary>
        /// 获取配置属性集合中包含的元素数。
        /// </summary>
        public int Count => _properties.Count;

        /// <summary>
        /// 获取配置属性集合的键的集合。
        /// </summary>
        public Dictionary<string, AddProperty[]>.KeyCollection Keys => _properties.Keys;

        /// <summary>
        /// 获取配置属性集合的值的集合。
        /// </summary>
        public Dictionary<string, AddProperty[]>.ValueCollection Values => _properties.Values;

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:属性不应返回数组", Justification = "<挂起>")]
        public AddProperty[] this[string key] => GetValue(key);

        #endregion Properties

        #region Construction

        internal NameValuePropertySet(XElement container)
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
                                var key = content.Attribute("key").Value;
                                AddProperty value = new AddProperty(content, comment);
                                if (_properties.TryGetValue(key, out AddProperty[] val))
                                {
                                    _properties[key] = new List<AddProperty>(val) { value }.ToArray();
                                }
                                else
                                {
                                    _properties.Add(key, new AddProperty[] { value });
                                }
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
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty Add(string key, AddProperty value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (value.Comment.HasValue)
            {
                _container.Add(value.Comment.Comment);
            }
            value.Content.SetAttributeValue("key", key);
            _container.Add(value.Content);
            if (_properties.TryGetValue(key, out AddProperty[] val))
            {
                _properties[key] = new List<AddProperty>(val) { value }.ToArray();
            }
            else
            {
                _properties.Add(key, new AddProperty[] { value });
            }
            return value;
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty Add(string key, string value)
        {
            return Add(key, new AddProperty(value));
        }

        #endregion Add

        #region TryGetValue

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out AddProperty[] value)
        {
            return _properties.TryGetValue(key, out value);
        }

        #endregion TryGetValue

        #region GetValue

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty[] GetValue(string key)
        {
            return _properties[key];
        }

        #endregion GetValue

        #region GetValueOrDefault

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty[] GetValue(string key, AddProperty[] defaultValue)
        {
            return TryGetValue(key, out AddProperty[] value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty[] GetValue(string key, string[] defaultValue)
        {
            if (defaultValue is null)
            {
                throw new ArgumentNullException(nameof(defaultValue));
            }
            if (TryGetValue(key, out AddProperty[] value))
            {
                return value;
            }
            var val = new List<AddProperty>();
            foreach (string d in defaultValue)
            {
                val.Add(new AddProperty(d));
            }
            return val.ToArray();
        }

        #endregion GetValueOrDefault

        /// <summary>
        /// 从配置属性集合中移除所有配置属性。
        /// </summary>
        public void Clear()
        {
            _container.RemoveNodes();
            _properties.Clear();
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
        /// 返回循环访问集合的枚举数。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, AddProperty[]>> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        /// <summary>
        /// 从配置属性集合中移除带有指定键的配置属性。和指定键关联的配置属性的注释一并移除。
        /// <br/>如果该元素成功移除，返回 <see langword="true"/>。如果没有找到指定元素，则返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool Remove(string key)
        {
            if (_properties.TryGetValue(key, out AddProperty[] value))
            {
                foreach (var val in value)
                {
                    val.Comment.Remove();
                    val.Content.Remove();
                }
                _properties.Remove(key);
                return true;
            }
            return false;
        }
    }
}