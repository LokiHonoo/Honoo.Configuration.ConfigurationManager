﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置属性集合。
    /// </summary>
    public sealed class DictionarySectionPropertySet : IEnumerable<KeyValuePair<string, object>>, IEnumerable
    {
        private readonly IDictionary<string, XElement> _contents = new Dictionary<string, XElement>();
        private readonly ISavable _savable;
        private readonly XElement _superior;
        private readonly IDictionary<string, object> _values = new Dictionary<string, object>();

        /// <summary>
        /// 获取配置属性集合中包含的元素数。
        /// </summary>
        public int Count => _values.Count;

        /// <summary>
        /// 获取配置属性集合的键的集合。
        /// </summary>
        public ICollection<string> Keys => _values.Keys;

        /// <summary>
        /// 获取配置属性集合的值的集合。
        /// </summary>
        public ICollection<object> Values => _values.Values;

        /// <summary>
        /// 获取或设置具有指定键的配置属性的值。直接赋值等同于 AddOrUpdate 方法。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public object this[string key]
        {
            get => _values.TryGetValue(key, out object value) ? value : null;
            set { AddOrUpdate(key, value); }
        }

        #region Construction

        internal DictionarySectionPropertySet(XElement superior, ISavable savable)
        {
            _superior = superior;
            _savable = savable;
            if (superior.HasElements)
            {
                foreach (XElement content in superior.Elements("add"))
                {
                    string key = content.Attribute("key").Value;
                    object value = XValueHelper.GetDictionarySectionValue(content);
                    _values.Add(key, value);
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
        public void AddOrUpdate(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"The invalid key - {nameof(key)}.");
            }
            if (value is null)
            {
                if (_values.Remove(key))
                {
                    _contents[key].Remove();
                    _contents.Remove(key);
                    if (_savable.AutoSave)
                    {
                        _savable.Save();
                    }
                }
            }
            else
            {
                if (_values.ContainsKey(key))
                {
                    XValueHelper.SetDictionarySectionValue(value, _contents[key]);
                    _values[key] = value;
                }
                else
                {
                    XElement content = new XElement("add");
                    content.SetAttributeValue("key", key);
                    XValueHelper.SetDictionarySectionValue(value, content);
                    _values.Add(key, value);
                    _contents.Add(key, content);
                    _superior.Add(content);
                }
                if (_savable.AutoSave)
                {
                    _savable.Save();
                }
            }
        }

        /// <summary>
        /// 从配置属性集合中移除所有配置属性。
        /// </summary>
        public void Clear()
        {
            _values.Clear();
            _contents.Clear();
            _superior.RemoveNodes();
            if (_savable.AutoSave)
            {
                _savable.Save();
            }
        }

        /// <summary>
        /// 确定配置属性集合是否包含带有指定键的配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool ContainsKey(string key)
        {
            return _values.ContainsKey(key);
        }

        /// <summary>
        /// 支持在泛型集合上进行简单迭代。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        /// <summary>
        /// 从配置属性集合中移除带有指定键的配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool Remove(string key)
        {
            if (_values.Remove(key))
            {
                _contents[key].Remove();
                _contents.Remove(key);
                if (_savable.AutoSave)
                {
                    _savable.Save();
                }
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
        public bool TryGetValue(string key, out object value)
        {
            return _values.TryGetValue(key, out value);
        }
    }
}