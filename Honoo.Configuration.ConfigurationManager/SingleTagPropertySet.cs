﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置属性集合。
    /// </summary>
    public sealed class SingleTagPropertySet : IEnumerable<KeyValuePair<string, SingleTagProperty>>
    {
        #region Members

        private readonly XElement _container;
        private readonly Dictionary<string, SingleTagProperty> _properties = new Dictionary<string, SingleTagProperty>();

        /// <summary>
        /// 获取配置属性集合中包含的元素数。
        /// </summary>
        public int Count => _properties.Count;

        /// <summary>
        /// 获取配置属性集合的键的集合。
        /// </summary>
        public Dictionary<string, SingleTagProperty>.KeyCollection Keys => _properties.Keys;

        /// <summary>
        /// 获取配置属性集合的值的集合。
        /// </summary>
        public Dictionary<string, SingleTagProperty>.ValueCollection Values => _properties.Values;

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public SingleTagProperty this[string key] => GetValue(key);

        #endregion Members

        #region Construction

        internal SingleTagPropertySet(XElement container)
        {
            _container = container;
            if (_container.HasAttributes)
            {
                foreach (XAttribute attribute in _container.Attributes())
                {
                    SingleTagProperty value = new SingleTagProperty(attribute);
                    _properties.Add(attribute.Name.LocalName, value);
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
        /// <exception cref="Exception"/>
        public SingleTagProperty Add(string key, SingleTagProperty value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (value.Content == null)
            {
                value.CreateContent(key);
            }
            _container.Add(value.Content);
            _properties.Add(key, value);
            return value;
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
        public SingleTagProperty AddOrUpdate(string key, SingleTagProperty value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (TryGetValue(key, out SingleTagProperty val))
            {
                val.RemoveContent();
                if (value.Content == null)
                {
                    value.CreateContent(key);
                }
                _container.Add(value.Content);
                _properties[key] = value;
                return value;
            }
            else
            {
                return Add(key, value);
            }
        }

        #endregion AddOrUpdate

        #region GetOrAdd

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果不存在，添加一个配置属性并返回值。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="valueIfNotExists">指定键关联的配置属性不存在时添加此配置属性。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public SingleTagProperty GetOrAdd<T>(string key, SingleTagProperty valueIfNotExists)
        {
            return TryGetValue(key, out SingleTagProperty value) ? value : Add(key, valueIfNotExists);
        }

        #endregion GetOrAdd

        #region TryGetValue

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键，返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out SingleTagProperty value)
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
        public SingleTagProperty GetValue(string key)
        {
            return _properties[key];
        }

        #endregion GetValue

        #region GetValueOrDefault

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键，返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public SingleTagProperty GetValue(string key, SingleTagProperty defaultValue)
        {
            return TryGetValue(key, out SingleTagProperty value) ? value : defaultValue;
        }

        #endregion GetValueOrDefault

        /// <summary>
        /// 从配置属性集合中移除所有配置属性。
        /// </summary>
        public void Clear()
        {
            _container.RemoveAttributes();
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
        public IEnumerator<KeyValuePair<string, SingleTagProperty>> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        /// <summary>
        /// 从配置属性集合中移除带有指定键的配置属性。
        /// <br/>如果该元素成功移除，返回 <see langword="true"/>。如果没有找到指定元素，则返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool Remove(string key)
        {
            if (_properties.TryGetValue(key, out SingleTagProperty value))
            {
                value.RemoveContent();
                _properties.Remove(key);
                return true;
            }
            return false;
        }
    }
}