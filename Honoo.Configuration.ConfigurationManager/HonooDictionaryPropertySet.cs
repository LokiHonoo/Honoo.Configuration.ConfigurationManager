using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 字典类型的配置属性集合。
    /// </summary>
    public class HonooDictionaryPropertySet : IEnumerable<KeyValuePair<string, HonooProperty>>
    {
        #region Properties

        private readonly XElement _container;
        private readonly Dictionary<string, HonooProperty> _properties = new Dictionary<string, HonooProperty>();

        /// <summary>
        /// 获取配置属性集合中包含的元素数。
        /// </summary>
        public int Count => _properties.Count;

        /// <summary>
        /// 获取配置属性集合的键的集合。
        /// </summary>
        public Dictionary<string, HonooProperty>.KeyCollection Keys => _properties.Keys;

        /// <summary>
        /// 获取配置属性集合的值的集合。
        /// </summary>
        public Dictionary<string, HonooProperty>.ValueCollection Values => _properties.Values;

        /// <summary>
        /// 获取或设置具有指定键的配置属性的值。直接赋值等同于 AddOrUpdate 方法。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        public HonooProperty this[string key]
        {
            get { return GetValue<HonooProperty>(key); }
            set { AddOrUpdate(key, value); }
        }

        #endregion Properties

        #region Construction

        internal HonooDictionaryPropertySet(XElement container)
        {
            _container = container;
            if (_container.HasElements)
            {
                foreach (var content in _container.Elements())
                {
                    var key = content.Attribute("key").Value;
                    _properties.Remove(key);
                    //
                    XComment comment = null;
                    XNode pre = content.PreviousNode;
                    if (pre != null && pre.NodeType == XmlNodeType.Comment)
                    {
                        comment = (XComment)pre;
                    }
                    if (content.Name == HonooSettingsManager.Namespace + "dictionary")
                    {
                        _properties.Add(key, new HonooDictionary(content, comment));
                    }
                    else if (content.Name == HonooSettingsManager.Namespace + "list")
                    {
                        _properties.Add(key, new HonooList(content, comment));
                    }
                    else if (content.Name == HonooSettingsManager.Namespace + "string")
                    {
                        _properties.Add(key, new HonooString(content, comment));
                    }
                    else
                    {
                        throw new ArgumentException($"The incorrect kind \"{content.Name.LocalName}\".");
                    }
                }
            }
        }

        #endregion Construction

        #region Add

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <typeparam name="T">指定配置属性类型。</typeparam>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public T Add<T>(string key, T value) where T : HonooProperty
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
        /// <exception cref="Exception"/>
        public HonooString Add(string key, string value)
        {
            return Add(key, new HonooString(value));
        }

        #endregion Add

        #region GetOrAdd

        /// <summary>
        /// 获取与指定名称关联的配置属性。如果不存在，添加一个 <typeparamref name="T"/> 类型的配置属性并返回值。
        /// <br/>如果配置属性存在但不是指定的类型，则抛出 <see cref="InvalidCastException"/>。
        /// </summary>
        /// <typeparam name="T">指定配置属性类型。</typeparam>
        /// <param name="key">配置属性的键。</param>
        /// <param name="valueIfNotExists">指定名称关联的配置属性不存在时添加此配置属性。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public T GetOrAdd<T>(string key, T valueIfNotExists) where T : HonooProperty
        {
            if (TryGetValue(key, out HonooProperty value))
            {
                if (value is T val)
                {
                    return val;
                }
                else
                {
                    throw new InvalidCastException($"The property exists but is not of the specified type - property type:{value.GetType()}.");
                }
            }
            else
            {
                return Add(key, valueIfNotExists);
            }
        }

        /// <summary>
        /// 获取与指定名称关联的配置属性。如果不存在，添加一个 <see cref="HonooString"/> 类型的配置属性并返回值。
        /// <br/>如果配置属性存在但不是指定的类型，则抛出 <see cref="InvalidCastException"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="valueIfNotExists">指定名称关联的配置属性不存在时添加此配置属性。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooString GetOrAdd(string key, string valueIfNotExists)
        {
            return GetOrAdd(key, new HonooString(valueIfNotExists));
        }

        #endregion GetOrAdd

        #region AddOrUpdate

        /// <summary>
        /// 添加或更新一个配置属性。
        /// </summary>
        /// <typeparam name="T">指定配置属性类型。</typeparam>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public T AddOrUpdate<T>(string key, T value) where T : HonooProperty
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (TryGetValue(key, out HonooProperty prop))
            {
                if (value.Comment.HasValue)
                {
                    prop.Content.AddBeforeSelf(value.Comment.Comment);
                }
                value.Content.SetAttributeValue("key", key);
                prop.Content.AddBeforeSelf(value.Content);
                prop.Comment.Remove();
                prop.Content.Remove();
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
        /// <exception cref="Exception"/>
        public HonooString AddOrUpdate(string key, string value)
        {
            return AddOrUpdate(key, new HonooString(value));
        }

        #endregion AddOrUpdate

        #region TryGetValue

        /// <summary>
        /// 获取与指定键关联的配置属性的值。
        /// <br/>如果没有找到指定键，返回 <see langword="false"/>。如果找到了指定键但指定的类型不符，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <typeparam name="T">指定配置属性类型。</typeparam>
        /// <param name="key">配置属性的键。</param>
        /// <param name="value">配置属性的值。</param>
        /// <returns></returns>
        public bool TryGetValue<T>(string key, out T value) where T : HonooProperty
        {
            if (_properties.TryGetValue(key, out HonooProperty val))
            {
                if (val is T v)
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
        /// <typeparam name="T">指定配置属性类型。</typeparam>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public T GetValue<T>(string key) where T : HonooProperty
        {
            return (T)_properties[key];
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，则抛出 <see cref="Exception"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooString GetValue(string key)
        {
            return (HonooString)_properties[key];
        }

        #endregion GetValue

        #region GetValueOrDefault

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <typeparam name="T">指定配置属性类型。</typeparam>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        public T GetValue<T>(string key, T defaultValue) where T : HonooProperty
        {
            return TryGetValue(key, out T value) ? value : defaultValue;
        }

        /// <summary>
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <paramref name="defaultValue"/>。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <param name="defaultValue">没有找到指定键时的配置属性的默认值。</param>
        /// <returns></returns>
        public HonooString GetValue(string key, string defaultValue)
        {
            return TryGetValue(key, out HonooString value) ? value : new HonooString(defaultValue);
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
        public bool ContainsKey(string key)
        {
            return _properties.ContainsKey(key);
        }

        /// <summary>
        /// 支持在泛型集合上进行简单迭代。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, HonooProperty>> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        /// <summary>
        /// 从配置属性集合中移除带有指定键的配置属性。
        /// <para/>如果该配置属性成功移除，返回 true。如果没有找到指定键，则仍返回 false。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool Remove(string key)
        {
            if (_properties.TryGetValue(key, out HonooProperty value))
            {
                value.Comment.Remove();
                value.Content.Remove();
                _properties.Remove(key);
                return true;
            }
            return false;
        }
    }
}