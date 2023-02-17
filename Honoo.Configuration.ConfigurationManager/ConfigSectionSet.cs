using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Honoo.Configuration
{
    /// <summary>
    /// 配置容器集合。
    /// </summary>
    public sealed class ConfigSectionSet : IEnumerable<KeyValuePair<string, IConfigSection>>, IEnumerable
    {
        private readonly IDictionary<string, XElement> _contents = new Dictionary<string, XElement>();
        private readonly XElement _contentSuperior;
        private readonly IDictionary<string, XElement> _declarations = new Dictionary<string, XElement>();
        private readonly XElement _declarationSuperior;
        private readonly IDictionary<string, IConfigSection> _sections = new Dictionary<string, IConfigSection>();

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
        public ICollection<IConfigSection> Values => _sections.Values;

        /// <summary>
        /// 获取具有指定名称的配置容器的值。
        /// </summary>
        /// <param name="name">配置容器的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public IConfigSection this[string name] => _sections.TryGetValue(name, out IConfigSection section) ? section : null;

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
                    string typeName = declaration.Attribute("type").Value;
                    XElement content = contentSuperior.Element(name);
                    IConfigSection value;
                    switch (typeName)
                    {
                        case "System.Configuration.DictionarySectionHandler, System":
                        case "System.Configuration.DictionarySectionHandler":
                        case "DictionarySectionHandler":
                            value = new DictionarySection(content);
                            break;

                        case "System.Configuration.NameValueSectionHandler, System":
                        case "System.Configuration.NameValueSectionHandler":
                        case "NameValueSectionHandler":
                            value = new NameValueSection(content);
                            break;

                        case "System.Configuration.SingleTagSectionHandler, System":
                        case "System.Configuration.SingleTagSectionHandler":
                        case "SingleTagSectionHandler":
                            value = new SingleTagSection(content);
                            break;

                        case "Honoo.Configuration.CustumSectionHandler, Honoo":
                        case "Honoo.Configuration.CustumSectionHandler":
                        case "CustumSectionHandler":
                        default: value = new CustumSection(content); break;
                    }
                    _sections.Add(name, value);
                    _contents.Add(name, content);
                    _declarations.Add(name, declaration);
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
            _contents.Clear();
            _contentSuperior.RemoveNodes();
            _declarations.Clear();
            _declarationSuperior.RemoveNodes();
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
        /// <param name="type">配置容器的类型。</param>
        /// <exception cref="Exception"/>
        public IConfigSection GetOrAdd(string name, ConfigSectionType type)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"The invalid name - {nameof(name)}.");
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
                switch (type)
                {
                    case ConfigSectionType.CustumSection:
                        declaration.SetAttributeValue("type", "Honoo.Configuration.CustumSectionHandler");
                        value = new CustumSection(content);
                        break;

                    case ConfigSectionType.DictionarySection:
                        declaration.SetAttributeValue("type", "System.Configuration.DictionarySectionHandler");
                        value = new DictionarySection(content);
                        break;

                    case ConfigSectionType.NameValueSection:
                        declaration.SetAttributeValue("type", "System.Configuration.NameValueSectionHandler");
                        value = new NameValueSection(content);
                        break;

                    case ConfigSectionType.SingleTagSection:
                        declaration.SetAttributeValue("type", "System.Configuration.SingleTagSectionHandler");
                        value = new SingleTagSection(content);
                        break;

                    default: throw new ArgumentException($"The invalid type - {nameof(type)}.");
                }
                _sections.Add(name, value);
                _contents.Add(name, content);
                _contentSuperior.Add(content);
                _declarations.Add(name, declaration);
                _declarationSuperior.Add(declaration);
                return value;
            }
        }

        /// <summary>
        /// 从配置容器集合中移除带有指定名称的配置容器。
        /// </summary>
        /// <param name="name">配置容器的名称。</param>
        /// <returns></returns>
        /// <exception cref="Exception"/>
        public bool Remove(string name)
        {
            if (_sections.Remove(name))
            {
                _contents[name].Remove();
                _contents.Remove(name);
                _declarations[name].Remove();
                _declarations.Remove(name);
                return true;
            }
            else
            {
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
        public bool TryGetValue(string name, out CustumSection value)
        {
            if (_sections.TryGetValue(name, out IConfigSection val))
            {
                value = (CustumSection)val;
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
            if (_sections.TryGetValue(name, out IConfigSection val))
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
            if (_sections.TryGetValue(name, out IConfigSection val))
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
            if (_sections.TryGetValue(name, out IConfigSection val))
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
        public bool TryGetValue(string name, out IConfigSection value)
        {
            return _sections.TryGetValue(name, out value);
        }
    }
}