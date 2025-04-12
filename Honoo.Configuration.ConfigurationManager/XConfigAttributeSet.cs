using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 附加属性集合。
    /// </summary>
    public sealed class XConfigAttributeSet : IEnumerable<KeyValuePair<string, XConfigAttribute>>
    {
        #region Members

        private readonly Dictionary<string, XConfigAttribute> _attributes = new Dictionary<string, XConfigAttribute>();
        private readonly XElement _container;

        /// <summary>
        /// 获取附加属性集合中包含的元素数。
        /// </summary>
        public int Count => _attributes.Count;

        /// <summary>
        /// 获取或设置具有指定名称的附加属性的值。直接赋值等同于 AddOrUpdate 方法。
        /// </summary>
        /// <param name="name">附加属性的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public XConfigAttribute this[string name]
        {
            get { return GetValue(name); }
            set { AddOrUpdate(name, value); }
        }

        #endregion Members

        #region Construction

        internal XConfigAttributeSet(XElement container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            if (container.HasAttributes)
            {
                foreach (XAttribute attribute in container.Attributes())
                {
                    if (attribute.Name != "name" && attribute.Name != "key")
                    {
                        string name = attribute.Name.LocalName;
                        XConfigAttribute attr = new XConfigAttribute(attribute);
                        _attributes.Add(name, attr);
                    }
                }
            }
        }

        #endregion Construction

        #region Add

        /// <summary>
        /// 添加一个附加属性。
        /// </summary>
        /// <param name="name">附加属性的名称。名称不能使用关键字 "<see langword="name"/>", "<see langword="key"/>".</param>
        /// <param name="value">附加属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public XConfigAttribute Add(string name, XConfigAttribute value)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (name == "name" || name == "key")
            {
                throw new ArgumentException("Don't use keyword \"name\", \"key\".", nameof(value));
            }
            _attributes.Add(name, value);
            if (value.Content == null)
            {
                value.CreateContent(name);
            }
            _container.Add(value.Content);
            return value;
        }

        /// <summary>
        /// 添加一个附加属性。
        /// </summary>
        /// <param name="name">附加属性的名称。名称不能使用关键字 "<see langword="name"/>"， "<see langword="key"/>"。</param>
        /// <param name="value">附加属性的值。</param>
        /// <exception cref="Exception"/>
        public XConfigAttribute AddString(string name, string value)
        {
            return Add(name, new XConfigAttribute(value));
        }

        #endregion Add

        #region AddOrUpdate

        /// <summary>
        /// 添加或更新一个附加属性。
        /// </summary>
        /// <param name="name">附加属性的名称。名称不能使用关键字 "<see langword="name"/>"， "<see langword="key"/>"。</param>
        /// <param name="value">附加属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public XConfigAttribute AddOrUpdate(string name, XConfigAttribute value)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (_attributes.TryGetValue(name, out XConfigAttribute val))
            {
                val.RemoveContent();
                value.CreateContent(name);
                _container.Add(value.Content);
                _attributes[name] = value;
                return value;
            }
            else
            {
                return Add(name, value);
            }
        }

        /// <summary>
        /// 添加或更新一个附加属性。
        /// </summary>
        /// <param name="name">附加属性的名称。名称不能使用关键字 "<see langword="name"/>"， "<see langword="key"/>"。</param>
        /// <param name="value">附加属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public XConfigAttribute AddOrUpdateString(string name, string value)
        {
            return AddOrUpdate(name, new XConfigAttribute(value));
        }

        #endregion AddOrUpdate

        #region GetOrAdd

        /// <summary>
        /// 获取与指定名称关联的附加属性的值。如果不存在，添加一个附加属性并返回值。
        /// </summary>
        /// <param name="name">附加属性的名称。名称不能使用关键字 "<see langword="name"/>"， "<see langword="key"/>"。</param>
        /// <param name="valueIfNotExists">指定名称关联的附加属性不存在时添加此附加属性。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public XConfigAttribute GetOrAdd(string name, XConfigAttribute valueIfNotExists)
        {
            return TryGetValue(name, out XConfigAttribute value) ? value : Add(name, valueIfNotExists);
        }

        /// <summary>
        /// 获取与指定名称关联的附加属性的值。如果不存在，添加一个附加属性并返回值。
        /// </summary>
        /// <param name="name">附加属性的名称。名称不能使用关键字 "<see langword="name"/>"， "<see langword="key"/>"。</param>
        /// <param name="valueIfNotExists">指定名称关联的附加属性不存在时添加此附加属性。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public string GetOrAddString(string name, string valueIfNotExists)
        {
            return TryGetValue(name, out XConfigAttribute value) ? value.GetStringValue() : Add(name, new XConfigAttribute(valueIfNotExists)).GetStringValue();
        }

        #endregion GetOrAdd

        #region TryGetValue

        /// <summary>
        /// 获取与指定名称关联的附加属性的值。
        /// </summary>
        /// <param name="name">附加属性的名称。</param>
        /// <param name="value">附加属性的值。</param>
        /// <returns></returns>
        public bool TryGetStringValue(string name, out string value)
        {
            if (_attributes.TryGetValue(name, out XConfigAttribute val))
            {
                value = val.GetStringValue();
                return true;
            }
            value = null;
            return false;
        }

        /// <summary>
        /// 获取与指定名称关联的附加属性的值。
        /// </summary>
        /// <param name="name">附加属性的名称。</param>
        /// <param name="value">附加属性的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string name, out XConfigAttribute value)
        {
            return _attributes.TryGetValue(name, out value);
        }

        #endregion TryGetValue

        #region GetValue

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// </summary>
        /// <param name="name">附加属性的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public string GetStringValue(string name)
        {
            return _attributes[name].GetStringValue();
        }

        /// <summary>
        /// 获取与指定名称关联的附加属性的值。
        /// </summary>
        /// <param name="name">附加属性的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public XConfigAttribute GetValue(string name)
        {
            return TryGetValue(name, out XConfigAttribute value) ? value : null;
        }

        #endregion GetValue

        #region GetValueOrDefault

        /// <summary>
        /// 获取与指定名称关联的附加属性的值。如果没有找到指定名称，返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="name">附加属性的名称。</param>
        /// <param name="defaultValue">没有找到指定名称时的附加属性的默认值。</param>
        /// <returns></returns>
        public string GetStringValue(string name, string defaultValue)
        {
            return TryGetValue(name, out XConfigAttribute value) ? value.GetStringValue() : defaultValue;
        }

        /// <summary>
        /// 获取与指定名称关联的附加属性的值。如果没有找到指定名称，返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="name">附加属性的名称。</param>
        /// <param name="defaultValue">没有找到指定名称时的附加属性的默认值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public XConfigAttribute GetValue(string name, XConfigAttribute defaultValue)
        {
            return TryGetValue(name, out XConfigAttribute value) ? value : defaultValue;
        }

        #endregion GetValueOrDefault

        /// <summary>
        /// 从附加属性集合中移除所有附加属性。
        /// </summary>
        public void Clear()
        {
            foreach (string name in _attributes.Keys)
            {
                Remove(name);
            }
        }

        /// <summary>
        /// 确定附加属性集合是否包含带有指定名称的附加属性。
        /// </summary>
        /// <param name="name">附加属性的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool ContainsName(string name)
        {
            return _attributes.ContainsKey(name);
        }

        /// <summary>
        /// 返回循环访问集合的枚举数。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, XConfigAttribute>> GetEnumerator()
        {
            return _attributes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _attributes.GetEnumerator();
        }

        /// <summary>
        /// 从附加属性集合中移除带有指定名称的附加属性。
        /// <br/>如果该元素成功移除，返回 <see langword="true"/>。如果没有找到指定名称，则返回 <see langword="false"/>。
        /// </summary>
        /// <param name="name">附加属性的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool Remove(string name)
        {
            if (_attributes.TryGetValue(name, out XConfigAttribute value))
            {
                value.RemoveContent();
                _attributes.Remove(name);
                return true;
            }
            return false;
        }
    }
}