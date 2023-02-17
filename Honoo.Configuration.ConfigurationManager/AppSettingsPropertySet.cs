using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置属性集合。
    /// </summary>
    public sealed class AppSettingsPropertySet : IEnumerable<KeyValuePair<string, string>>, IEnumerable
    {
        private readonly IDictionary<string, XElement> _contents = new Dictionary<string, XElement>();
        private readonly IDictionary<string, string> _properties = new Dictionary<string, string>();
        private readonly XElement _superior;

        /// <summary>
        /// 获取配置属性集合中包含的元素数。
        /// </summary>
        public int Count => _properties.Count;

        /// <summary>
        /// 获取配置属性集合的键的集合。
        /// </summary>
        public ICollection<string> Keys => _properties.Keys;

        /// <summary>
        /// 获取配置属性集合的值的集合。
        /// </summary>
        public ICollection<string> Values => _properties.Values;

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

        #region Construction

        internal AppSettingsPropertySet(XElement superior)
        {
            _superior = superior;
            if (superior.HasElements)
            {
                foreach (XElement content in superior.Elements("add"))
                {
                    string key = content.Attribute("key").Value;
                    string value = content.Attribute("value").Value;
                    _properties.Add(key, value);
                    _contents.Add(key, content);
                }
            }
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
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"The invalid key - {nameof(key)}.");
            }
            if (value is null)
            {
                if (_properties.Remove(key))
                {
                    _contents[key].Remove();
                    _contents.Remove(key);
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
                    XElement content = new XElement("add");
                    content.SetAttributeValue("key", key);
                    content.SetAttributeValue("value", value);
                    _properties.Add(key, value);
                    _contents.Add(key, content);
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
        /// 从配置属性集合中移除带有指定键的配置属性。
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
                return true;
            }
            else
            {
                return false;
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
    }
}