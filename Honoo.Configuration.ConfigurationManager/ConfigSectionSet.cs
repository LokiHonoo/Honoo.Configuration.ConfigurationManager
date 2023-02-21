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
    public sealed class ConfigSectionSet : IEnumerable<KeyValuePair<string, ConfigSection>>
    {
        private readonly Dictionary<string, XComment> _comments = new Dictionary<string, XComment>();
        private readonly Dictionary<string, XElement> _contents = new Dictionary<string, XElement>();
        private readonly XElement _contentSuperior;
        private readonly Dictionary<string, XElement> _declarations = new Dictionary<string, XElement>();
        private readonly XElement _declarationSuperior;
        private readonly Dictionary<string, ConfigSection> _sections = new Dictionary<string, ConfigSection>();

        /// <summary>
        /// 获取配置容器集合中包含的元素数。
        /// </summary>
        public int Count => _sections.Count;

        /// <summary>
        /// 获取配置容器集合的名称的集合。
        /// </summary>
        public ICollection<string> Names => _sections.Keys;

        /// <summary>
        /// 获取配置容器集合。
        /// </summary>
        public ICollection<ConfigSection> Values => _sections.Values;

        /// <summary>
        /// 获取具有指定名称的配置容器的值。
        /// </summary>
        /// <param name="name">配置容器的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public ConfigSection this[string name] => _sections.TryGetValue(name, out ConfigSection section) ? section : null;

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
                    ConfigSection value;
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
        /// 支持在泛型集合上进行简单迭代。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, ConfigSection>> GetEnumerator()
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
        /// <param name="type">配置容器的类型。</param>
        /// <exception cref="Exception"/>
        public ConfigSection GetOrAdd(string name, ConfigSectionType type)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"The invalid name - {nameof(name)}.");
            }
            if (_sections.TryGetValue(name, out ConfigSection section))
            {
                return section;
            }
            else
            {
                XElement declaration = new XElement("section");
                declaration.SetAttributeValue("name", name);
                XElement content = new XElement(name);
                ConfigSection value;
                switch (type)
                {
                    case ConfigSectionType.TextSection:
                        declaration.SetAttributeValue("type", "Honoo.Configuration.TextSectionHandler");
                        value = new TextSection(content);
                        break;

                    case ConfigSectionType.SingleTagSection:
                        declaration.SetAttributeValue("type", "System.Configuration.SingleTagSectionHandler");
                        value = new SingleTagSection(content);
                        break;

                    case ConfigSectionType.NameValueSection:
                        declaration.SetAttributeValue("type", "System.Configuration.NameValueSectionHandler");
                        value = new NameValueSection(content);
                        break;

                    case ConfigSectionType.DictionarySection:
                        declaration.SetAttributeValue("type", "System.Configuration.DictionarySectionHandler");
                        value = new DictionarySection(content);
                        break;

                    default: throw new ArgumentException($"The invalid type - {nameof(type)}.");
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
        /// <para/>如果没有找到指定名称，返回 false。如果找到了指定名称但没有注释节点，则仍返回 false。
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
        /// </summary>
        /// <param name="name">配置容器的名称。</param>
        /// <param name="value">配置容器的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string name, out TextSection value)
        {
            if (_sections.TryGetValue(name, out ConfigSection val))
            {
                value = (TextSection)val;
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        /// <summary>
        /// 获取与指定名称关联的配置容器的值。
        /// </summary>
        /// <param name="name">配置容器的名称。</param>
        /// <param name="value">配置容器的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string name, out DictionarySection value)
        {
            if (_sections.TryGetValue(name, out ConfigSection val))
            {
                value = (DictionarySection)val;
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        /// <summary>
        /// 获取与指定名称关联的配置容器的值。
        /// </summary>
        /// <param name="name">配置容器的名称。</param>
        /// <param name="value">配置容器的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string name, out NameValueSection value)
        {
            if (_sections.TryGetValue(name, out ConfigSection val))
            {
                value = (NameValueSection)val;
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        /// <summary>
        /// 获取与指定名称关联的配置容器的值。
        /// </summary>
        /// <param name="name">配置容器的名称。</param>
        /// <param name="value">配置容器的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string name, out SingleTagSection value)
        {
            if (_sections.TryGetValue(name, out ConfigSection val))
            {
                value = (SingleTagSection)val;
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        /// <summary>
        /// 获取与指定名称关联的配置容器的值。
        /// </summary>
        /// <param name="name">配置容器的名称。</param>
        /// <param name="value">配置容器的值。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool TryGetValue(string name, out ConfigSection value)
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