﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置文件链接属性集合。
    /// </summary>
    public sealed class AssemblyBindingPropertySet : IEnumerable<LinkedConfigurationProperty>
    {
        #region Members

        private readonly XElement _container;
        private readonly List<LinkedConfigurationProperty> _properties = new List<LinkedConfigurationProperty>();

        /// <summary>
        /// 获取配置文件链接属性集合中包含的元素数。
        /// </summary>
        public int Count => _properties.Count;

        /// <summary>
        /// 获取或设置指定索引处的连接属性的值。
        /// </summary>
        /// <param name="index">连接属性的索引。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public LinkedConfigurationProperty this[int index]
        {
            get => _properties[index];
            set => SetValue(index, value);
        }

        #endregion Members

        #region Construction

        internal AssemblyBindingPropertySet(XElement container)
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
                            if (content.Name == (ConfigurationManager.AssemblyBindingNamespace + "linkedConfiguration"))
                            {
                                LinkedConfigurationProperty value = new LinkedConfigurationProperty(content, comment);
                                _properties.Add(value);
                            }
                        }
                        comment = null;
                    }
                }
            }
        }

        #endregion Construction

        /// <summary>
        /// 添加一个配置文件链接属性。
        /// </summary>
        /// <param name="value">配置文件链接属性的值。</param>
        /// <exception cref="Exception"/>
        public LinkedConfigurationProperty Add(LinkedConfigurationProperty value)
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
        /// 从配置文件链接属性集合中移除所有配置文件链接属性。
        /// </summary>
        /// <exception cref="Exception"/>
        public void Clear()
        {
            _container.RemoveNodes();
            _properties.Clear();
        }

        /// <summary>
        /// 确定指定配置文件链接属性是否在集合中。
        /// </summary>
        /// <param name="value">搜索的指定对象。</param>
        /// <returns></returns>
        public bool Contains(LinkedConfigurationProperty value)
        {
            return _properties.Contains(value);
        }

        /// <summary>
        /// 从指定数组索引开始将值配置文件链接属性复制到到指定数组。
        /// </summary>
        /// <param name="array">要复制到的目标数组。</param>
        /// <param name="arrayIndex">目标数组中从零开始的索引，从此处开始复制。</param>
        public void CopyTo(LinkedConfigurationProperty[] array, int arrayIndex)
        {
            _properties.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 支持在泛型集合上进行简单迭代。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<LinkedConfigurationProperty> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        /// <summary>
        /// 获取与指定索引处的配置文件链接属性的值。
        /// </summary>
        /// <param name="index">配置文件链接属性的索引。</param>
        /// <exception cref="Exception"/>
        public LinkedConfigurationProperty GetValue(int index)
        {
            return _properties[index];
        }

        /// <summary>
        /// 搜索指定对象，并返回第一个匹配项从零开始的索引。
        /// </summary>
        /// <param name="value">搜索的指定对象。</param>
        /// <returns></returns>
        public int IndexOf(LinkedConfigurationProperty value)
        {
            return _properties.IndexOf(value);
        }

        /// <summary>
        /// 将配置文件链接属性插入指定索引处。
        /// </summary>
        /// <param name="index">指定索引。</param>
        /// <param name="value">要插入的配置文件链接属性。</param>
        /// <exception cref="Exception"/>
        public LinkedConfigurationProperty Insert(int index, LinkedConfigurationProperty value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            _properties.Insert(index, value);
            return value;
        }

        /// <summary>
        /// 从配置文件链接属性集合中移除指定配置文件链接属性。
        /// </summary>
        /// <param name="value">要移除的配置文件链接属性。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool Remove(LinkedConfigurationProperty value)
        {
            value?.Comment.Remove();
            value?.Content.Remove();
            return _properties.Remove(value);
        }

        /// <summary>
        /// 从配置文件链接属性集合中移除指定索引处的配置文件链接属性。
        /// </summary>
        /// <param name="index">要移除的配置文件链接属性的索引。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public void RemoveAt(int index)
        {
            LinkedConfigurationProperty property = _properties[index];
            property?.Comment.Remove();
            property?.Content.Remove();
            _properties.RemoveAt(index);
        }

        /// <summary>
        /// 获取与指定索引处的配置文件链接属性的值。
        /// </summary>
        /// <param name="index">配置文件链接属性的索引。</param>
        /// <param name="value">配置文件链接属性的值。</param>
        /// <exception cref="Exception"/>
        public LinkedConfigurationProperty SetValue(int index, LinkedConfigurationProperty value)
        {
            RemoveAt(index);
            return Insert(index, value);
        }
    }
}