using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 附加属性集合。
    /// </summary>
    public sealed class XConfigAttributeSet : IEnumerable<XConfigAttribute>
    {
        #region Members

        private readonly Dictionary<string, XConfigAttribute> _attributes = new Dictionary<string, XConfigAttribute>();
        private readonly XElement _container;

        /// <summary>
        /// 获取附加属性集合中包含的元素数。
        /// </summary>
        public int Count => _attributes.Count;

        /// <summary>
        /// 获取与指定键关联的附加属性。
        /// </summary>
        /// <param name="key">附加属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public XConfigAttribute this[string key] => GetValue(key);

        #endregion Members

        #region Construction

        internal XConfigAttributeSet(XElement container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            if (container.HasAttributes)
            {
                foreach (XAttribute attribute in container.Attributes())
                {
                    if (attribute.Name != "key")
                    {
                        string key = attribute.Name.LocalName;
                        XConfigAttribute attr = new XConfigAttribute(attribute);
                        _attributes.Add(key, attr);
                    }
                }
            }
        }

        #endregion Construction

        #region Add

        /// <summary>
        /// 添加一个附加属性。
        /// </summary>
        /// <param name="key">附加属性的键。键的名称不能使用关键字 <see langword="key"/>.</param>
        /// <param name="value">附加属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public XConfigAttribute Add(string key, XConfigAttribute value)
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
            _attributes.Add(key, value);
            return value;
        }

        #endregion Add

        #region AddOrUpdate

        /// <summary>
        /// 添加或更新一个附加属性。
        /// </summary>
        /// <param name="key">附加属性的键。</param>
        /// <param name="value">附加属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public XConfigAttribute AddOrUpdate(string key, XConfigAttribute value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (TryGetValue(key, out XConfigAttribute val))
            {
                val.RemoveContent();
                value.CreateContent(key);
                _container.Add(value.Content);
                _attributes[key] = value;
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
        /// 获取与指定键关联的附加属性的值。如果不存在，添加一个附加属性并返回值。
        /// </summary>
        /// <param name="key">附加属性的键。</param>
        /// <param name="valueIfNotExists">指定键关联的附加属性不存在时添加此附加属性。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public XConfigAttribute GetOrAdd<T>(string key, XConfigAttribute valueIfNotExists)
        {
            return TryGetValue(key, out XConfigAttribute value) ? value : Add(key, valueIfNotExists);
        }

        #endregion GetOrAdd

        #region TryGetValue

        /// <summary>
        /// 获取与指定键关联的附加属性的值。
        /// </summary>
        /// <param name="key">附加属性的键。</param>
        /// <param name="value">附加属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string key, out XConfigAttribute value)
        {
            return _attributes.TryGetValue(key, out value);
        }

        #endregion TryGetValue

        #region GetValue

        /// <summary>
        /// 获取与指定键关联的附加属性的值。
        /// </summary>
        /// <param name="key">附加属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public XConfigAttribute GetValue(string key)
        {
            return TryGetValue(key, out XConfigAttribute value) ? value : null;
        }

        #endregion GetValue

        #region GetValueOrDefault

        /// <summary>
        /// 获取与指定键关联的附加属性的值。如果没有找到指定键，返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">附加属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的附加属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public XConfigAttribute GetValue(string key, XConfigAttribute defaultValue)
        {
            return TryGetValue(key, out XConfigAttribute value) ? value : defaultValue;
        }

        #endregion GetValueOrDefault

        /// <summary>
        /// 从附加属性集合中移除所有附加属性。
        /// </summary>
        public void Clear()
        {
            foreach (string key in _attributes.Keys)
            {
                Remove(key);
            }
        }

        /// <summary>
        /// 确定附加属性集合是否包含带有指定键的附加属性。
        /// </summary>
        /// <param name="key">附加属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool ContainsKey(string key)
        {
            return _attributes.ContainsKey(key);
        }

        /// <summary>
        /// 返回循环访问集合的枚举数。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<XConfigAttribute> GetEnumerator()
        {
            return _attributes.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _attributes.Values.GetEnumerator();
        }

        /// <summary>
        /// 从附加属性集合中移除带有指定键的附加属性。
        /// <br/>如果该元素成功移除，返回 <see langword="true"/>。如果没有找到指定键，则返回 <see langword="false"/>。
        /// </summary>
        /// <param name="key">附加属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool Remove(string key)
        {
            if (_attributes.TryGetValue(key, out XConfigAttribute value))
            {
                value.RemoveContent();
                _attributes.Remove(key);
                return true;
            }
            return false;
        }
    }
}