﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置容器集合。
    /// </summary>
    public sealed class XSectionSet : IEnumerable<XDictionary>
    {
        #region Members

        private readonly XElement _container;
        private readonly Dictionary<string, XDictionary> _sections = new Dictionary<string, XDictionary>();

        /// <summary>
        /// 获取配置容器集合中包含的元素数。
        /// </summary>
        public int Count => _sections.Count;

        /// <summary>
        /// 获取与指定名称关联的配置容器。
        /// </summary>
        /// <param name="name">配置容器的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public XDictionary this[string name] => GetValue(name);

        #endregion Members

        #region Construction

        internal XSectionSet(XElement container)
        {
            _container = container;
            if (container.HasElements)
            {
                foreach (XElement content in container.Elements(XConfigManager.Namespace + "section"))
                {
                    string name = content.Attribute("name").Value;
                    XComment comment = null;
                    XNode pre = content.PreviousNode;
                    if (pre != null && pre.NodeType == XmlNodeType.Comment)
                    {
                        comment = (XComment)pre;
                    }
                    XDictionary section = new XDictionary(content, comment);
                    _sections.Add(name, section);
                }
            }
        }

        #endregion Construction

        #region Add

        /// <summary>
        /// 添加一个配置容器。
        /// </summary>
        /// <param name="name">配置容器的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public XDictionary Add(string name)
        {
            XElement content = new XElement(XConfigManager.Namespace + "section");
            content.SetAttributeValue("name", name);
            XDictionary section = new XDictionary(content, null);
            _sections.Add(name, section);
            _container.Add(content);
            return section;
        }

        #endregion Add

        #region GetOrAdd

        /// <summary>
        /// 获取与指定名称关联的配置容器的值。如果不存在，添加一个配置容器并返回值。
        /// </summary>
        /// <param name="name">配置容器的名称。</param>
        /// <exception cref="Exception"/>
        public XDictionary GetOrAdd(string name)
        {
            if (_sections.TryGetValue(name, out XDictionary value))
            {
                return value;
            }
            else
            {
                return Add(name);
            }
        }

        #endregion GetOrAdd

        #region TryGetValue

        /// <summary>
        /// 获取与指定名称关联的配置容器的值。
        /// </summary>
        /// <param name="name">配置容器的名称。</param>
        /// <param name="section">配置容器的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string name, out XDictionary section)
        {
            return _sections.TryGetValue(name, out section);
        }

        #endregion TryGetValue

        #region GetValue

        /// <summary>
        /// 获取与指定名称关联的配置容器的值。
        /// </summary>
        /// <param name="name">配置容器的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public XDictionary GetValue(string name)
        {
            return TryGetValue(name, out XDictionary section) ? section : null;
        }

        #endregion GetValue

        /// <summary>
        /// 从配置容器集合中移除所有配置容器。
        /// </summary>
        public void Clear()
        {
            foreach (string name in _sections.Keys)
            {
                Remove(name);
            }
        }

        /// <summary>
        /// 确定配置容器集合是否包含带有指定名称的配置容器。
        /// </summary>
        /// <param name="name">配置容器的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool ContainsName(string name)
        {
            return _sections.ContainsKey(name);
        }

        /// <summary>
        /// 返回循环访问集合的枚举数。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<XDictionary> GetEnumerator()
        {
            return _sections.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _sections.Values.GetEnumerator();
        }

        /// <summary>
        /// 从配置容器集合中移除带有指定名称的配置容器。和指定名称关联的配置容器的注释一并移除。
        /// <br/>如果该元素成功移除，返回 <see langword="true"/>。如果没有找到指定名称，则返回 <see langword="false"/>。
        /// </summary>
        /// <param name="name">配置容器的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool Remove(string name)
        {
            if (_sections.TryGetValue(name, out XDictionary value))
            {
                value.Comment.Remove();
                value.Content.Remove();
                _sections.Remove(name);
                return true;
            }
            return false;
        }
    }
}