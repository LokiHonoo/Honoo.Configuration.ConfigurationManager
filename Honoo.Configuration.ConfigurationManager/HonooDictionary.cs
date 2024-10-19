using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 字典类型。
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1711:标识符应采用正确的后缀", Justification = "<挂起>")]
    public class HonooDictionary : HonooProperty, IEnumerable<KeyValuePair<string, HonooProperty>>
    {
        #region Class

        /// <summary>
        /// 代表此配置属性集合的键的集合。
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:嵌套类型应不可见", Justification = "<挂起>")]
        public sealed class KeyCollection : IEnumerable<string>
        {
            #region Properties

            private readonly Dictionary<string, HonooProperty> _elements;

            /// <summary>
            /// 获取配置属性的键集合中包含的元素数。
            /// </summary>
            public int Count => _elements.Count;

            #endregion Properties

            internal KeyCollection(Dictionary<string, HonooProperty> elements)
            {
                _elements = elements;
            }

            /// <summary>
            /// 从指定数组索引开始将键配置属性复制到到指定数组。
            /// </summary>
            /// <param name="array">目标数组。</param>
            /// <param name="arrayIndex">目标数组中从零开始的索引，从此处开始复制。</param>
            public void CopyTo(string[] array, int arrayIndex)
            {
                _elements.Keys.CopyTo(array, arrayIndex);
            }

            /// <summary>
            /// 支持在泛型集合上进行简单迭代。
            /// </summary>
            /// <returns></returns>
            public IEnumerator<string> GetEnumerator()
            {
                return _elements.Keys.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _elements.Keys.GetEnumerator();
            }
        }

        /// <summary>
        /// 代表此配置属性集合的值的集合。
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:嵌套类型应不可见", Justification = "<挂起>")]
        public sealed class ValueCollection : IEnumerable<HonooProperty>
        {
            #region Properties

            private readonly Dictionary<string, HonooProperty> _elements;

            /// <summary>
            /// 获取配置属性的值集合中包含的元素数。
            /// </summary>
            public int Count => _elements.Count;

            #endregion Properties

            internal ValueCollection(Dictionary<string, HonooProperty> elements)
            {
                _elements = elements;
            }

            /// <summary>
            /// 从指定数组索引开始将值配置属性复制到到指定数组。
            /// </summary>
            /// <typeparam name="T">指定配置属性类型。</typeparam>
            /// <param name="array">目标数组。</param>
            /// <param name="arrayIndex">目标数组中从零开始的索引，从此处开始复制。</param>
            public void CopyTo<T>(T[] array, int arrayIndex) where T : HonooProperty
            {
                _elements.Values.CopyTo(array, arrayIndex);
            }

            /// <summary>
            /// 支持在泛型集合上进行简单迭代。
            /// </summary>
            /// <returns></returns>
            public IEnumerator<HonooProperty> GetEnumerator()
            {
                return _elements.Values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _elements.Values.GetEnumerator();
            }
        }

        #endregion Class

        #region Properties

        private readonly Dictionary<string, HonooProperty> _elements = new Dictionary<string, HonooProperty>();
        private readonly KeyCollection _keyExhibits;
        private readonly ValueCollection _valueExhibits;

        /// <summary>
        /// 获取配置属性集合中包含的元素数。
        /// </summary>
        public int Count => _elements.Count;

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
        public HonooProperty this[string key]
        {
            get { return GetValue<HonooProperty>(key); }
            set { AddOrUpdate(key, value); }
        }

        #endregion Properties

        #region Construction

        /// <summary>
        /// 初始化 HonooDictionary 类的新实例。
        /// </summary>
        public HonooDictionary() : base(HonooPropertyKind.HonooDictionary, new XElement(HonooSettingsManager.Namespace + "dictionary"), null)
        {
            _keyExhibits = new KeyCollection(_elements);
            _valueExhibits = new ValueCollection(_elements);
        }

        internal HonooDictionary(XElement content, XComment comment) : base(HonooPropertyKind.HonooDictionary, content, comment)
        {
            if (content.HasElements)
            {
                foreach (var element in content.Elements())
                {
                    var key = element.Attribute("key").Value;
                    XComment comment_ = null;
                    XNode pre = element.PreviousNode;
                    if (pre != null && pre.NodeType == XmlNodeType.Comment)
                    {
                        comment_ = (XComment)pre;
                    }
                    if (element.Name == HonooSettingsManager.Namespace + "dictionary")
                    {
                        _elements.Add(key, new HonooDictionary(element, comment_));
                    }
                    else if (element.Name == HonooSettingsManager.Namespace + "list")
                    {
                        _elements.Add(key, new HonooList(element, comment_));
                    }
                    else if (element.Name == HonooSettingsManager.Namespace + "string")
                    {
                        _elements.Add(key, new HonooString(element, comment_));
                    }
                    else
                    {
                        throw new ArgumentException($"The incorrect kind \"{element.Name.LocalName}\".");
                    }
                }
            }
            _keyExhibits = new KeyCollection(_elements);
            _valueExhibits = new ValueCollection(_elements);
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
                base.Content.Add(value.Comment.Comment);
            }
            value.Content.SetAttributeValue("key", key);
            base.Content.Add(value.Content);
            _elements.Add(key, value);
            return value;
        }

        #endregion Add

        #region GetOrAdd

        /// <summary>
        /// 获取与指定名称关联的配置属性。如果不存在，添加一个 <typeparamref name="T"/> 类型的配置属性并返回值。
        /// <br/>如果配置属性存在但不是指定的类型，方法抛出 <see cref="ArgumentException"/>。
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
                    throw new ArgumentException($"The section exists but is not of the specified type - {typeof(T)}.");
                }
            }
            else
            {
                return Add(key, valueIfNotExists);
            }
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
                _elements[key] = value;
                return value;
            }
            else
            {
                return Add(key, value);
            }
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
            if (_elements.TryGetValue(key, out HonooProperty val))
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
        /// 获取与指定键关联的配置属性的值。如果没有找到指定键或者无法转换指定的类型，返回 <see langword="null"/>。
        /// </summary>
        /// <typeparam name="T">指定配置属性类型。</typeparam>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        public T GetValue<T>(string key) where T : HonooProperty
        {
            return (T)_elements[key];
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

        #endregion GetValueOrDefault

        /// <summary>
        /// 从配置属性集合中移除所有配置属性。
        /// </summary>
        public void Clear()
        {
            base.Content.RemoveNodes();
            _elements.Clear();
        }

        /// <summary>
        /// 确定配置属性集合是否包含带有指定键的配置属性。
        /// </summary>
        /// <param name="key">配置属性的键。</param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return _elements.ContainsKey(key);
        }

        /// <summary>
        /// 支持在泛型集合上进行简单迭代。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, HonooProperty>> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _elements.GetEnumerator();
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
            if (_elements.TryGetValue(key, out HonooProperty value))
            {
                value.Comment.Remove();
                value.Content.Remove();
                _elements.Remove(key);
                return true;
            }
            return false;
        }
    }
}