﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 列表类型。
    /// </summary>
    public class HonooList : HonooProperty, IEnumerable<HonooProperty>
    {
        private readonly List<HonooProperty> _elements = new List<HonooProperty>();

        /// <summary>
        /// 获取配置属性集合中包含的元素数。
        /// </summary>
        public int Count => _elements.Count;

        /// <summary>
        /// 获取或设置指定索引的配置属性的值。
        /// </summary>
        /// <param name="index">配置属性的索引。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public HonooProperty this[int index]
        {
            get => _elements[index];
            set
            {
                RemoveAt(index);
                Insert(index, value);
            }
        }

        #region Construction

        /// <summary>
        /// 初始化 HonooList 类的新实例。
        /// </summary>
        public HonooList() : base(HonooPropertyKind.HonooList, new XElement(HonooSettingsManager.Namespace + "list"), null)
        {
        }

        internal HonooList(XElement content, XComment comment) : base(HonooPropertyKind.HonooList, content, comment)
        {
            if (content.HasElements)
            {
                foreach (var element in content.Elements())
                {
                    XComment comment_ = null;
                    XNode pre = content.PreviousNode;
                    if (pre != null && pre.NodeType == XmlNodeType.Comment)
                    {
                        comment_ = (XComment)pre;
                    }
                    if (element.Name == HonooSettingsManager.Namespace + "dictionary")
                    {
                        _elements.Add(new HonooDictionary(element, comment_));
                    }
                    else if (element.Name == HonooSettingsManager.Namespace + "list")
                    {
                        _elements.Add(new HonooList(element, comment_));
                    }
                    else if (element.Name == HonooSettingsManager.Namespace + "string")
                    {
                        _elements.Add(new HonooString(element, comment_));
                    }
                    else
                    {
                        throw new ArgumentException($"The incorrect kind \"{element.Name.LocalName}\".");
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
                base.Content.Add(value.Comment.Comment);
            }
            base.Content.Add(value.Content);
            _elements.Add(value);
            return value;
        }

        /// <summary>
        /// 添加配置属性集合。
        /// </summary>
        /// <typeparam name="T">指定配置属性类型。</typeparam>
        /// <param name="values">配置属性的集合。</param>
        /// <exception cref="Exception"/>
        public IEnumerable<HonooProperty> AddRange<T>(IEnumerable<HonooProperty> values) where T : HonooProperty
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
            base.Content.RemoveNodes();
            _elements.Clear();
        }

        /// <summary>
        /// 确定指定配置属性是否在集合中。
        /// </summary>
        /// <typeparam name="T">指定配置属性类型。</typeparam>
        /// <param name="value">搜索的指定对象。</param>
        /// <returns></returns>
        public bool Contains<T>(T value) where T : HonooProperty
        {
            return _elements.Contains(value);
        }

        /// <summary>
        /// 从指定数组索引开始将值配置属性复制到到指定数组。
        /// </summary>
        /// <typeparam name="T">指定配置属性类型。</typeparam>
        /// <param name="array">要复制到的目标数组。</param>
        /// <param name="arrayIndex">目标数组中从零开始的索引，从此处开始复制。</param>
        public void CopyTo<T>(T[] array, int arrayIndex) where T : HonooProperty
        {
            _elements.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 支持在泛型集合上进行简单迭代。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<HonooProperty> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        /// <summary>
        /// 搜索指定对象，并返回第一个匹配项从零开始的索引。
        /// </summary>
        /// <typeparam name="T">指定配置属性类型。</typeparam>
        /// <param name="value">搜索的指定对象。</param>
        /// <returns></returns>
        public int IndexOf<T>(T value) where T : HonooProperty
        {
            return _elements.IndexOf(value);
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
            _elements.Insert(index, value);
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
            return _elements.Remove(value);
        }

        /// <summary>
        /// 从配置属性集合中移除指定索引处的配置属性。
        /// </summary>
        /// <param name="index">要移除的配置属性的索引。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public void RemoveAt(int index)
        {
            HonooProperty property = _elements[index];
            property?.Comment.Remove();
            property?.Content.Remove();
            _elements.RemoveAt(index);
        }
    }
}