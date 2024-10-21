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
    public sealed class DictionaryPropertySet : IEnumerable<KeyValuePair<string, TagProperty>>
    {
        #region Properties

        private readonly XElement _container;
        private readonly Dictionary<string, TagProperty> _properties = new Dictionary<string, TagProperty>();

        /// <summary>
        /// 获取配置属性集合中包含的元素数。
        /// </summary>
        public int Count => _properties.Count;

        /// <summary>
        /// 获取配置属性集合的键的集合。
        /// </summary>
        public Dictionary<string, TagProperty>.KeyCollection Keys => _properties.Keys;

        /// <summary>
        /// 获取配置属性集合的值的集合。
        /// </summary>
        public Dictionary<string, TagProperty>.ValueCollection Values => _properties.Values;

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty this[string key] => GetValue(key);

        #endregion Properties

        #region Construction

        internal DictionaryPropertySet(XElement container)
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
                                _properties.Remove(key);
                                _properties.Add(key, value);
                            }
                            else if (content.Name == "remove")
                            {
                                var key = "{remove_" + Guid.NewGuid().ToString("N") + "}" + content.Attribute("key").Value;
                                RemoveProperty value = new RemoveProperty(content, comment);
                                _properties.Add(key, value);
                            }
                            else if (content.Name == "clear")
                            {
                                var key = "{clear_" + Guid.NewGuid().ToString("N") + "}";
                                ClearProperty value = new ClearProperty(content, comment);
                                _properties.Add(key, value);
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
            _properties.Add(key, value);
            return value;
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public RemoveProperty Add(string key, RemoveProperty value)
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
            value.Content.SetAttributeValue("key", "{remove_" + Guid.NewGuid().ToString("N") + "}" + key);
            _container.Add(value.Content);
            _properties.Add(key, value);
            return value;
        }

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ClearProperty Add(ClearProperty value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (value.Comment.HasValue)
            {
                _container.Add(value.Comment.Comment);
            }
            _container.Add(value.Content);
            _properties.Add("{clear_" + Guid.NewGuid().ToString("N") + "}", value);
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

        #region AddOrUpdate

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty AddOrUpdate(string key, AddProperty value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (TryGetValue(key, out AddProperty val))
            {
                if (value.Comment.HasValue)
                {
                    val.Content.AddBeforeSelf(value.Comment.Comment);
                }
                value.Content.SetAttributeValue("key", key);
                val.Content.AddBeforeSelf(value.Content);
                val.Comment.Remove();
                val.Content.Remove();
                _properties[key] = value;
                return value;
            }
            else
            {
                return Add(key, value);
            }
        }

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty AddOrUpdate(string key, string value)
        {
            return AddOrUpdate(key, new AddProperty(value));
        }

        #endregion AddOrUpdate

        #region TryGetValue

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但指定的类型不符，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out AddProperty value)
        {
            if (_properties.TryGetValue(key, out TagProperty val))
            {
                if (val is AddProperty v)
                {
                    value = v;
                    return true;
                }
            }
            value = null;
            return false;
        }

        #endregion TryGetValue

        #region GetValue

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，则抛出 <see cref="Exception"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty GetValue(string key)
        {
            return (AddProperty)_properties[key];
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
        public AddProperty GetValue(string key, AddProperty defaultValue)
        {
            return TryGetValue(key, out AddProperty value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public AddProperty GetValue(string key, string defaultValue)
        {
            return TryGetValue(key, out AddProperty value) ? value : new AddProperty(defaultValue);
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
        public IEnumerator<KeyValuePair<string, TagProperty>> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        /// <summary>
        /// 从配置属性集合中移除配置属性。配置属性的注释一并移除。
        /// <br/>如果该元素成功移除，返回 <see langword="true"/>。如果没有找到指定元素，则返回 <see langword="false"/>。
        /// </summary>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool Remove(TagProperty value)
        {
            string key = null;
            foreach (var val in _properties)
            {
                if (val.Value == value)
                {
                    key = val.Key;
                }
            }
            if (key != null)
            {
                TagProperty val = _properties[key];
                val.Comment.Remove();
                val.Content.Remove();
                _properties.Remove(key);
                return true;
            }
            return false;
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
            foreach (var value in _properties)
            {
                if (value.Key == key)
                {
                    value.Value.Comment.Remove();
                    value.Value.Content.Remove();
                    _properties.Remove(key);
                    return true;
                }
                else if (value.Value is RemoveProperty _)
                {
                    if (value.Key.Remove(0, 41) == key)
                    {
                        value.Value.Comment.Remove();
                        value.Value.Content.Remove();
                        _properties.Remove(key);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}