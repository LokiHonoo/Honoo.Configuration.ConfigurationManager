using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    ///  配置组集合。
    /// </summary>
    public sealed class ConfigSectionGroupSet : IEnumerable<ConfigSectionGroup>
    {
        #region Properties

        private readonly XElement _contentContainer;
        private readonly XElement _declarationContainer;
        private readonly Dictionary<string, ConfigSectionGroup> _groups = new Dictionary<string, ConfigSectionGroup>();

        /// <summary>
        /// 获取配置组集合中包含的元素数。
        /// </summary>
        public int Count => _groups.Count;
        /// <summary>
        /// 获取与指定名称关联的配置组。
        /// </summary>
        /// <param name="name">配置组的名称。</param>
        /// <returns></returns>
        public ConfigSectionGroup this[string name]=> GetValue(name);
        #endregion Properties

        #region Construction

        internal ConfigSectionGroupSet(XElement declarationContainer, XElement contentContainer)
        {
            _declarationContainer = declarationContainer;
            _contentContainer = contentContainer;
            if (_declarationContainer.HasElements)
            {
                foreach (XElement declaration in _declarationContainer.Elements("sectionGroup"))
                {
                    string name = declaration.Attribute("name").Value;
                    XElement content = _contentContainer.Element(name);
                    if (content != null)
                    {
                        XComment comment = null;
                        XNode pre = content.PreviousNode;
                        if (pre != null && pre.NodeType == XmlNodeType.Comment)
                        {
                            comment = (XComment)pre;
                        }
                        ConfigSectionGroup group = new ConfigSectionGroup(declaration, content, comment);
                        _groups.Add(name, group);
                    }
                }
            }
        }

        #endregion Construction

        #region GetOrAdd

        /// <summary>
        /// 获取与指定名称关联的配置组的值。如果不存在，添加一个配置组并返回值。
        /// </summary>
        /// <param name="name">配置组的名称。</param>
        /// <exception cref="Exception"/>
        public ConfigSectionGroup GetOrAdd(string name)
        {
            if (_groups.TryGetValue(name, out ConfigSectionGroup value))
            {
                return value;
            }
            else
            {
                XElement declaration = new XElement("sectionGroup");
                declaration.SetAttributeValue("name", name);
                XElement content = new XElement(name);
                ConfigSectionGroup group = new ConfigSectionGroup(declaration, content, null);
                _groups.Add(name, value);
                _declarationContainer.Add(declaration);
                _contentContainer.Add(content);
                return group;
            }
        }

        #endregion GetOrAdd

        #region TryGetValue

        /// <summary>
        /// 获取与指定名称关联的配置组的值。
        /// </summary>
        /// <param name="name">配置组的名称。</param>
        /// <param name="group">配置组的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string name, out ConfigSectionGroup group)
        {
            return _groups.TryGetValue(name, out group);
        }

        #endregion TryGetValue

        #region GetValue

        /// <summary>
        /// 获取与指定名称关联的配置组的值。
        /// </summary>
        /// <param name="name">配置组的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ConfigSectionGroup GetValue(string name)
        {
            return TryGetValue(name, out ConfigSectionGroup group) ? group : null;
        }

        #endregion GetValue

        /// <summary>
        /// 从配置组集合中移除所有配置组。
        /// </summary>
        public void Clear()
        {
            foreach (string name in _groups.Keys)
            {
                Remove(name);
            }
        }

        /// <summary>
        /// 确定配置组集合是否包含带有指定名称的配置组。
        /// </summary>
        /// <param name="name">配置组的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool Contains(string name)
        {
            return _groups.ContainsKey(name);
        }

        /// <summary>
        /// 返回循环访问集合的枚举数。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<ConfigSectionGroup> GetEnumerator()
        {
            return _groups.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _groups.Values.GetEnumerator();
        }

        /// <summary>
        /// 从配置组集合中移除带有指定名称的配置组。和指定名称关联的配置组的注释一并移除。
        /// <br/>如果该元素成功移除，返回 <see langword="true"/>。如果没有找到指定名称，则返回 <see langword="false"/>。
        /// </summary>
        /// <param name="name">配置组的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool Remove(string name)
        {
            if (_groups.TryGetValue(name, out ConfigSectionGroup value))
            {
                value.Comment?.Remove();
                value.Declaration.Remove();
                value.Content.Remove();
                _groups.Remove(name);
                return true;
            }
            return false;
        }
    }
}