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
        /// 代表此配置组集合的名称的集合。
        /// </summary>
        public sealed class NameCollection : IEnumerable<string>
        {
            #region Properties

            private readonly Dictionary<string, ConfigSectionGroup> _groups;

            /// <summary>
            /// 获取配置组集合的名称的元素数。
            /// </summary>
            public int Count => _groups.Count;

            #endregion Properties

            internal NameCollection(Dictionary<string, ConfigSectionGroup> groups)
            {
                _groups = groups;
            }

            /// <summary>
            /// 从指定数组索引开始将名称元素复制到到指定数组。
            /// </summary>
            /// <param name="array">要复制到的目标数组。</param>
            /// <param name="arrayIndex">目标数组中从零开始的索引，从此处开始复制。</param>
            public void CopyTo(string[] array, int arrayIndex)
            {
                _groups.Keys.CopyTo(array, arrayIndex);
            }

            /// <summary>
            /// 返回循环访问集合的枚举数。
            /// </summary>
            /// <returns></returns>
            public IEnumerator<string> GetEnumerator()
            {
                return _groups.Keys.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _groups.Keys.GetEnumerator();
            }
        }

        /// <summary>
        /// 代表此配置组集合的值的集合。
        /// </summary>
        public sealed class ValueCollection : IEnumerable<ConfigSectionGroup>
        {
            #region Properties

            private readonly Dictionary<string, ConfigSectionGroup> _groups;

            /// <summary>
            /// 获取配置组集合的值的元素数。
            /// </summary>
            public int Count => _groups.Count;

            #endregion Properties

            internal ValueCollection(Dictionary<string, ConfigSectionGroup> groups)
            {
                _groups = groups;
            }

            /// <summary>
            /// 从指定数组索引开始将值元素复制到到指定数组。
            /// </summary>
            /// <param name="array">要复制到的目标数组。</param>
            /// <param name="arrayIndex">目标数组中从零开始的索引，从此处开始复制。</param>
            public void CopyTo(ConfigSectionGroup[] array, int arrayIndex)
            {
                _groups.Values.CopyTo(array, arrayIndex);
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
        }

        #endregion Class

        #region Properties

        private readonly Dictionary<string, XComment> _comments = new Dictionary<string, XComment>();
        private readonly Dictionary<string, XElement> _contents = new Dictionary<string, XElement>();
        private readonly XElement _contentSuperior;
        private readonly Dictionary<string, XElement> _declarations = new Dictionary<string, XElement>();
        private readonly XElement _declarationSuperior;
        private readonly Dictionary<string, ConfigSectionGroup> _groups = new Dictionary<string, ConfigSectionGroup>();
        private readonly NameCollection _nameExhibits;
        private readonly ValueCollection _valueExhibits;

        /// <summary>
        /// 获取配置组集合中包含的元素数。
        /// </summary>
        public int Count => _groups.Count;

        /// <summary>
        /// 获取配置组集合的名称的集合。
        /// </summary>
        public NameCollection Names => _nameExhibits;

        /// <summary>
        /// 获取配置组集合的值的集合。
        /// </summary>
        public ValueCollection Values => _valueExhibits;

        /// <summary>
        /// 获取具有指定名称的配置组的值。
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
                    ConfigSectionGroup value = new ConfigSectionGroup(declaration, content);
                    _groups.Add(name, value);
                    _declarations.Add(name, declaration);
                    _contents.Add(name, content);
                    XNode pre = content.PreviousNode;
                    if (pre != null && pre.NodeType == XmlNodeType.Comment)
                    {
                        _comments.Add(name, (XComment)pre);
                    }
                    else
                    {
                        _comments.Add(name, null);
                    }
                }
            }
            _nameExhibits = new NameCollection(_groups);
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
                throw new ArgumentException($"The invalid name - {nameof(name)}.");
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
                ConfigSectionGroup value = new ConfigSectionGroup(declaration, content);
                _groups.Add(name, value);
                _declarations.Add(name, declaration);
                _contents.Add(name, content);
                _comments.Add(name, null);
                _declarationSuperior.Add(declaration);
                _contentSuperior.Add(content);
                return value;
            }
        }

        /// <summary>
        /// 从配置组集合中移除带有指定名称的配置组。和指定名称关联的配置组的注释一并移除。
        /// </summary>
        /// <param name="name">配置组的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool Remove(string name)
        {
            if (_groups.Remove(name))
            {
                _declarations[name].Remove();
                _declarations.Remove(name);
                _contents[name].Remove();
                _contents.Remove(name);
                _comments[name]?.Remove();
                _comments.Remove(name);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取与指定名称关联的配置组的注释。
        /// <para/>如果没有找到指定名称，返回 false。如果找到了指定名称但没有注释节点，则仍返回 false。
        /// </summary>
        /// <param name="name">配置组的名称。</param>
        /// <param name="comment">配置组的注释。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetComment(string name, out string comment)
        {
            _comments.TryGetValue(name, out XComment comment_);
            if (comment_ == null)
            {
                comment = null;
                return false;
            }
            else
            {
                comment = comment_.Value;
                return true;
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

        /// <summary>
        /// 添加或更新或删除一个与指定名称关联的配置组的注释。
        /// </summary>
        /// <param name="name">配置组的名称。</param>
        /// <param name="comment">配置组的注释。</param>
        /// <exception cref="Exception"/>
        public bool TrySetComment(string name, string comment)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (comment == null)
            {
                if (_comments.TryGetValue(name, out XComment comment_))
                {
                    if (comment_ != null)
                    {
                        comment_.Remove();
                        _comments[name] = null;
                    }
                    return true;
                }
            }
            else
            {
                if (_comments.TryGetValue(name, out XComment comment_))
                {
                    if (comment_ == null)
                    {
                        comment_ = new XComment(comment);
                        _contents[name].AddBeforeSelf(comment_);
                        _comments[name] = comment_;
                    }
                    else
                    {
                        comment_.Value = comment;
                    }
                    return true;
                }
            }
            return false;
        }
    }
}