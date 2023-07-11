using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置容器集合。
    /// </summary>
    public sealed class ConfigSectionSet : IEnumerable<KeyValuePair<string, IConfigSection>>
    {
        #region Class

        /// <summary>
        /// 代表此配置容器集合的名称的集合。
        /// </summary>
        public sealed class NameCollection : IEnumerable<string>
        {
            #region Properties

            private readonly Dictionary<string, IConfigSection> _sections;

            /// <summary>
            /// 获取配置容器集合的名称的元素数。
            /// </summary>
            public int Count => _sections.Count;

            #endregion Properties

            internal NameCollection(Dictionary<string, IConfigSection> groups)
            {
                _sections = groups;
            }

            /// <summary>
            /// 从指定数组索引开始将名称元素复制到到指定数组。
            /// </summary>
            /// <param name="array">要复制到的目标数组。</param>
            /// <param name="arrayIndex">目标数组中从零开始的索引，从此处开始复制。</param>
            public void CopyTo(string[] array, int arrayIndex)
            {
                _sections.Keys.CopyTo(array, arrayIndex);
            }

            /// <summary>
            /// 返回循环访问集合的枚举数。
            /// </summary>
            /// <returns></returns>
            public IEnumerator<string> GetEnumerator()
            {
                return _sections.Keys.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _sections.Keys.GetEnumerator();
            }
        }

        /// <summary>
        /// 代表此配置容器集合的值的集合。
        /// </summary>
        public sealed class ValueCollection : IEnumerable<IConfigSection>
        {
            #region Properties

            private readonly Dictionary<string, IConfigSection> _sections;

            /// <summary>
            /// 获取配置容器集合的值的元素数。
            /// </summary>
            public int Count => _sections.Count;

            #endregion Properties

            internal ValueCollection(Dictionary<string, IConfigSection> groups)
            {
                _sections = groups;
            }

            /// <summary>
            /// 从指定数组索引开始将值元素复制到到指定数组。
            /// </summary>
            /// <param name="array">要复制到的目标数组。</param>
            /// <param name="arrayIndex">目标数组中从零开始的索引，从此处开始复制。</param>
            public void CopyTo(IConfigSection[] array, int arrayIndex)
            {
                _sections.Values.CopyTo(array, arrayIndex);
            }

            /// <summary>
            /// 返回循环访问集合的枚举数。
            /// </summary>
            /// <returns></returns>
            public IEnumerator<IConfigSection> GetEnumerator()
            {
                return _sections.Values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _sections.Values.GetEnumerator();
            }
        }

        #endregion Class

        #region Properties

        private readonly Dictionary<string, XComment> _comments = new Dictionary<string, XComment>();
        private readonly Dictionary<string, XElement> _contents = new Dictionary<string, XElement>();
        private readonly XElement _contentSuperior;
        private readonly Dictionary<string, XElement> _declarations = new Dictionary<string, XElement>();
        private readonly XElement _declarationSuperior;
        private readonly NameCollection _nameExhibits;
        private readonly Dictionary<string, IConfigSection> _sections = new Dictionary<string, IConfigSection>();
        private readonly ValueCollection _valueExhibits;

        /// <summary>
        /// 获取配置容器集合中包含的元素数。
        /// </summary>
        public int Count => _sections.Count;

        /// <summary>
        /// 获取配置容器集合的名称的集合。
        /// </summary>
        public NameCollection Names => _nameExhibits;

        /// <summary>
        /// 获取配置容器集合的值的集合。
        /// </summary>
        public ValueCollection Values => _valueExhibits;

        /// <summary>
        /// 获取具有指定名称的配置容器的值。
        /// <br/>取值时如果没有找到指定名称，返回 <see langword="null"/>。
        /// </summary>
        /// <param name="name">配置容器的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public IConfigSection this[string name] => _sections.TryGetValue(name, out IConfigSection section) ? section : null;

        #endregion Properties

        #region Construction

        internal ConfigSectionSet(XElement declarationSuperior, XElement contentSuperior)
        {
            _declarationSuperior = declarationSuperior;
            _contentSuperior = contentSuperior;
            if (declarationSuperior.HasElements)
            {
                foreach (XElement declaration in declarationSuperior.Elements("section"))
                {
                    string name = declaration.Attribute("name").Value;
                    string type = declaration.Attribute("type").Value;
                    XElement content = contentSuperior.Element(name);
                    IConfigSection value;
                    switch (type)
                    {
                        case "SingleTagSectionHandler":
                        case "System.Configuration.SingleTagSectionHandler":
                        case "System.Configuration.SingleTagSectionHandler, System.Configuration":
                            value = new SingleTagSection(content);
                            break;

                        case "NameValueSectionHandler":
                        case "System.Configuration.NameValueSectionHandler":
                        case "System.Configuration.NameValueSectionHandler, System.Configuration":
                            value = new NameValueSection(content);
                            break;

                        case "DictionarySectionHandler":
                        case "System.Configuration.DictionarySectionHandler":
                        case "System.Configuration.DictionarySectionHandler, System.Configuration":
                            value = new DictionarySection(content);
                            break;

                        case "TextSectionHandler":
                        case "Honoo.Configuration.TextSectionHandler":
                        case "Honoo.Configuration.TextSectionHandler, Honoo.Configuration":
                        default: value = new TextSection(content); break;
                    }
                    _sections.Add(name, value);
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
            _nameExhibits = new NameCollection(_sections);
            _valueExhibits = new ValueCollection(_sections);
        }

        #endregion Construction

        /// <summary>
        /// 从配置容器集合中移除所有配置容器。
        /// </summary>
        public void Clear()
        {
            _sections.Clear();
            _declarations.Clear();
            _declarationSuperior.RemoveNodes();
            _contents.Clear();
            _contentSuperior.RemoveNodes();
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
        public IEnumerator<KeyValuePair<string, IConfigSection>> GetEnumerator()
        {
            return _sections.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _sections.GetEnumerator();
        }

        /// <summary>
        /// 获取与指定名称关联的配置容器的值。如果不存在，添加一个配置容器并返回值。
        /// </summary>
        /// <param name="name">配置容器的名称。</param>
        /// <param name="kind">配置容器的类型。</param>
        /// <exception cref="Exception"/>
        public IConfigSection GetOrAdd(string name, ConfigSectionKind kind)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"The invalid argument - {nameof(name)}.");
            }
            if (_sections.TryGetValue(name, out IConfigSection section))
            {
                return section;
            }
            else
            {
                XElement declaration = new XElement("section");
                declaration.SetAttributeValue("name", name);
                XElement content = new XElement(name);
                IConfigSection value;
                switch (kind)
                {
                    case ConfigSectionKind.TextSection:
                        declaration.SetAttributeValue("type", "Honoo.Configuration.TextSectionHandler");
                        value = new TextSection(content);
                        break;

                    case ConfigSectionKind.SingleTagSection:
                        declaration.SetAttributeValue("type", "System.Configuration.SingleTagSectionHandler");
                        value = new SingleTagSection(content);
                        break;

                    case ConfigSectionKind.NameValueSection:
                        declaration.SetAttributeValue("type", "System.Configuration.NameValueSectionHandler");
                        value = new NameValueSection(content);
                        break;

                    case ConfigSectionKind.DictionarySection:
                        declaration.SetAttributeValue("type", "System.Configuration.DictionarySectionHandler");
                        value = new DictionarySection(content);
                        break;

                    default: throw new ArgumentException($"The invalid argument - {nameof(kind)}.");
                }
                _sections.Add(name, value);
                _declarations.Add(name, declaration);
                _contents.Add(name, content);
                _comments.Add(name, null);
                _contentSuperior.Add(content);
                _declarationSuperior.Add(declaration);
                return value;
            }
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
            if (_sections.Remove(name))
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
        /// 获取与指定名称关联的配置容器的注释。
        /// <br/>如果没有找到指定名称，返回 <see langword="false"/>。如果找到了指定名称但没有注释节点，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="name">配置容器的名称。</param>
        /// <param name="comment">配置容器的注释。</param>
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
        /// 获取与指定名称关联的配置容器的值。
        /// <br/>如果没有找到指定名称，返回 <see langword="false"/>。如果找到了指定名称但指定的类型不符，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="name">配置容器的名称。</param>
        /// <param name="value">配置容器的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string name, out TextSection value)
        {
            if (_sections.TryGetValue(name, out IConfigSection val))
            {
                if (val is TextSection section)
                {
                    value = section;
                    return true;
                }
            }
            value = null;
            return false;
        }

        /// <summary>
        /// 获取与指定名称关联的配置容器的值。
        /// <br/>如果没有找到指定名称，返回 <see langword="false"/>。如果找到了指定名称但指定的类型不符，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="name">配置容器的名称。</param>
        /// <param name="value">配置容器的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string name, out DictionarySection value)
        {
            if (_sections.TryGetValue(name, out IConfigSection val))
            {
                if (val is DictionarySection section)
                {
                    value = section;
                    return true;
                }
            }
            value = null;
            return false;
        }

        /// <summary>
        /// 获取与指定名称关联的配置容器的值。
        /// <br/>如果没有找到指定名称，返回 <see langword="false"/>。如果找到了指定名称但指定的类型不符，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="name">配置容器的名称。</param>
        /// <param name="value">配置容器的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string name, out NameValueSection value)
        {
            if (_sections.TryGetValue(name, out IConfigSection val))
            {
                if (val is NameValueSection section)
                {
                    value = section;
                    return true;
                }
            }
            value = null;
            return false;
        }

        /// <summary>
        /// 获取与指定名称关联的配置容器的值。
        /// <br/>如果没有找到指定名称，返回 <see langword="false"/>。如果找到了指定名称但指定的类型不符，则仍返回 <see langword="false"/>。
        /// </summary>
        /// <param name="name">配置容器的名称。</param>
        /// <param name="value">配置容器的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string name, out SingleTagSection value)
        {
            if (_sections.TryGetValue(name, out IConfigSection val))
            {
                if (val is SingleTagSection section)
                {
                    value = section;
                    return true;
                }
            }
            value = null;
            return false;
        }

        /// <summary>
        /// 获取与指定名称关联的配置容器的值。
        /// </summary>
        /// <param name="name">配置容器的名称。</param>
        /// <param name="value">配置容器的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string name, out IConfigSection value)
        {
            return _sections.TryGetValue(name, out value);
        }

        /// <summary>
        /// 添加或更新或删除一个与指定名称关联的配置容器的注释。
        /// </summary>
        /// <param name="name">配置容器的名称。</param>
        /// <param name="comment">配置容器的注释。</param>
        /// <exception cref="Exception"/>
        public bool TrySetComment(string name, string comment)
        {
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