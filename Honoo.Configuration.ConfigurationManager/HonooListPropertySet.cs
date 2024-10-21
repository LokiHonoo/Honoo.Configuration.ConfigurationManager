using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 列表类型的配置属性集合。
    /// </summary>
    public class HonooListPropertySet : IEnumerable<HonooProperty>
    {
        private readonly XElement _container;
        private readonly List<HonooProperty> _properties = new List<HonooProperty>();

        /// <summary>
        /// 获取配置属性集合中包含的元素数。
        /// </summary>
        public int Count => _properties.Count;

        /// <summary>
        /// 获取或设置指定索引的配置属性的值。
        /// </summary>
        /// <param name="index">配置属性的索引。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty this[int index]
        {
            get => _properties[index];
            set
            {
                RemoveAt(index);
                Insert(index, value);
            }
        }

        #region Construction

        internal HonooListPropertySet(XElement container)
        {
            _container = container;
            if (_container.HasElements)
            {
                foreach (var content in _container.Elements())
                {
                    XComment comment = null;
                    XNode pre = content.PreviousNode;
                    if (pre != null && pre.NodeType == XmlNodeType.Comment)
                    {
                        comment = (XComment)pre;
                    }
                    if (content.Name == HonooSettingsManager.Namespace + "dictionary")
                    {
                        _properties.Add(new HonooDictionary(content, comment));
                    }
                    else if (content.Name == HonooSettingsManager.Namespace + "list")
                    {
                        _properties.Add(new HonooList(content, comment));
                    }
                    else if (content.Name == HonooSettingsManager.Namespace + "string")
                    {
                        _properties.Add(new HonooString(content, comment));
                    }
                    else
                    {
                        throw new ArgumentException($"The incorrect kind \"{content.Name.LocalName}\".");
                    }
                }
            }
        }

        #endregion Construction

        /// <summary>
        /// 添加一个配置属性。
        /// </summary>
        /// <typeparam name="T">指定配置属性类型。</typeparam>
        /// <param name="value">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public T Add<T>(T value) where T : HonooProperty
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
            _properties.Add(value);
            return value;
        }

        /// <summary>
        /// 添加配置属性集合。
        /// </summary>
        /// <typeparam name="T">指定配置属性类型。</typeparam>
        /// <param name="values">配置属性的集合。</param>
        /// <exception cref="Exception"/>
        public IEnumerable<T> AddRange<T>(IEnumerable<T> values) where T : HonooProperty
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            foreach (var value in values)
            {
                Add(value);
            }
            return values;
        }

        /// <summary>
        /// 从配置属性集合中移除所有配置属性。
        /// </summary>
        /// <exception cref="Exception"/>
        public void Clear()
        {
            _container.RemoveNodes();
            _properties.Clear();
        }

        /// <summary>
        /// 确定指定配置属性是否在集合中。
        /// </summary>
        /// <typeparam name="T">指定配置属性类型。</typeparam>
        /// <param name="value">搜索的指定对象。</param>
        /// <returns></returns>
        public bool Contains<T>(T value) where T : HonooProperty
        {
            return _properties.Contains(value);
        }

        /// <summary>
        /// 从指定数组索引开始将值配置属性复制到到指定数组。
        /// </summary>
        /// <typeparam name="T">指定配置属性类型。</typeparam>
        /// <param name="array">要复制到的目标数组。</param>
        /// <param name="arrayIndex">目标数组中从零开始的索引，从此处开始复制。</param>
        public void CopyTo<T>(T[] array, int arrayIndex) where T : HonooProperty
        {
            _properties.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 支持在泛型集合上进行简单迭代。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<HonooProperty> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        /// <summary>
        /// 获取与指定索引处的配置属性的值。
        /// </summary>
        /// <typeparam name="T">指定配置属性类型。</typeparam>
        /// <param name="index">配置属性的索引。</param>
        /// <exception cref="Exception"/>
        public T GetValue<T>(int index) where T : HonooProperty
        {
            return (T)_properties[index];
        }

        /// <summary>
        /// 获取与指定索引处的配置属性的值。
        /// </summary>
        /// <param name="index">配置属性的索引。</param>
        /// <exception cref="Exception"/>
        public HonooString GetValue(int index)
        {
            return (HonooString)_properties[index];
        }

        /// <summary>
        /// 搜索指定对象，并返回第一个匹配项从零开始的索引。
        /// </summary>
        /// <typeparam name="T">指定配置属性类型。</typeparam>
        /// <param name="value">搜索的指定对象。</param>
        /// <returns></returns>
        public int IndexOf<T>(T value) where T : HonooProperty
        {
            return _properties.IndexOf(value);
        }

        /// <summary>
        /// 将配置属性插入指定索引处。
        /// </summary>
        /// <typeparam name="T">指定配置属性类型。</typeparam>
        /// <param name="index">指定索引。</param>
        /// <param name="value">要插入的配置属性。</param>
        /// <exception cref="Exception"/>
        public void Insert<T>(int index, T value) where T : HonooProperty
        {
            _properties.Insert(index, value);
        }

        /// <summary>
        /// 从配置属性集合中移除指定配置属性。
        /// </summary>
        /// <typeparam name="T">指定配置属性类型。</typeparam>
        /// <param name="value">要移除的配置属性。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool Remove<T>(T value) where T : HonooProperty
        {
            value?.Comment.Remove();
            value?.Content.Remove();
            return _properties.Remove(value);
        }

        /// <summary>
        /// 从配置属性集合中移除指定索引处的配置属性。
        /// </summary>
        /// <param name="index">要移除的配置属性的索引。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public void RemoveAt(int index)
        {
            HonooProperty property = _properties[index];
            property?.Comment.Remove();
            property?.Content.Remove();
            _properties.RemoveAt(index);
        }

        /// <summary>
        /// 获取与指定索引处的配置属性的值。
        /// </summary>
        /// <typeparam name="T">指定配置属性类型。</typeparam>
        /// <param name="index">配置属性的索引。</param>
        /// <param name="value">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public T SetValue<T>(int index, T value) where T : HonooProperty
        {
            RemoveAt(index);
            Insert(index, value);
            return value;
        }

        /// <summary>
        /// 获取与指定索引处的配置属性的值。
        /// </summary>
        /// <param name="index">配置属性的索引。</param>
        /// <param name="value">配置属性的值。</param>
        /// <exception cref="Exception"/>
        public HonooString SetValue(int index, string value)
        {
            RemoveAt(index);
            HonooString val = new HonooString(value);
            Insert(index, val);
            return val;
        }
    }
}