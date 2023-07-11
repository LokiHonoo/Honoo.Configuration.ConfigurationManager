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
    public sealed class ConfigSectionGroupSet : IEnumerable<KeyValuePair<string, ConfigSectionGroup>>
    {
        #region Class

        /// <summary>
        /// 代表此配置属性集合的键的集合。
        /// </summary>
        public sealed class KeyCollection : IEnumerable<string>
        {
            #region Properties

            private readonly Dictionary<string, ConfigSectionGroup> _properties;

            /// <summary>
            /// 获取配置属性集合的键的元素数。
            /// </summary>
            public int Count => _properties.Count;

            #endregion Properties

            internal KeyCollection(Dictionary<string, ConfigSectionGroup> properties)
            {
                _properties = properties;
            }

            /// <summary>
            /// 从指定数组索引开始将键元素复制到到指定数组。
            /// </summary>
            /// <param name="array">要复制到的目标数组。</param>
            /// <param name="arrayIndex">目标数组中从零开始的索引，从此处开始复制。</param>
            public void CopyTo(string[] array, int arrayIndex)
            {
                _properties.Keys.CopyTo(array, arrayIndex);
            }

            /// <summary>
            /// 返回循环访问集合的枚举数。
            /// </summary>
            /// <returns></returns>
            public IEnumerator<string> GetEnumerator()
            {
                return _properties.Keys.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _properties.Keys.GetEnumerator();
            }
        }

        /// <summary>
        /// 代表此配置属性集合的值的集合。
        /// </summary>
        public sealed class ValueCollection : IEnumerable<ConfigSectionGroup>
        {
            #region Properties

            private readonly Dictionary<string, ConfigSectionGroup> _properties;

            /// <summary>
            /// 获取配置属性集合的值的元素数。
            /// </summary>
            public int Count => _properties.Count;

            #endregion Properties

            internal ValueCollection(Dictionary<string, ConfigSectionGroup> properties)
            {
                _properties = properties;
            }

            /// <summary>
            /// 从指定数组索引开始将值元素复制到到指定数组。
            /// </summary>
            /// <param name="array">要复制到的目标数组。</param>
            /// <param name="arrayIndex">目标数组中从零开始的索引，从此处开始复制。</param>
            public void CopyTo(ConfigSectionGroup[] array, int arrayIndex)
            {
                _properties.Values.CopyTo(array, arrayIndex);
            }

            /// <summary>
            /// 返回循环访问集合的枚举数。
            /// </summary>
            /// <returns></returns>
            public IEnumerator<ConfigSectionGroup> GetEnumerator()
            {
                return _properties.Values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _properties.Values.GetEnumerator();
            }
        }

        #endregion Class

        #region Properties

        private readonly Dictionary<string, XElement> _contents = new Dictionary<string, XElement>();
        private readonly XElement _contentSuperior;
        private readonly Dictionary<string, XElement> _declarations = new Dictionary<string, XElement>();
        private readonly XElement _declarationSuperior;
        private readonly Dictionary<string, ConfigSectionGroup> _groups = new Dictionary<string, ConfigSectionGroup>();
        private readonly KeyCollection _keyExhibits;
        private readonly ValueCollection _valueExhibits;

        /// <summary>
        /// 获取配置组集合中包含的元素数。
        /// </summary>
        public int Count => _groups.Count;

        /// <summary>
        /// 获取配置组集合的名称的集合。
        /// </summary>
        public KeyCollection Names => _keyExhibits;

        /// <summary>
        /// 获取配置组集合的值的集合。
        /// </summary>
        public ValueCollection Values => _valueExhibits;

        /// <summary>
        /// 获取具有指定名称的配置组的值。
        /// <br/>取值时如果没有找到指定名称，返回 <see langword="null"/>。
        /// </summary>
        /// <param name="name">配置组的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ConfigSectionGroup this[string name] => _groups.TryGetValue(name, out ConfigSectionGroup group) ? group : null;

        #endregion Properties

        #region Construction

        internal ConfigSectionGroupSet(XElement declarationSuperior, XElement contentSuperior)
        {
            _declarationSuperior = declarationSuperior;
            _contentSuperior = contentSuperior;
            if (declarationSuperior.HasElements)
            {
                foreach (XElement declaration in declarationSuperior.Elements("sectionGroup"))
                {
                    string name = declaration.Attribute("name").Value;
                    XElement content = contentSuperior.Element(name);
                    if (content != null)
                    {
                        XComment comment = null;
                        XNode pre = content.PreviousNode;
                        if (pre != null && pre.NodeType == XmlNodeType.Comment)
                        {
                            comment = (XComment)pre;
                        }
                        ConfigSectionGroup value = new ConfigSectionGroup(declaration, content, comment);
                        _groups.Add(name, value);
                        _declarations.Add(name, declaration);
                        _contents.Add(name, content);
                    }
                }
            }
            _keyExhibits = new KeyCollection(_groups);
            _valueExhibits = new ValueCollection(_groups);
        }

        #endregion Construction

        /// <summary>
        /// 从配置组集合中移除所有配置组。
        /// </summary>
        public void Clear()
        {
            _groups.Clear();
            _declarations.Clear();
            _declarationSuperior.RemoveNodes();
            _contents.Clear();
            _contentSuperior.RemoveNodes();
        }

        /// <summary>
        /// 确定配置组集合是否包含带有指定名称的配置组。
        /// </summary>
        /// <param name="name">配置组的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool ContainsName(string name)
        {
            return _groups.ContainsKey(name);
        }

        /// <summary>
        /// 返回循环访问集合的枚举数。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, ConfigSectionGroup>> GetEnumerator()
        {
            return _groups.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _groups.GetEnumerator();
        }

        /// <summary>
        /// 获取与指定名称关联的配置组的值。如果不存在，添加一个配置组并返回值。
        /// </summary>
        /// <param name="name">配置组的名称。</param>
        /// <exception cref="Exception"/>
        public ConfigSectionGroup GetOrAdd(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"The invalid argument - {nameof(name)}.");
            }
            if (_groups.TryGetValue(name, out ConfigSectionGroup group))
            {
                return group;
            }
            else
            {
                XElement declaration = new XElement("sectionGroup");
                declaration.SetAttributeValue("name", name);
                XElement content = new XElement(name);
                ConfigSectionGroup value = new ConfigSectionGroup(declaration, content, null);
                _groups.Add(name, value);
                _declarations.Add(name, declaration);
                _contents.Add(name, content);
                _declarationSuperior.Add(declaration);
                _contentSuperior.Add(content);
                return value;
            }
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
            if (_groups.TryGetValue(name, out ConfigSectionGroup group))
            {
                group.RemoveComment();
                _declarations[name].Remove();
                _declarations.Remove(name);
                _contents[name].Remove();
                _contents.Remove(name);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取与指定名称关联的配置组的值。
        /// </summary>
        /// <param name="name">配置组的名称。</param>
        /// <param name="value">配置组的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string name, out ConfigSectionGroup value)
        {
            return _groups.TryGetValue(name, out value);
        }
    }
}